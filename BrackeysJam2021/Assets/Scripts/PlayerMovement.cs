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
    [SerializeField]
    [Tooltip("Max speed of together player")]
    float maxTogetherSpeed = 5.0f;
    [SerializeField]
    [Tooltip("Max speed of alone player")]
    float maxAloneSpeed = 10.0f;
    [SerializeField]
    [Tooltip("Max jump velocity of together player")]
    float maxTogetherJumpVelocity = 5.0f;
    [SerializeField]
    [Tooltip("Max jump of alone player")]
    float maxAloneJumpVelocity = 10.0f;

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
                OnSpeedChanged?.Invoke();
            }
        }
    }

    private float _speedDirection = 0.0f;
    public float _speed = 0.0f;
    #endregion

    #region Interactions
    [SerializeField]
    [Tooltip("Range of interactions with nearby objects")]
    float interactionRange = 2.0f;
    #endregion

    #region Rigidbody properties
    [SerializeField]
    [Tooltip("Weight to the rigidbody of together")]
    float weightTogether = 2.0f;
    [SerializeField]
    [Tooltip("Weight to the rigidbody of alone")]
    float weightAlone = 2.0f;
    #endregion

    private Rigidbody2D _rigidbody2D;
    EPlayerState PlayerState { get; set; }
    public event System.Action OnSpeedChanged;

    #region Unity Events
    private void OnDestroy()
    {
        OnSpeedChanged -= ClampPlayerSpeed;
    }

    private void Start()
    {
        OnSpeedChanged += ClampPlayerSpeed;
        PlayerState = EPlayerState.TOGETHER;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Speed += _speedDirection * Time.deltaTime;

        transform.Translate(Vector3.right * Speed);
    }
    #endregion

    private void ClampPlayerSpeed()
    {
        var velocity = _rigidbody2D.velocity;
        switch(PlayerState)
        {
            case EPlayerState.TOGETHER:
                _speed = Mathf.Clamp(_speed, -maxTogetherJumpVelocity, maxTogetherJumpVelocity);
                velocity.x = _speed;
                _rigidbody2D.velocity = velocity;
                break;
            case EPlayerState.ALONE:
                _speed = Mathf.Clamp(_speed, -maxAloneSpeed, maxAloneSpeed);
                velocity.x = _speed;
                _rigidbody2D.velocity = velocity;
                break;
            default: break;
        }
    }

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
        var jumping = context.ReadValueAsButton();
        if (context.performed)
        {
            Debug.Log("Jump state:" + jumping);
        }
        else if (context.canceled)
        {
            Debug.Log("Jump state:" + jumping);
        }
    }
    #endregion
}
