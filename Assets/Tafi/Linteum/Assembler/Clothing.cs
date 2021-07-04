using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Clothing", menuName = "Character/Clothing", order = 0)]
public class Clothing : ScriptableObject
{
    [Header("Prefab (Required)")]
    public GameObject ModelPrefab;

    [HideInInspector]
    public ClothingInternalData Internals;
    [HideInInspector]
    public ClothingInternalData[] InternalsMany; // Some clothing items have multiple skinnedmeshrenderers.

    [Header("Clothing Key Data (Required if not attachable)")]
    public BaseCharacter BaseCharacter;
    public MaterialData[] Materials;

    [Header("Feet Deletion")]
    public bool DeleteFeet;
    public float DeleteFeetHeight = 0.1f;

    [Header("Attachment Information (Optional)")]
    public bool IsNonSkinnedAttachment;
    public string AttachBone;
    public Vector3 AttachPosition, AttachRotation, AttachScale;

    [Serializable]
    public class MaterialData
    {
        public Material Material;
        public MaterialAction BakeAction;

        [Header("Optional Overrides")]
        public Material OverrideHDRP2018Material;
        public Material OverrideHDRP2019Material;
        public Material OverrideURP2019Material;
        public Material OverrideStandardMaterial;

        public enum MaterialAction
        {
            Merge,
            Leave,
            LeaveOrStrip,
        }
    }

    [Serializable]
    public class ClothingInternalData
    {
        public string MeshName;
        public Vector3[] OriginalVertices;
        public Vector3[] OriginalNormals;
        public TriangleData[] OriginalTriangles;
        public int[] Triangles;
        public Vector3[] Barycentric;
        public Vector3[] ClosestPoint;
    }

    [Serializable]
    public class TriangleData
    {
        public int SubMesh;
        public int[] Triangles;
    }
}
