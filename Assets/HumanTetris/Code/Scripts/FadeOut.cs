using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    MeshRenderer rend;

    void Start()
    {
        rend = GetComponentInChildren<MeshRenderer>();
    }

    IEnumerator _FadeOut()
    {
        for(float fade = 1f; fade >= -0.2f; fade -= 0.075f)
        {
            Color c = rend.material.GetColor("_BaseColor");
            c.a = fade;
            rend.material.SetColor("_BaseColor", c);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void startFading()
    {
        Debug.Log("fading started!");
        StartCoroutine(_FadeOut());
    }
}
