﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyBob : MonoBehaviour
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
    private float speed = 5.0f;
    [SerializeField]
    [Tooltip("Enemy speed")]
    private float detectionRange = 2.2f;

    GameObject player = null;
    PlayerMovement playerMovementScript = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        distance = boxCollider.bounds.size.x / 2.0f + 0.1f;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovementScript = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            // Could use another condition to tell if they virtually colide, in which case we can
            // set the player to die

            if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.TOGETHER)
            {
                afraid = true;
            }
            else
            {
                afraid = false;
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

            direction = fleeDirection;
        }
    }

    private void FixedUpdate()
    {
        int mask = ~(LayerMask.GetMask("Enemy") + LayerMask.GetMask("StayPlayer"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * direction, distance, mask);

        if (hit.collider != null && !hit.collider.CompareTag("Player"))
        {
            // more conditions with player so when we're together player gets eaten
            leftDirection = !leftDirection;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            rb.angularVelocity = 0.0f;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            direction = leftDirection ? -1.0f : 1.0f;
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
    }
}
