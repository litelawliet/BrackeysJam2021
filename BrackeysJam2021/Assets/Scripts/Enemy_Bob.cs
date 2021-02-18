using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy_Bob: MonoBehaviour
{
    private bool leftDirection = true;
    private bool afraid = false;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;

    private float distance = 0.0f;
    private float direction = 0.0f;

    [SerializeField]
    [Tooltip("Enemy speed")]
    private float EnemySpeed = 5.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        distance = boxCollider.bounds.size.x / 2.0f + 0.1f;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        direction = leftDirection ? -1.0f : 1.0f;
        int mask =  ~(LayerMask.GetMask("Enemy"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * direction, distance, mask);

        if (hit.collider != null && !hit.collider.CompareTag("Player"))
        {
            // more conditions with player so when we're together player gets eaten
            leftDirection = !leftDirection;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            rb.angularVelocity = 0.0f;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }


        float speed = EnemySpeed * direction * Time.fixedDeltaTime;
        rb.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
        float x = Mathf.Clamp(rb.velocity.x, -EnemySpeed * 0.667f, EnemySpeed * 0.667f);
        rb.velocity = new Vector2(x, rb.velocity.y);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;    //for visual debugging purpose
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * distance);
    }
}
