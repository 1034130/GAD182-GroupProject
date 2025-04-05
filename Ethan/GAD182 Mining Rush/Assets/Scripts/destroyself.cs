using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyself : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, t: UnityEngine.Random.Range(0.5f, 1.5f));
        Debug.Log("Destroyed");
    }
}
