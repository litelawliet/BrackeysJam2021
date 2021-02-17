using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class BreakableWall : MonoBehaviour
{
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerMovementScript = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.TOGETHER)
            {
                gameObject.SetActive(false);
            }
        }
    }

    protected Rigidbody2D _rigidbody2D;
    protected BoxCollider2D _boxCollider2D;
}
