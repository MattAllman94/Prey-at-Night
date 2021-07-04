using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character/Character Pack", order = 0)]
public class Character : ScriptableObject
{
    [Header("Optional Fields")]
    public Material[] ReplacementMaterialSets = new Material[16];
    public Avatar ReplacementAvatar;
    public SkeletonMorph BaseMorph;
}