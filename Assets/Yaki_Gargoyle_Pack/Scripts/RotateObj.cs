using UnityEngine;
using System.Collections;

namespace Yaki_Gargoyle_Pack
{
    public class RotateObj : MonoBehaviour
    {

        void Update()
        {
            transform.Rotate(new Vector3(0, -1.0f, 0));
        }
    }
}