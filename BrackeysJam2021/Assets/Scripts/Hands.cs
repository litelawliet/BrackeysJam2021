using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Hands : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Rigidbody2D rb;
    public GameObject aloneGolem = null;
    public PlayerMovement playerMovementScript = null;
    Transform aloneGolemTransform;

    private bool beginChase = false;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        boxCollider.isTrigger = true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerMovementScript = player.GetComponent<PlayerMovement>();
        aloneGolem = playerMovementScript.aloneStayGO;
        aloneGolemTransform = aloneGolem.transform;
        PlayerMovement.OnPlayerStateChange += ChasePlayer;
    }

    // Update is called once per frame
    private void Update()
    {
        if (beginChase)
        {
            float step = 1.0f * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, aloneGolemTransform.position, step);
            var newRotation = Quaternion.LookRotation(transform.position - aloneGolemTransform.position, Vector3.right);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);
        }
    }

    private void ChasePlayer()
    {
        if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.ALONE)
        {
            beginChase = true;
        }
        else
        {
            beginChase = false;
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
}
