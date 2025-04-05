using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spawning : MonoBehaviour
{
    new public GameObject mineral;

    private void Start()
    {
        InvokeRepeating("SpawnMineral", 1f, UnityEngine.Random.Range(0.1f, 0.5f));
    }
    void SpawnMineral()
    {
        Object.Instantiate(mineral, new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f)), Quaternion.identity);
        Debug.Log("Spawned");
    }
}
