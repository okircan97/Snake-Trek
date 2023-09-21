using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSpinner : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * (Random.Range(5, 30) * Time.deltaTime));
    }
}
