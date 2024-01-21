using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class FaceControl : MonoBehaviour
{
    public static FaceControl Instance { get; private set; }
    public float minimumDragPixels = 3.0f;
    public float dragSensitivity = 3.0f;
    public float snapSpeed = 90.0f;
    public bool isDragging = false;
    Camera _camera;
    public LayerMask layerMask;
    ReadCube readCube;

    Collider[] _subCubes = new Collider[9];
    Vector3[] _originalPositions = new Vector3[9];
    Vector3[] _localPositions = new Vector3[9];
    Quaternion[] _originalOrientations = new Quaternion[9];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        _camera = Camera.main;
        Vector3 camForward = _camera.transform.forward;
        float axisSign = Mathf.Sign(camForward.x * camForward.y * camForward.z);
        // Define the layer to ignore (replace "LayerToIgnore" with your layer's name)
        int layerToIgnore = LayerMask.NameToLayer("Square");

        // Create a layer mask that ignores the specified layer
        layerMask = ~(1 << layerToIgnore);

        while (true)
        { 
            yield return null;

            // Keep waiting until the player presses the mouse button.
            if (!Input.GetMouseButton(0) || RubikControl.Instance.isCubeRotating)
                continue;

                // Step 1: Fire a ray through the mouse position.
                Vector2 clickPosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(clickPosition);

            // If we didn't hit anything, try again next frame.
            if (!Physics.Raycast(ray, out RaycastHit hit))
                continue;
            isDragging = true;
            // Step 2: find the two potential axes of rotation.
            int normalAxis = Mathf.Abs(Mathf.RoundToInt(Vector3.Dot(
                    hit.normal,
                    new Vector3(0, 1, 2)
                )));

            Vector3 rotationAxis = Vector3.zero;
            Vector3 alternativeAxis = Vector3.zero;

            rotationAxis[(normalAxis + 1) % 3] = 1;
            alternativeAxis[(normalAxis + 2) % 3] = 1;

            // This sign fiddling just ensures our drag direction matches the visible rotation.
            float signFlip = axisSign * Mathf.Sign(Vector3.Dot(rotationAxis, camForward) * Mathf.Sign(Vector3.Dot(alternativeAxis, camForward)));
            Vector2 rotationDirection = signFlip * ScreenDirection(clickPosition, hit.point, alternativeAxis);
            Vector2 alternativeDirection = -signFlip * ScreenDirection(clickPosition, hit.point, rotationAxis);

            // Step 3: wait until we've dragged far enough to pick an axis.
            float signedDistance;
            do
            {
                yield return null;

                Vector2 mousePosition = Input.mousePosition;
                signedDistance = DistanceAlong(clickPosition, mousePosition, rotationDirection);
                if (Mathf.Abs(signedDistance) > minimumDragPixels)
                    break;

                signedDistance = DistanceAlong(clickPosition, mousePosition, alternativeDirection);
                if (Mathf.Abs(signedDistance) > minimumDragPixels)
                {
                    rotationAxis = alternativeAxis;
                    rotationDirection = alternativeDirection;
                    break;
                }

            } while (Input.GetMouseButton(0));


            // Step 4: Gather the 8-9 sub-cubes we need to rotate.
            Vector3 extents = Vector3.one - 0.9f * rotationAxis;
            extents = extents * 2.0f;
            int subCubeCount = Physics.OverlapBoxNonAlloc(hit.collider.transform.position, extents, _subCubes, Quaternion.identity, layerMask);
            if (subCubeCount == 8)
            {
                continue;
            }
            for (int i = 0; i < subCubeCount; i++)
            {
                var subCube = _subCubes[i].transform;

                _originalPositions[i] = subCube.position;
                Vector3 originalLocalPosition = subCube.localPosition; // Your original Vector3
                Vector3 roundedLocalPosition;

                roundedLocalPosition.x = Mathf.Round(originalLocalPosition.x);
                roundedLocalPosition.y = Mathf.Round(originalLocalPosition.y);
                roundedLocalPosition.z = Mathf.Round(originalLocalPosition.z);

                _localPositions[i] = roundedLocalPosition;

                _originalOrientations[i] = subCube.rotation;
            }

            // Step 5-6: Rotate the group by the input angle each frame.
            float angle = 0.0f;
            while (Input.GetMouseButton(0))
            {
                angle = signedDistance * dragSensitivity;

                RotateGroup(angle, rotationAxis, subCubeCount);

                yield return null;
                Vector2 mousePosition = Input.mousePosition;
                signedDistance = DistanceAlong(clickPosition, mousePosition, rotationDirection);
            }

            // Step 7: After release, snap the angle to a multiple of 90.
            float snappedAngle = Mathf.Round(angle / 90.0f) * 90.0f;

            if (snappedAngle < -90.0f)
            {
                snappedAngle = -90.0f;
            }

            if (snappedAngle > 90.0f)
            {
                snappedAngle = 90.0f;
            }

            bool allXOne = true;
            bool allXMinusOne = true;
            bool allYOne = true;
            bool allYMinusOne = true;
            bool allZOne = true;
            bool allZMinusOne = true;

            foreach (Vector3 position in _localPositions)
            {
                if (position.x != 1)
                {
                    allXOne = false;
                }
                if (position.x != -1)
                {
                    allXMinusOne = false;
                }
                if (position.y != 1)
                {
                    allYOne = false;
                }
                if (position.y != -1)
                {
                    allYMinusOne = false;
                }
                if (position.z != 1)
                {
                    allZOne = false;
                }
                if (position.z != -1)
                {
                    allZMinusOne = false;
                }
            }

            while (angle != snappedAngle)
            {
                angle = Mathf.MoveTowards(angle, snappedAngle, snapSpeed * Time.deltaTime);

                RotateGroup(angle, rotationAxis, subCubeCount);
                yield return null;
            }
            isDragging = false;
            readCube.ReadState();
        }
    }

    Vector2 ScreenDirection(Vector2 screenPoint, Vector3 worldPoint, Vector3 worldDirection)
    {
        Vector2 shifted = _camera.WorldToScreenPoint(worldPoint + worldDirection);

        return (shifted - screenPoint).normalized;
    }

    float DistanceAlong(Vector2 clickPosition, Vector2 currentPosition, Vector2 direction)
    {
        return Vector2.Dot(currentPosition - clickPosition, direction);
    }

    void RotateGroup(float angle, Vector3 axis, int count)
    {

        Quaternion rotation = Quaternion.AngleAxis(angle, axis);

        for (int i = 0; i < count; i++)
        {
            var subCube = _subCubes[i].transform;
            subCube.position = rotation * _originalPositions[i];
            subCube.rotation = rotation * _originalOrientations[i];
        }
    }


}