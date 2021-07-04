using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Skeleton Morph", menuName = "Character/Morph", order = 0)]
public class SkeletonMorph : ScriptableObject
{
    public Mesh ImportFrom;
    public string ImportName;
    public BaseCharacter OptionalBaseCharacter;

    [HideInInspector]
    public Vector3[] DeltaVertices;
    [HideInInspector]
    public Vector3[] DeltaNormals;
    [HideInInspector]
    public Vector3[] DeltaTangents;

    [HideInInspector]
    public string[] BoneNames;
    [HideInInspector]
    public Matrix4x4[] BindPoses;
    [HideInInspector]
    public bool LocalPositions = true;
    [HideInInspector]
    public Vector3[] BonePositions;
    [HideInInspector]
    public Vector3[] BoneRotations;
    [HideInInspector]
    public Vector3[] BoneScales;
}
