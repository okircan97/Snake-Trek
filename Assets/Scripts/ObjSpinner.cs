using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSpinner : MonoBehaviour
{
    [SerializeField] int minRange;
    [SerializeField] int maxRange;

    void Update()
    {
        transform.Rotate(Vector3.up * (Random.Range(minRange, maxRange) * Time.deltaTime));
    }
}
