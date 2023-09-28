using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // ////////////////////////////////////////
    // //////////////// FIELDS ////////////////
    // ////////////////////////////////////////
    #region FIELDS

    public Transform cameraTransform;   // Reference to the main camera's transform
    public float shakeDuration = 0.5f;  // Duration of the camera shake
    public float shakeAmount = 0.2f;    // Intensity of the shake
    private Vector3 originalPosition;   // Store the original camera position
    private float currentShakeDuration; // Current duration of the shake

    #endregion


    // ////////////////////////////////////////
    // //////////// MONO-BEHAVIORS ////////////
    // ////////////////////////////////////////
    #region MONOBEHAVIORS

    private void Awake()
    {
        if (cameraTransform == null)
        {
            // If the cameraTransform is not assigned, use the main camera's transform
            cameraTransform = Camera.main.transform;
        }
    }

    private void Start()
    {
        originalPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        // Check if the camera shake duration is greater than 0
        if (currentShakeDuration > 0)
        {
            // Randomly offset the camera's position within the specified shakeAmount
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;

            // Decrease the shake duration over time
            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            // Reset the camera's position to its original position
            cameraTransform.localPosition = originalPosition;
        }
    }

    #endregion


    // ////////////////////////////////////////
    // /////////////// METHODS ////////////////
    // ////////////////////////////////////////
    #region METHODS

    // Call this method to start the camera shake
    public void ShakeCamera()
    {
        currentShakeDuration = shakeDuration;
    }

    #endregion
}