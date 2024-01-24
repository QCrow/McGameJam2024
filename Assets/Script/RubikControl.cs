using System.Collections;
using UnityEngine;

public class RubikControl : MonoBehaviour
{
  public static RubikControl Instance { get; private set; }
  private Vector3 previousMousePosition;
  public Quaternion cubeRotation;
  private bool isLerping = false;
  public bool isCubeRotating = false;
  private float lerpSpeed = 5f; // Speed of lerp rotation
  private Camera mainCamera; // Reference to the main camera


  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      mainCamera = Camera.main; // Initialize the camera reference
    }
    else
    {
      Destroy(gameObject);
    }
  }

  IEnumerator Start()
  {
    while (true)
    {
      yield return null;

      if (Input.GetMouseButton(1) && !FaceControl.Instance.isDragging)
      {
        isCubeRotating = true;
        RotateCube();
      }
      else if (Input.GetMouseButtonUp(1))
      {
        StartLerpingToSnappedRotation();
      }
      else if (isLerping)
      {
        LerpToTargetRotation();
      }

      previousMousePosition = Input.mousePosition;
    }
  }

  private void RotateCube()
  {
    Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
    mouseDelta *= 1.2f; // reduction of rotation speed

    // Convert mouse movements into a rotation relative to the camera's view
    Quaternion cameraRotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);

    // Adjust the direction of rotation here
    Vector3 rotationDir = cameraRotation * new Vector3(mouseDelta.y, -mouseDelta.x, 0);
    transform.Rotate(rotationDir, Space.World);
  }

  private void StartLerpingToSnappedRotation()
  {
    Debug.Log("Starting to Lerp!");
    // Calculate target rotation
    Vector3 snappedRotation = transform.rotation.eulerAngles;
    snappedRotation.x = Mathf.Round(snappedRotation.x / 90) * 90;
    snappedRotation.y = Mathf.Round(snappedRotation.y / 90) * 90;
    snappedRotation.z = Mathf.Round(snappedRotation.z / 90) * 90;
    cubeRotation = Quaternion.Euler(snappedRotation);

    isLerping = true;
  }

  private void LerpToTargetRotation()
  {
    Debug.Log("Lerp!");
    transform.rotation = Quaternion.Lerp(transform.rotation, cubeRotation, Time.deltaTime * lerpSpeed);

    if (Quaternion.Angle(transform.rotation, cubeRotation) < 0.1f)
    {
      transform.rotation = cubeRotation;
      isLerping = false;
      isCubeRotating = false;
    }
  }
}
