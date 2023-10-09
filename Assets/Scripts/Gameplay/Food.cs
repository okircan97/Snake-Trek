using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region  FIELDS

    public BoxCollider gridArea;
    Vector3 rotationVector;
    public Animator animator;
    bool foodAnimPlaying;

    #endregion FIELDS

    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region  MONOBEHAVIORS

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        RandomizePosition();
    }

    void Update()
    {
        transform.Rotate(rotationVector * (Random.Range(5, 30) * Time.deltaTime));
    }

    #endregion

    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region METHODS

    // This method is to get a random coordinate between the given bounds.
    public void RandomizePosition()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        Bounds bounds = gridArea.bounds;
        float randX = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float randY = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        transform.position = new Vector3(randX, randY, -10);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rotationVector = GetRandVector3();

        StartCoroutine(PlayFoodAnimBackwards());
    }

    // This method is to return a random Vector3.
    Vector3 GetRandVector3()
    {
        int rand = Random.Range(0, 6);

        switch (rand)
        {
            case 0:
                return Vector3.back;
            case 1:
                return Vector3.down;
            case 2:
                return Vector3.forward;
            case 3:
                return Vector3.left;
            case 4:
                return Vector3.right;
            default:
                return Vector3.up;
        }
    }

    // Play the foor animation.
    IEnumerator PlayFoodAnimBackwards()
    {
        foodAnimPlaying = true;
        animator.SetTrigger("isSpawn");
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(1f);

        animator.ResetTrigger("isSpawn");
        foodAnimPlaying = false;
    }

    #endregion
}

