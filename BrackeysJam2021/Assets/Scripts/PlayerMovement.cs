using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EPlayerState : ushort
{
    TOGETHER = 0,
    ALONE
};

public class PlayerMovement : MonoBehaviour
{
    #region Player speeds
    [Header("Player properties")]
    [SerializeField]
    [Tooltip("Max speed of together player")]
    float maxTogetherSpeed = 5.0f;
    [SerializeField]
    [Tooltip("Max speed of alone player")]
    float maxAloneSpeed = 10.0f;
    [SerializeField]
    [Tooltip("Max jump velocity of together player")]
    float maxTogetherJumpHeight = 5.0f;
    [SerializeField]
    [Tooltip("Max jump of alone player")]
    float maxAloneJumpHeight = 10.0f;

    /// <summary>
    /// Speed property. This property auto clamp its speed velocity accordingly to PlayerState enum.
    /// </summary>
    public float Speed
    {
        get { return _speed; }
        set
        {
            if (_speed != value)
            {
                _speed = value;
            }
        }
    }

    private float _speedDirection = 0.0f;
    public float _speed = 0.0f;
    #endregion

    #region Interactions
    [Header("Interactions properties")]
    [SerializeField]
    [Tooltip("Range of interactions with nearby objects")]
    float interactionRange = 2.0f;
    #endregion

    #region Rigidbody properties
    [Header("Rigibody properties")]
    [SerializeField]
    [Tooltip("Weight to the rigidbody of together")]
    float weightTogether = 5.0f;
    [SerializeField]
    [Tooltip("Weight to the rigidbody of alone")]
    float weightAlone = 2.0f;
    [SerializeField]
    [Tooltip("Force to apply at jump")]
    private float liftForce = 150.0f;
    #endregion

    public EPlayerState PlayerState { get; set; }
    
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider;

    private bool _isJumping = false;
    private bool _jumpCalled = false;
    private float damping = 0.5f;

    #region Unity Events
    private void Start()
    {
        PlayerState = EPlayerState.TOGETHER;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float currentSpeed = PlayerState == EPlayerState.TOGETHER ? maxTogetherSpeed : maxAloneSpeed;
        transform.Translate(Vector3.right * _speedDirection * currentSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (_jumpCalled && !_isJumping)
        {
            int layerMask = ~(LayerMask.GetMask("Player"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, layerMask);

            if (hit.collider != null)
            {
                // Calculate the distance from the surface and the "error" relative
                // to the floating height.
                float distance = Mathf.Abs(hit.point.y - transform.position.y);

                float floatHeight = PlayerState == EPlayerState.TOGETHER ? maxTogetherJumpHeight : maxAloneJumpHeight;
                float heightError = floatHeight - distance;

                // The force is proportional to the height error, but we remove a part of it
                // according to the object's speed.
                float force = liftForce * heightError - _rigidbody2D.velocity.y * damping;

                _rigidbody2D.AddForce(Vector3.up * force);
                
                _jumpCalled = false;
                _isJumping = true;
            }
        }
        else
        {
            int layerMask = ~(LayerMask.GetMask("Player"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, _boxCollider.bounds.size.y, layerMask);
            Debug.DrawLine(transform.position, transform.position - transform.up * (_boxCollider.bounds.size.y / 2.0f), Color.red);

            if (hit.collider != null)
            {
                _isJumping = false;
            }
        }
    }
    #endregion

    #region PlayerActions Events
    public void Move(InputAction.CallbackContext context)
    {
        _speedDirection = context.ReadValue<float>();
    }

    public void Use(InputAction.CallbackContext context)
    {
        var usePressed = context.ReadValueAsButton();
        if (context.performed)
        {
            Debug.Log("Use state:" + usePressed);
        }
        else if(context.canceled)
        {
            Debug.Log("Use state:" + usePressed);
        }
    }

    public void Split(InputAction.CallbackContext context)
    {
        var splitComplete = context.ReadValueAsButton();
        if (context.performed)
        {
            Debug.Log("Splitting state:" + splitComplete);
        }
        else if (context.canceled)
        {
            Debug.Log("Splitting state:" + splitComplete);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetJumpState(context.ReadValueAsButton());
        }
    }
    #endregion

    void SetJumpState(bool jumpingState)
    {
        _jumpCalled = jumpingState;
    }
}
