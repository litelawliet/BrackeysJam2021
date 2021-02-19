using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class BreakableGround : MonoBehaviour
{
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Update()
    {
        if (transform.position.y < GameManager.deathLimit)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerMovementScript = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovementScript != null)
            {
                if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.TOGETHER)
                {
                    _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    _rigidbody2D.mass = 10.0f;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Draggable"))
        {
            var otherRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            var otherScript = collision.gameObject.GetComponent<IInteractible>();
            if (otherRigidbody != null && otherScript != null)
            {
                if (!otherScript.AloneCanDrag)
                {
                    _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    _rigidbody2D.mass = 10.0f;
                }
            }
        }
        // TODO: add another if case for Enemy Tag
    }

    protected Rigidbody2D _rigidbody2D;
    protected BoxCollider2D _boxCollider2D;
}
