using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseCharacter", menuName = "Character/Base Character", order = 0)]
public class BaseCharacter : ScriptableObject
{
    public GameObject ModelPrefab;
    public MaterialData[] Materials;
    [HideInInspector]
    public BaseCharacterInternalData Internals;

    [Serializable]
    public class MaterialData
    {
        public Material Material;
        public MaterialAction BakeAction;

        public enum MaterialAction
        {
            Merge,
            Leave,
            LeaveOrStrip,
        }
    }

    [Serializable]
    public class BaseCharacterInternalData
    {
        public Vector3[] Vertices;
        public TriangleData[] Triangles;
        public int[] AllTriangles;
        public Vector2[] UVs;
        public string[] AvailableMorphs;
    }

    [Serializable]
    public class TriangleData
    {
        public int SubMesh;
        public int[] Triangles;
    }
}