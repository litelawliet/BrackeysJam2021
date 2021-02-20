using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
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
    private float previousSpeedDirection = 1.0f;
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

    public enum EPlayerState : byte
    {
        TOGETHER = 0,
        ALONE,
    };

    private EPlayerState _playerState = EPlayerState.TOGETHER;
    public EPlayerState PlayerState
    {
        get { return _playerState; }
        set
        {
            _playerState = value;
            OnPlayerStateChange?.Invoke();
        }
    }

    #region Delegates
    public delegate void OnPlayerStateChangeDelegate();
    public static event OnPlayerStateChangeDelegate OnPlayerStateChange;
    public delegate void OnUseInteractibleDelegate();
    public static event OnUseInteractibleDelegate OnUseInteractible;
    #endregion

    public GameObject aloneStayGO;
    public IInteractible interactionTargetScript;
    public GameObject interactionTarget;
    [SerializeField]
    [Tooltip("Objects in range. Updated at each physic tick.")]
    private List<GameObject> objectsInRange;

    #region PlayerMovement component references
    private Rigidbody2D rb;
    private BoxCollider2D playerTopCollider;
    private CapsuleCollider2D playerGroundCollider;
    private SpriteRenderer spriteRenderer;
    private Animator playerAnimator;
    #endregion
     
    #region Unity Events
    private void OnDestroy()
    {
        OnPlayerStateChange -= UpdatePlayerState;
        OnUseInteractible -= UseInteractibleTarget;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerGroundCollider = GetComponent<CapsuleCollider2D>();
        playerTopCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        OnPlayerStateChange += UpdatePlayerState;
        OnUseInteractible += UseInteractibleTarget;
        PlayerState = EPlayerState.TOGETHER;
        playerAnimator.SetBool("IsTogether", true);
    }

    private void Start()
    {
        var outline = gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = true;
    }

    private void Update()
    {
        // TODO: Maybe handle some states in here ?
    }

    private void FixedUpdate()
    {
        MoveAction();
        JumpAction();
        PopulateInteractiblesInRange();
    }

    private void LateUpdate()
    {
        if (!aloneStayGO.activeSelf)
        {
            aloneStayGO.transform.position = transform.position;
        }
    }
    #endregion

    private void MoveAction()
    {
        float currentAirControl = 1.0f;
        if (isJumping)
        {
            currentAirControl = 1.0f - airControlReducer;
        }
        float speed = (currentSpeed * currentAirControl) * speedDirection * Time.fixedDeltaTime;
        rb.AddForce(Vector2.right * speed, ForceMode2D.Impulse);

        if (Mathf.Approximately(Mathf.Abs(speedDirection), 0.0f))
        {
            playerAnimator.SetBool("IsWalking", false);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", true);
        }
    }

    #region States setters
    public void SwitchPlayerState()
    {
        if (PlayerState == EPlayerState.TOGETHER)
        {
            PlayerState = EPlayerState.ALONE;
            playerAnimator.SetBool("IsTogether", false);
        }
        else
        {
            PlayerState = EPlayerState.TOGETHER;
            playerAnimator.SetBool("IsTogether", true);
        }
    }

    private void UpdatePlayerState()
    {
        switch (_playerState)
        {
            case EPlayerState.TOGETHER:
                currentSpeed = togetherMaxSpeed;
                rb.mass = togetherMass;
                floatHeight = togetherMaxJumpHeight;
                //spriteRenderer.sprite = togetherSprite;
                break;
            case EPlayerState.ALONE:
                currentSpeed = aloneMaxSpeed;
                rb.mass = aloneMass;
                floatHeight = aloneMaxJumpHeight;
                //spriteRenderer.sprite = alonePlayerSprite;
                break;
            default: break;
        }
    }
    #endregion
}