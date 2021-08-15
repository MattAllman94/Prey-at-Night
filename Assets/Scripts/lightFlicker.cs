using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlicker : MonoBehaviour
{

    Light light;

    public Material litMat;
    public Material offMat;

    void Start()
    {
        light = GetComponent<Light>();

        StartCoroutine(FlickerLight());
    }


    IEnumerator FlickerLight()
    {
        light.enabled = true;
        this.gameObject.GetComponent<MeshRenderer>().material = litMat;
        yield return new WaitForSeconds(Random.Range(0.1f, 3f));
        light.enabled = false;
        this.gameObject.GetComponent<MeshRenderer>().material = offMat;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        StartCoroutine(FlickerLight());
    }
}
