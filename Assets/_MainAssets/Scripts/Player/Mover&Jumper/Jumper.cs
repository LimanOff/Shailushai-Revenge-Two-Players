using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(Rigidbody2D))]
public class Jumper : MonoBehaviour
{
    public event Action PlayerStartedJump;
    public event Action PlayerEnteredAir;
    public event Action PlayerGrounded;
    public event Action PlayerJumped;

    [SerializeField] private LayerMask JumpableObjects;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;

    public float JumpHeight;

    private float _lastGroundedTime;
    private bool _canJump;
    private float _coyoteTime = 0.1f;

    private PlayerInputController _inputController;

    [Inject]
    private void Construct(PlayerInputController inputController)
    {
        _inputController = inputController;
    }

    private void Awake()
    {
        InitializeComponents();

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnSubscribeEvents();
    }

    private void Jump()
    {
        PlayerStartedJump?.Invoke();

        float jumpForce = (Mathf.Sqrt(JumpHeight * (Physics2D.gravity.y * _rigidbody2D.gravityScale) * -2)) * _rigidbody2D.mass;
        _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        PlayerJumped?.Invoke();
    }

    public void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (_canJump)
            Jump();
    }

    private bool IsOnSurface()
    {
        float colliderBottom = _boxCollider2D.bounds.min.y;
        float offset = 0.1f;
        Vector2 rayOrigin = new Vector2(transform.position.x, colliderBottom - offset);


        return _canJump = Physics2D.Raycast(rayOrigin, Vector2.down, 0.7f, JumpableObjects);
    }

    private void Update()
    {
        if (IsOnSurface())
        {
            _lastGroundedTime = Time.time;
            PlayerGrounded?.Invoke();
        }
        else
        {
            if (Time.time - _lastGroundedTime <= _coyoteTime)
            {
                _canJump = true;
                PlayerEnteredAir?.Invoke();
            }
            else
            {
                _canJump = false;
            }
        }
    }

    private void InitializeComponents()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void SubscribeEvents()
    {
        if (gameObject.name == "Player 1")
        {
            _inputController.GameInput.Player1.Jump.performed += HandleJumpInput;
        }
        else
        {
            _inputController.GameInput.Player2.Jump2.performed += HandleJumpInput;
        }
    }
    private void UnSubscribeEvents()
    {
        if (gameObject.name == "Player 1")
        {
            _inputController.GameInput.Player1.Jump.performed -= HandleJumpInput;
        }
        else
        {
            _inputController.GameInput.Player2.Jump2.performed -= HandleJumpInput;
        }
    }
}
