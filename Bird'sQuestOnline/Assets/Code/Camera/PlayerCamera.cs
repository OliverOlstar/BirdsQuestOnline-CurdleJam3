using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    private Transform _CameraTansform;
    private Transform _ParentTransform;
    private Vector3 _LocalRotation;
    private Vector3 _TargetLocalPosition;

    [SerializeField] private bool Inverted = false;

    [Header("Idle")]
    [SerializeField] private float idleSpinSpeed = 1;
    [SerializeField] private float idleSpinY = 40;
    [SerializeField] private float idleDelay = 120;
    private float idleTime = 60;

    [Header("Camera Collision")]
    [SerializeField] private LayerMask cameraCollisionLayers;
    [SerializeField] private float cameraCollisionDampening = 20;
    [SerializeField] [Range(0, 1)] private float cameraCollisionMinDisPercent = 0.1f;

    [Space]
    public float MouseSensitivity = 4f;
    public float TurnDampening = 10f;
    [SerializeField] private float OffSetLeft = 0f;
    [SerializeField] private float CameraDistance = 6f;
    [SerializeField] private float CameraMinHeight = -20f;
    [SerializeField] private float CameraMaxHeight = 90f;

    public bool CameraDisabled = false;

    void Start()
    {
        //Getting Transforms
        _CameraTansform = transform;
        _ParentTransform = transform.parent;

        //Maintaining Starting Rotation
        _LocalRotation.x = _ParentTransform.eulerAngles.y;
        _LocalRotation.y = _ParentTransform.eulerAngles.x;

        //Locking cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Setting camera distance
        _TargetLocalPosition = new Vector3(-OffSetLeft, 0f, CameraDistance * -1f);
        _CameraTansform.localPosition = _TargetLocalPosition;
    }

    void Update()
    {
        if (!CameraDisabled && idleTime < Time.time)
            IdleCameraMovement();

        //Actual Camera Transformations
        Quaternion TargetQ = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        _ParentTransform.rotation = Quaternion.Lerp(_ParentTransform.rotation, TargetQ, Time.deltaTime * TurnDampening);

        //Camera Collision
        CameraCollision();
    }

    void DefaultCameraMovement(Vector2 pInput)
    {
        //Rotation of the camera based on mouse movement
        if (pInput != Vector2.zero)
        {
            _LocalRotation.x += pInput.x * MouseSensitivity;
            _LocalRotation.y -= pInput.y * MouseSensitivity * (Inverted ? -1 : 1);

            //Clamping the y rotation to horizon and not flipping over at the top
            if (_LocalRotation.y < CameraMinHeight)
            {
                _LocalRotation.y = CameraMinHeight;
            }
            else if (_LocalRotation.y > CameraMaxHeight)
            {
                _LocalRotation.y = CameraMaxHeight;
            }
        }
    }

    void IdleCameraMovement()
    {
        //Slowly Rotate
        _LocalRotation.x += idleSpinSpeed * Time.deltaTime;
        _LocalRotation.y = Mathf.Lerp(_LocalRotation.y, idleSpinY, Time.deltaTime);
    }

    // Camera Collision //////////////
    void CameraCollision()
    {
        RaycastHit hit;
        Physics.Raycast(_ParentTransform.position, (_CameraTansform.position - _ParentTransform.position).normalized, out hit, CameraDistance, cameraCollisionLayers);

        if (hit.point != Vector3.zero)
        {
            hit.point -= _ParentTransform.position;
            _CameraTansform.localPosition = Vector3.Lerp(_CameraTansform.localPosition, _TargetLocalPosition * Mathf.Clamp((hit.point.magnitude / _TargetLocalPosition.magnitude), cameraCollisionMinDisPercent, 0.5f), Time.deltaTime * cameraCollisionDampening);
        }
        else
        {
            _CameraTansform.localPosition = Vector3.Lerp(_CameraTansform.localPosition, _TargetLocalPosition * 0.5f, Time.deltaTime * cameraCollisionDampening);
        }
    }

    // Inputs ///////////////////////
    public void OnCamera(InputAction.CallbackContext ctx)
    {
        if (!CameraDisabled)
            DefaultCameraMovement(ctx.ReadValue<Vector2>());
        idleTime = Time.time + idleDelay;
    }
}
