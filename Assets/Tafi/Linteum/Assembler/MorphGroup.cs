using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Skeleton Morph Group", menuName = "Character/Morph (Group)", order = 0)]
public class MorphGroup : ScriptableObject
{
    public BaseCharacter OptionalBaseCharacter;
    public SkeletonMorph[] Morphs;
}
