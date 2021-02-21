using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class EnemyBob : MonoBehaviour
{
    private bool leftDirection = true;
    private bool afraid = false;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private Animator _animator;
    private LevelLoader levelLoader;

    private float distance = 0.0f;
    private float direction = 0.0f;

    [SerializeField]
    [Tooltip("Enemy speed")]
    private float speed = 5.0f;
    [SerializeField]
    [Tooltip("Enemy player detection")]
    private float detectionRange = 2.2f;
    [SerializeField]
    [Tooltip("Range of death near Alone player")]
    private float deathRange = 1.0f;

    public GameObject aloneStayGolem = null;
    GameObject player = null;
    PlayerMovement playerMovementScript = null;
    SpriteRenderer spriteRenderer = null;

    private bool AttackStarted = false;
    private bool StealStarted = false;

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("BobIdle_Stop", gameObject); // Probably not used because we never destroy Bob
    }

    private void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerMovementScript = player.GetComponent<PlayerMovement>();
        aloneStayGolem = playerMovementScript.aloneStayGO;

        distance = boxCollider.bounds.size.x / 2.0f + 0.1f;
        direction = leftDirection ? -1.0f : 1.0f;
        transform.Rotate(0.0f, 180.0f, 0.0f);

        AkSoundEngine.PostEvent("BobIdle_Start", gameObject);
    }

    private void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);
        float golemDistance = Vector2.Distance(transform.position, aloneStayGolem.transform.position);
        if (playerDistance <= detectionRange || golemDistance <= detectionRange)
        {
            if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.TOGETHER)
            {
                afraid = true;
                AkSoundEngine.PostEvent("BobSeePlayer", gameObject);
            }
            else
            {
                afraid = false;
                if (!AttackStarted)
                {
                    AkSoundEngine.PostEvent("BobAttack", gameObject);
                    AttackStarted = true;
                }
                else
                {
                    AttackStarted = false;
                }
            }
        }
        else
        {
            afraid = false;
        }

        if (afraid)
        {
            float fleeDirection = (transform.position - player.transform.position).x;
            if (fleeDirection < 0.0f)
            {
                fleeDirection = -1.0f;
            }
            else
            {
                fleeDirection = 1.0f;
            }

            if (direction != fleeDirection)
            {
                _animator.SetTrigger("Afraid");
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
            direction = fleeDirection;
        }
        else
        {
            if (playerDistance <= deathRange || golemDistance <= deathRange)
            {
                // Play animation
                if (!StealStarted)
                {
                    _animator.SetTrigger("Steal");
                    AkSoundEngine.PostEvent("BobMunch", gameObject);
                    StealStarted = true;
                }

                // Notify the player to stop the inputs
                StartCoroutine(Restart());
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0.0f;
                direction = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        var previousDir = direction;
        int mask = ~(LayerMask.GetMask("Enemy") + LayerMask.GetMask("StayPlayer"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * direction, distance, mask);

        if (hit.collider != null && !hit.collider.CompareTag("Player"))
        {
            // more conditions with player so when we're alone player gets eaten
            leftDirection = !leftDirection;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            rb.angularVelocity = 0.0f;
            //transform.Rotate(0.0f, 180.0f, 0.0f);
            direction = leftDirection ? -1.0f : 1.0f;
        }

        if (direction != previousDir)
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        float acceleration = this.speed * direction * Time.fixedDeltaTime;
        rb.AddForce(Vector2.right * acceleration, ForceMode2D.Impulse);
        float x = Mathf.Clamp(rb.velocity.x, -this.speed * 0.667f, this.speed * 0.667f);
        rb.velocity = new Vector2(x, rb.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * distance);

        if (afraid)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, 10.0f); // Potential check of enemy in range of player for sounds

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, deathRange);
    }

    private IEnumerator Restart()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerInput = player.GetComponent<PlayerInput>();
        var playerRigidbody = player.GetComponent<Rigidbody2D>();
        playerInput.enabled = false;
        playerRigidbody.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.6f);

        // Save level via SaveSystem
        GameManager.levelDataComponentScript.SaveLevel(SceneManager.GetActiveScene().buildIndex);
        levelLoader.LoadNextLevel("GameOver");
    }

    public void PlayFootsteps()
    {
        AkSoundEngine.PostEvent("BobFootsteps", gameObject);
    }
}
