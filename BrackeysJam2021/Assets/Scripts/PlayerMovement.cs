using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerMovement : MonoBehaviour
{
    #region Player speeds
    [Header("Player properties", order = 0)]
    [SerializeField]
    [Tooltip("Max speed of together player")]
    float togetherMaxSpeed = 5.0f;
    [SerializeField]
    [Tooltip("Max speed of alone player")]
    float aloneMaxSpeed = 10.0f;
    [SerializeField]
    [Tooltip("Max jump velocity of together player")]
    float togetherMaxJumpHeight = 5.0f;
    [SerializeField]
    [Tooltip("Max jump of alone player")]
    float aloneMaxJumpHeight = 10.0f;

    private float speedDirection = 0.0f;
    private float currentSpeed = 0.0f;
    #endregion

    #region Interactions
    [Header("Interactions properties", order = 4)]
    [SerializeField]
    [Tooltip("Range of interactions with nearby objects")]
    float interactionRange = 2.0f;
    #endregion

    #region Rigidbody properties
    [Header("Rigibody properties", order = 2)]
    [SerializeField]
    [Tooltip("Weight to the rigidbody of together")]
    float togetherMass = 3.0f;
    [SerializeField]
    [Tooltip("Weight to the rigidbody of alone")]
    float aloneMass = 2.0f;
    [SerializeField]
    [Tooltip("Force to apply at jump")]
    private float liftForce = 150.0f;
    #endregion

    private EPlayerState _playerState;
    public EPlayerState PlayerState
    {
        get { return _playerState;  }
        set
        {
            if (_playerState != value)
            {
                _playerState = value;
                OnPlayerStateChange?.Invoke();
            }
        }
    }

    public float speed; // TODO: to change in local

    public delegate void OnPlayerStateChangeDelegate();
    public event OnPlayerStateChangeDelegate OnPlayerStateChange;

    #region PlayerMovement component references
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    #endregion

    #region Unity Events
    private void OnDestroy()
    {
        OnPlayerStateChange -= UpdatePlayerState;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        OnPlayerStateChange += UpdatePlayerState;
    }

    private void Start()
    {
        PlayerState = EPlayerState.ALONE;
    }

    private void Update()
    {
        // TODO: Maybe handle some states in here ?
    }

    private void FixedUpdate()
    {
        MoveAction();
        JumpAction();
    }
    #endregion

    private void MoveAction()
    {
         float currentAirControl = 1.0f;
        if (isJumping)
        {
            currentAirControl = 1.0f - airControlReducer;
        }
        speed = (currentSpeed * currentAirControl) * speedDirection * Time.fixedDeltaTime;
        rb.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
    }

    #region PlayerActions Events
    public void Move(InputAction.CallbackContext context)
    {
        speedDirection = context.ReadValue<float>();
    }

    public void Use(InputAction.CallbackContext context)
    {
        var usePressed = context.ReadValueAsButton();
        if (context.performed)
        {
            Debug.Log("Use interaction.");
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetJumpState(context.ReadValueAsButton());
        }
    }

    public void Split(InputAction.CallbackContext context)
    {
        // TODO: Add a state to enable/disable an event interaction
        // Here we want to disable Hold interaction in case we set playerState to ALONE
        // In such a manner we can now simple press the button to go back to TOGETHER state instead of holding
        // this state should be handled here in context.performed
        if (context.performed)
        {
            SwitchPlayerState();
        }
    }
    #endregion

    #region States setters
    public void SwitchPlayerState()
    {
        if (PlayerState == EPlayerState.TOGETHER)
        {
            PlayerState = EPlayerState.ALONE;
        }
        else
        {
            PlayerState = EPlayerState.TOGETHER;
        }
    }

    private void UpdatePlayerState()
    {
        switch(_playerState)
        {
            case EPlayerState.TOGETHER:
                currentSpeed = togetherMaxSpeed;
                rb.mass = togetherMass;
                floatHeight = togetherMaxJumpHeight;
                break;
            case EPlayerState.ALONE:
                currentSpeed = aloneMaxSpeed;
                rb.mass = aloneMass;
                floatHeight = aloneMaxJumpHeight;
                break;
            default: break;
        }
    }
    #endregion
}

public enum EPlayerState : byte
{
    TOGETHER = 0,
    ALONE
};