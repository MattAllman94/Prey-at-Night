using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Linteum.Editor.Public;
using Unity.Jobs;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class UnsetAssetStoreCookie 
{
    [MenuItem("Tools/Unset Asset Store Login")]
    public static void UnsetAssetStore()
    {
        EditorApplication.ExecuteMenuItem("Asset Store Tools/Log Out");
        EditorPrefs.SetString("kharma.sessionid", string.Empty);

    }

    [MenuItem("Validation/Validate Before Upload", false, 100)]
    public static void ValidatePackage()
    {
        /*
        - 0) Has someone checked the product works, looks correct, and has no errors?
        - 1) Have I deleted the .dtu files?
        - 2) Have I renamed the folder to the product name?
        - 3) Have I included or removed the dependency folders?
        - 4) Have I removed any previous clothing/characters from the uploads?
        - 5) Have I removed the Sample Dependencies (folder) & "Example Assembly" (prefab)?
        - 6) Have I edited the "Example Outfit", and removed the 'Clay' character from the slot (just set to empty)?
        - 7) Have I edited the import settings on the FBX and recalculated the data?
         */

        if (EditorUtility.DisplayDialog("Directory is renamed", "Has the product directory name been renamed?", "Yes",
            "No"))
        {
            SearchAndDestroyDTU();
            ExampleDestroy();
            DoSanityChecks();
            RemoveCharacterFromSampleOutfits();
            RecalculateClothingData();

            if(EditorUtility.DisplayDialog("Checks completed","Ready to open the uploader?\n\n(Make sure to select the right item!)", "Yes", "Not now"))
            {
                EditorApplication.ExecuteMenuItem("Asset Store Tools/Package Upload");
            }
        }
    }

    [MenuItem("Tools/Import Unitypackage Directory", false, 100)]
    public static void ImportAWholeBunchOfUnityPackages()
    {
        var packages = EditorUtility.OpenFolderPanel("Where?", "", "");
        var files = Directory.GetFiles(packages, "*.unitypackage");
        try
        {
            for (var i = 0; i < files.Length; i++)
            {
                var pack = files[i];
                EditorUtility.DisplayProgressBar("Importing ", pack, i / (float) files.Length);
                
                AssetDatabase.ImportPackage(pack, false);
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    private static void RecalculateClothingData()
    {
        {
            var clothingModels = AssetDatabase.FindAssets("t:model", new[] {"Assets/Tafi/Clothing"})
                .Select(AssetDatabase.GUIDToAssetPath);
            foreach (var assetPath in clothingModels)
            {
                ModelImporter mi = (ModelImporter) AssetImporter.GetAtPath(assetPath);
                mi.importNormals = ModelImporterNormals.Import;
                mi.importTangents = ModelImporterTangents.Import;
#pragma warning disable CS0618 // Type or member is obsolete
                mi.optimizeMesh = false;
#pragma warning restore CS0618 // Type or member is obsolete
                mi.importBlendShapeNormals = ModelImporterNormals.Import;
                mi.weldVertices = false;
                mi.isReadable = true;

                mi.SaveAndReimport();
            }
        }

        var clothingItems = AssetDatabase.FindAssets("t:Clothing", new[] { "Assets/Tafi/Clothing" })
            .Select(AssetDatabase.GUIDToAssetPath);
        foreach (var assetPath in clothingItems)
        {
            var clothing = AssetDatabase.LoadAssetAtPath<Clothing>(assetPath);
            if (clothing != null)
            {
                var mats = clothing.Materials;
                ClothingEditor.SetupClothing(clothing);

                if (mats.Any(m => m.OverrideStandardMaterial != null) ||
                    mats.Any(m => m.OverrideHDRP2018Material != null) ||
                    mats.Any(m => m.OverrideHDRP2019Material != null) ||
                    mats.Any(m => m.OverrideURP2019Material != null))
                {
                    clothing.Materials = mats;
                }

                try
                {
                    EditorUtility.SetDirty(clothing);
                }
                catch (ArgumentNullException)
                {
                    EditorUtility.DisplayDialog("Error",
                        "Unity threw an error trying to save changes to " + assetPath +
                        " you may want to select the item, and press 'Import Data from Model'.", "OK");
                    throw new OperationCanceledException();
                }
            }
        }

        {
            var morphModels = AssetDatabase.FindAssets("t:model", new[] { "Assets/Tafi/Characters" })
                .Select(AssetDatabase.GUIDToAssetPath);
            foreach (var assetPath in morphModels)
            {
                ModelImporter mi = (ModelImporter)AssetImporter.GetAtPath(assetPath);
                mi.importNormals = ModelImporterNormals.Import;
                mi.importTangents = ModelImporterTangents.Import;
#pragma warning disable CS0618 // Type or member is obsolete
                mi.optimizeMesh = false;
#pragma warning restore CS0618 // Type or member is obsolete
                mi.importBlendShapeNormals = ModelImporterNormals.Import;
                mi.weldVertices = false;
                mi.isReadable = true;

                mi.SaveAndReimport();
            }
        }

        var chars = AssetDatabase.FindAssets("t:SkeletonMorph", new[] { "Assets/Tafi/Characters" })
            .Select(AssetDatabase.GUIDToAssetPath);
        foreach (var assetPath in chars)
        {
            var morph = AssetDatabase.LoadAssetAtPath<SkeletonMorph>(assetPath);
            if (morph != null)
            {
                if (morph.ImportName != "____BASE" || morph.OptionalBaseCharacter == null)
                {
                    EditorUtility.DisplayDialog("Error",
                        "Skeleton morph "+assetPath+" is not marked as ____BASE, or base character is missing.", "OK");
                    throw new OperationCanceledException();
                }

                SkeletonMorphEditor.ImportData(morph, morph.OptionalBaseCharacter);

                try
                {
                    EditorUtility.SetDirty(morph);
                }
                catch (ArgumentNullException)
                {
                    EditorUtility.DisplayDialog("Error",
                        "Unity threw an error trying to save changes to " + assetPath +
                        " you may want to select the item, and press 'Import From Base'.", "OK");
                    throw new OperationCanceledException();
                }
            }
        }

        AssetDatabase.SaveAssets();

    }

    private static void RemoveCharacterFromSampleOutfits()
    {
        var assemblers = AssetDatabase.FindAssets("t:CharacterAssembler", new[] {"Assets/Tafi"})
            .Select(AssetDatabase.GUIDToAssetPath);
        foreach (var assetPath in assemblers)
        {
            if (assetPath.Contains("Example Outfit"))
            {
                var ca = AssetDatabase.LoadAssetAtPath<CharacterAssembler>(assetPath);
                if (ca.Character != null)
                {
                    ca.Character = null;
                    try
                    {
                        EditorUtility.SetDirty(ca);
                        AssetDatabase.SaveAssets();
                    }
                    catch (ArgumentNullException)
                    {
                        EditorUtility.DisplayDialog("Error",
                            "Unity threw an error trying to save changes to " + assetPath +
                            " you may want to remove the character yourself and re-run this tool.", "OK");
                        throw new OperationCanceledException();
                    }
                }
            }
        }
    }

    [MenuItem("Validation/Passes/Basic Checks")]
    private static void DoSanityChecks()
    {
        var numFemaleClothes = Directory.GetDirectories("Assets/Tafi/Clothing/Female", "*", SearchOption.TopDirectoryOnly)
            .Length;
        var numMaleClothes = Directory.GetDirectories("Assets/Tafi/Clothing/Male", "*", SearchOption.TopDirectoryOnly)
            .Length;
        var numCharacters =
            Directory.GetDirectories("Assets/Tafi/Characters/Female", "*", SearchOption.TopDirectoryOnly).Length +
            Directory.GetDirectories("Assets/Tafi/Characters/Male", "*", SearchOption.TopDirectoryOnly).Length;
        var archetypes = Directory.GetDirectories("Assets/Tafi/Archetypes", "*", SearchOption.TopDirectoryOnly);
        var numArchetypes = archetypes.Length - 1;
        var numFemaleMorphs = Directory.GetDirectories("Assets/Tafi/Morphs/Female", "*", SearchOption.TopDirectoryOnly)
            .Length;
        var numMaleMorphs = Directory.GetDirectories("Assets/Tafi/Morphs/Male", "*", SearchOption.TopDirectoryOnly).Length;

        if (numFemaleClothes + numMaleClothes + numCharacters > 1)
        {
            if (EditorUtility.DisplayDialog("Warning",
                "There is more than one product (male clothing, female clothing, characters)", "Abort",
                "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numFemaleClothes + numMaleClothes + numCharacters == 0)
        {
            if (EditorUtility.DisplayDialog("Warning",
                "No product? (male clothing, female clothing, characters)", "Abort",
                "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numArchetypes > 1)
        {
            if (EditorUtility.DisplayDialog("Warning", "There is more than one base character archetype in this package.",
                "Abort", "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numFemaleClothes > 1)
        {
            if (EditorUtility.DisplayDialog("Warning", "There is more than one female clothing item", "Abort",
                "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numMaleMorphs > 1)
        {
            if (EditorUtility.DisplayDialog("Warning", "There is more than one male clothing item", "Abort",
                "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numCharacters > 1)
        {
            if (EditorUtility.DisplayDialog("Warning", "There is more than one character item", "Abort", "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numMaleClothes > 0 && numCharacters == 0 && numFemaleMorphs > 0)
        {
            if (EditorUtility.DisplayDialog("Warning",
                "Female morphs are detected, but package only contains male clothing.", "Abort", "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numFemaleClothes > 0 && numCharacters == 0 && numMaleMorphs > 0)
        {
            if (EditorUtility.DisplayDialog("Warning",
                "Male morphs are detected, but package only contains female clothing.", "Abort", "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numMaleClothes > 0 && !archetypes.Any(a => a.Contains("Genesis 8 Male")))
        {
            if (EditorUtility.DisplayDialog("Warning", "Male clothing detected, but no Genesis 8 male archetype.", "Abort",
                "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numFemaleClothes > 0 && !archetypes.Any(a => a.Contains("Genesis 8 Female")))
        {
            if (EditorUtility.DisplayDialog("Warning", "Female clothing detected, but no Genesis 8 female archetype.",
                "Abort", "Proceed Anyway"))
                throw new OperationCanceledException();
        }

        if (numCharacters > 1)
        {
            if (EditorUtility.DisplayDialog("Warning", "There is more than one character item", "Abort", "Proceed Anyway"))
                throw new OperationCanceledException();
        }
    }

    [MenuItem("Validation/Passes/Destroy Examples")]
    public static void ExampleDestroy()
    {
        bool changed = false;
        var allDirectories = Directory.GetDirectories("Assets/Tafi", "*", SearchOption.AllDirectories);
        foreach (var directory in allDirectories)
        {
            if (directory.Contains("Sample Dependencies"))
            {
                if (Directory.Exists(directory))
                    Directory.Delete(directory, true);
                changed = true;
            }
        }

        var prefabs = Directory.GetFiles("Assets/Tafi", "*.prefab", SearchOption.AllDirectories);
        foreach (var prefab in prefabs)
        {
            if (prefab.Contains("Example Assembly"))
            {
                File.Delete(prefab);
                changed = true;
            }
        }

        if(changed)
            AssetDatabase.Refresh();
    }

    [MenuItem("Validation/Passes/Destroy DTUs")]
    public static void SearchAndDestroyDTU()
    {
        bool changed = false;
        var dtus = Directory.GetFiles("Assets/Tafi", "*.dtu", SearchOption.AllDirectories);
        foreach (var dtu in dtus)
        {
            if (File.Exists(dtu))
            {
                Debug.Log("Deleting " + dtu);
                File.Delete(dtu);
                changed = true;
            }
        }

        if(changed)
            AssetDatabase.Refresh();
    }

    
}
