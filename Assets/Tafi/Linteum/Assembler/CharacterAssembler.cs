using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Customised Character", menuName = "Character/Create New Character...", order = 0)]
public class CharacterAssembler : ScriptableObject
{
    [HideInInspector]
    public string[] Morphs;

    [HideInInspector]
    public bool[] MorphEnabled;

    [HideInInspector]
    public int[] TargetPolyCount;
    [HideInInspector]
    public int[] MaxPolyCount;

    [HideInInspector]
    public int CharTargetPolyCount;
    [HideInInspector]
    public int CharMaxPolyCount;

    [Header("Character")]
    public BaseCharacter BaseCharacter;
    public Character Character;

    [Header("Clothing")]
    public Clothing[] Clothing;
    
    [Header("Extra Morphs")]
    public SkeletonMorph[] AdditionalMorphs;

    [Header("Miscellaneous")]
    public bool DeleteHiddenSurfaces;
    public float HiddenSurfaceRangeMin = 0.02f;
    public float HiddenSurfaceRangeMax = 0.005f;
    public bool UnwindHiddenSurfacesByOne = true;
    public bool StripMorphs = true;
    public bool StripMorphsFromClothing = true;
    public bool TrimOrphanVertices = false;

    [Header("Vertex Re-Ordering")]
    public bool OptimiseMesh = true;
    public bool OptimiseGameObjects = true;

    [Header("Render Pipeline Selection")]
    public ShaderSet Shaders;

    //[Header("Materials")]
    public bool MergeDuplicateCharacterMaterials = true;
    public bool MergeDuplicateClothingMaterials = true;

    [Header("Textures")]
    public bool AtlasTextures;
    public TextureResolution AtlasResolution = TextureResolution.Huge4096x4096;

    public bool StripTextures;
    public bool IncludeAlbedo = true, IncludeNormal = true, IncludeSpecular = true;

    [NonSerialized]
    private Clothing[] _lastSavedClothing = null;

    public void OnValidate()
    {
        if (BaseCharacter != null && BaseCharacter.Internals != null)
        {
            CharMaxPolyCount = BaseCharacter.Internals.AllTriangles.Length / 3;
        }

        if (BaseCharacter != null && BaseCharacter.Internals != null && BaseCharacter.Internals.AvailableMorphs != null)
        {
            Morphs = BaseCharacter.Internals.AvailableMorphs;
            if (MorphEnabled == null || Morphs.Length != MorphEnabled.Length)
                MorphEnabled = new bool[Morphs.Length];
        }

        if (Clothing != null && TargetPolyCount == null || (Clothing != null && TargetPolyCount.Length != Clothing.Length) || _lastSavedClothing == null || (Clothing != null && !Enumerable.SequenceEqual(_lastSavedClothing,Clothing)))
        {
            _lastSavedClothing = (Clothing[]) Clothing.Clone();

            TargetPolyCount = new int[Clothing.Length];
            MaxPolyCount = new int[Clothing.Length];
            for (int i = 0; i < MaxPolyCount.Length; i++)
            {
                if (Clothing[i] != null)
                {
                    if (Clothing[i].InternalsMany != null)
                    {
                        if (Clothing[i].InternalsMany.Length > 0)
                        {
                            int sum = 0;
                            foreach (var multipleInternalData in Clothing[i].InternalsMany)
                            {
                                int localSum = 0;
                                foreach (var t in multipleInternalData.OriginalTriangles)
                                {
                                    localSum += t.Triangles.Length / 3;
                                }

                                sum += localSum;
                            }

                            MaxPolyCount[i] = sum;
                        }
                        else
                        {
                            MaxPolyCount[i] = Clothing[i].Internals.OriginalTriangles.Sum(t => t.Triangles.Length / 3);
                        }
                    }
                    else
                    {
                        MaxPolyCount[i] = Clothing[i].Internals.OriginalTriangles.Sum(t => t.Triangles.Length / 3);
                    }
                }
            }
        }
    }

    public enum ShaderSet
    {
        AutoDetect,
        Standard,
        UniversalRenderPipeline,
        HighDefinitionRenderPipeline2018,
        HighDefinitionRenderPipeline2019,
    }

    public enum TextureResolution : int
    {
        Micro128x128 = 128,
        Tiny256x256 = 256,
        Small512x512 = 512,
        Medium1024x1024 = 1024,
        Large2048x2048 = 2048,
        Huge4096x4096 = 4096,
        Gigantic8192x8192 = 8192
    }
}
