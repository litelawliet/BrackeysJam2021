using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Hands : MonoBehaviour
{
    #region Components references
    BoxCollider2D _boxCollider;
    Rigidbody2D _rigidbody;
    SpriteRenderer _spriteRenderer;
    public GameObject aloneGolem = null;
    public PlayerMovement playerMovementScript = null;
    Transform aloneGolemTransform;
    Camera _cameraRef;
    #endregion

    private bool beginChase = false;
    private Vector3 initialPosition = Vector3.zero;
    private Vector2 direction;
    private float currentSpeed = 0.0f;
    private LevelLoader levelLoader;
    private GameObject soundManager;

    public float handSoundTimer = 5.0f;
    public float handSoundCurrentTime = 0.0f;

    private void OnDestroy()
    {
        PlayerMovement.OnPlayerStateChange -= ChasePlayer;
    }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody.isKinematic = true;
        _boxCollider.isTrigger = true;
    }

    private void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager");

        var player = GameObject.FindGameObjectWithTag("Player");
        playerMovementScript = player.GetComponent<PlayerMovement>();
        aloneGolem = playerMovementScript.aloneStayGO;
        aloneGolemTransform = aloneGolem.transform;
        _cameraRef = Camera.main;
        PlayerMovement.OnPlayerStateChange += ChasePlayer;
        StartPosition();
    }

    private void FixedUpdate()
    {
        if (beginChase)
        {
            var newRotation = Quaternion.LookRotation(transform.position - aloneGolemTransform.position, direction);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * 2.0f);

            currentSpeed += Time.fixedDeltaTime / (GameManager.timeSplitBeforeDeath + 1.0f);
            transform.position = Vector3.Lerp(initialPosition, aloneGolemTransform.position, currentSpeed);

            handSoundCurrentTime += Time.fixedDeltaTime;

            if (handSoundCurrentTime >= handSoundTimer)
            {
                // Play again hand sound
                AkSoundEngine.PostEvent("Hand_Trigger", soundManager);
                handSoundCurrentTime = 0.0f;
            }
        }
    }

    private void ChasePlayer()
    {
        if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.ALONE)
        {
            _spriteRenderer.enabled = true;
            beginChase = true;
            currentSpeed = 0.0f;
            
            if (aloneGolemTransform.position.x < _cameraRef.transform.position.x)
            {
                _spriteRenderer.flipX = true;
                direction = -Vector2.right;
            }
            else
            {
                _spriteRenderer.flipX = false;
                direction = Vector2.right;
            }

            AkSoundEngine.PostEvent("HandTimer_Start", soundManager);
            AkSoundEngine.PostEvent("Hand_Trigger", soundManager);
        }
        else
        {
            beginChase = false;
            StartPosition();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AloneStay"))
        {
            GameManager.levelDataComponentScript.SaveLevel(SceneManager.GetActiveScene().buildIndex);
            levelLoader.LoadNextLevel("GameOver");
        }
    }

    private void StartPosition()
    {
        float halfHeight = _cameraRef.orthographicSize;
        transform.position = new Vector3(_cameraRef.transform.position.x, halfHeight + 1.0f, 0.0f);
        initialPosition = transform.position;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        _spriteRenderer.enabled = false;
        _spriteRenderer.flipX = false;
        currentSpeed = 0.0f;
        handSoundCurrentTime = 0.0f;
    }
}
