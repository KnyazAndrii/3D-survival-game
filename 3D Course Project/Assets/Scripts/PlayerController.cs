using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public bool CanMove = true;

    [Header("Moving")]

    private float _waklingSpeed = 4;
    private float _runningSpeed = 7;

    private float _jumpForce = 3;
    private float _gravity = 5;

    [Header("Rotation")]
    public Camera MainCamera;
    private float _lookSpeed = 5;
    private float _lookXLimit = 80;

    private CharacterController characterController;
    private Vector3 _moveDirection = Vector3.zero;
    private float rotationX;
    private Animator _animator;
    private GameObject _loseScreenInstance;

    public Vector3 Forward;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = gameObject.GetComponentInChildren<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MovePlayer();
        RotateCamera();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentSpeedZ = 0;
        float currentSpeedX = 0;

        if (CanMove)
        {
            if (isRunning)
            {
                currentSpeedZ = _runningSpeed;
                currentSpeedX = _runningSpeed;
            }
            else
            {
                currentSpeedZ = _waklingSpeed;
                currentSpeedX = _waklingSpeed;
            }

            currentSpeedZ *= Input.GetAxis("Vertical");
            currentSpeedX *= Input.GetAxis("Horizontal");
        }
        else
        {
            currentSpeedZ = 0;
            currentSpeedX = 0;
        }

        float movementDirectionY = _moveDirection.y;

        _moveDirection = (Forward * currentSpeedZ) + (right * currentSpeedX);

        if (Input.GetKey(KeyCode.Space) && CanMove && characterController.isGrounded)
        {
            _moveDirection.y += _jumpForce;
        }
        else
        {
            _moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        characterController.Move(_moveDirection * Time.deltaTime);
    }

    private void RotateCamera()
    {
        if (CanMove)
        {
            rotationX += Input.GetAxis("Mouse Y") * _lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -_lookXLimit, _lookXLimit);
            MainCamera.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);
        }
    }

    private void RotatePlayer()
    {
        if(CanMove)
        {
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
        }
    }

    public void Death()
    {
        Time.timeScale = 0;
        _loseScreenInstance = Instantiate(Resources.Load<GameObject>("CanvasLose"));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CanMove = false;
    }
}