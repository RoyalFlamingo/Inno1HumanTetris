using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputActionMap gameplayActions;
    private PlayerInput playerInput;
    private Rigidbody playerRb;
    private Vector3 moveDirection;
    private Vector3 lookDirection;
    private bool grounded = true;
    private bool jumping = false;
    private float jumpForce;
    private float maxJumpHeight = 4.0f;
    private float counterJumpForce = 10.0f;
    private float maxVelocity = 10.0f;
    private float speed = 40.0f;
    private bool inputActive = false;
    private Animator _animator;
    private float capsuleDistance;
    private float capsuleRadius;
    private Vector3 capsuleCenter;
    private ObstacleSpawner obstacleSpawner;

    public Vector3 SpawnPosition { get; set; }

    private void Awake()
    {
        lookDirection = Vector3.right;
        playerInput = gameObject.GetComponent<PlayerInput>();
        gameplayActions = playerInput.currentActionMap;
        gameplayActions.Enable();
    }
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        jumpForce = Mathf.Sqrt(2 * Physics.gravity.magnitude * maxJumpHeight);
        SpawnPosition = transform.position;
        _animator = GetComponentInChildren<Animator>();
        capsuleRadius = GetComponentInChildren<CapsuleCollider>().radius * transform.localScale.x;
        capsuleDistance = GetComponentInChildren<CapsuleCollider>().height*transform.localScale.x-2*capsuleRadius;
        capsuleCenter = GetComponentInChildren<CapsuleCollider>().center*transform.localScale.x;
        obstacleSpawner = GameObject.Find("ObstacleSpawner").GetComponent<ObstacleSpawner>();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position+capsuleCenter + Vector3.up * -(capsuleDistance * 0.5f);
        Vector3 p2 = p1 + Vector3.up * (capsuleDistance);
        Debug.DrawLine(p2, p2 + Vector3.right*obstacleSpawner.obstacleSpeed * Time.fixedDeltaTime, Color.green,2,false);
        Debug.DrawLine(p2, p2 + Vector3.back * capsuleRadius, Color.red, 2, false);
        Debug.DrawLine(p2, p2 - Vector3.back * capsuleRadius, Color.red, 2, false);
        Debug.DrawLine(p2, p2 + Vector3.up * capsuleRadius, Color.red, 2, false);
        Debug.DrawLine(p1, p1 - Vector3.up * capsuleRadius, Color.red, 2, false);
        if (Physics.CapsuleCast(p1, p2, capsuleRadius, Vector3.right,out hit, 1.2f*obstacleSpawner.obstacleSpeed*Time.fixedDeltaTime))
        {
            if (!hit.collider.CompareTag("Player")&&!hit.collider.CompareTag("Ground"))
            {
                Debug.Log("Hit");
                playerRb.velocity = new Vector3(-obstacleSpawner.obstacleSpeed,playerRb.velocity.y,playerRb.velocity.z);
            }
        }

        if (!grounded)
        {
            if (!jumping)
            {
                playerRb.AddForce(counterJumpForce * playerRb.mass * Vector3.down);
            }

            if (Mathf.Abs(playerRb.velocity.x) < maxVelocity)
            {
                playerRb.AddForce(speed / 4 * new Vector3(moveDirection.x, 0, 0));
            }
            if (Mathf.Abs(playerRb.velocity.z) < maxVelocity)
            {
                playerRb.AddForce(speed / 4 * new Vector3(0, 0, moveDirection.z));
            }
        }
        else
        {

            if (Mathf.Abs(playerRb.velocity.x) < maxVelocity)
            {
                playerRb.AddForce(speed * new Vector3(moveDirection.x, 0, 0));
            }
            if (Mathf.Abs(playerRb.velocity.z) < maxVelocity)
            {
                playerRb.AddForce(speed * new Vector3(0, 0, moveDirection.z));
            }
        }

        if (playerRb.velocity.magnitude > 0.5 && inputActive)
        {
            lookDirection.x = playerRb.velocity.x;
            lookDirection.z = playerRb.velocity.z;
        }

        playerRb.MoveRotation(Quaternion.FromToRotation(Vector3.right, lookDirection));

        if (_animator == null)
            return;

        _animator.SetFloat("Speed_f", playerRb.velocity.magnitude/10);
        //Debug.Log($"Player Speed: {playerRb.velocity.magnitude/10}");
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (grounded && !jumping && context.performed == true)
        {
            jumping = true;
            grounded = false;
            playerRb.AddForce(jumpForce * playerRb.mass * Vector3.up, ForceMode.Impulse);
        }
        else if (!grounded && context.canceled == true)
        {
            jumping = false;
        }

		if (_animator == null)
			return;

        _animator.SetBool("Jump_b", true);

	}

    private void Movement(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            moveDirection = new Vector3(direction.y, 0, -direction.x);
        }
        else if (context.canceled == true)
        {
            moveDirection = Vector3.zero;
        }
    }

    public void ActivateInput()
    {
        InputAction action;
        action = gameplayActions.FindAction("Jump");
        action.performed += Jump;
        action.canceled += Jump;
        action = gameplayActions.FindAction("Move");
        action.performed += Movement;
        action.canceled += Movement;
        inputActive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumping = false;
            grounded = true;
        }
    }

    private void OnDestroy()
    {
        InputAction action;
        action = gameplayActions.FindAction("Jump");
        action.performed -= Jump;
        action.canceled -= Jump;
        action = gameplayActions.FindAction("Move");
        action.performed -= Movement;
        action.canceled -= Movement;
    }


}
