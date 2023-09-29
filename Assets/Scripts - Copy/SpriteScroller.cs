using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is to handle the parallax background.
public class SpriteScroller : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////

    [SerializeField] Vector2 moveSpeed;
    Vector2 offset;
    Material material;


    // ////////////////////////////////////////
    // //////////// MONO BEHAVIORS ////////////
    // ////////////////////////////////////////

    void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        offset = moveSpeed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
