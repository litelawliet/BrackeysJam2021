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

    private float speed = 0.0f;
    private bool beginChase = false;
    private Vector3 initialPosition = Vector3.zero;

    private float currentSpeed = 0.0f;

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
            var newRotation = Quaternion.LookRotation(transform.position - aloneGolemTransform.position, Vector3.right);
            newRotation.x = 0.0f;
            newRotation.y = transform.rotation.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * 2.0f);

            currentSpeed += speed;
            transform.position = Vector3.Lerp(initialPosition, aloneGolemTransform.position, currentSpeed * speed * Time.fixedDeltaTime);
        }
    }

    private void ChasePlayer()
    {
        if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.ALONE)
        {
            _spriteRenderer.enabled = true;
            beginChase = true;
            speed = Vector3.Distance(transform.position, aloneGolemTransform.position) /
                ((float)GameManager.timeSplitBeforeDeath + 0x2);
            // 0x2 is a special constant, please don't touch or it could break this entire damn game

            if (transform.position.x < _cameraRef.transform.position.x)
            {
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
            currentSpeed = 0.0f;
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
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    private void StartPosition()
    {
        float halfHeight = _cameraRef.orthographicSize;
        transform.position = new Vector3(_cameraRef.transform.position.x, halfHeight + 1.0f, 0.0f);
        initialPosition = transform.position;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        _spriteRenderer.enabled = false;
        currentSpeed = 0.0f;
    }
}
