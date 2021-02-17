﻿using UnityEngine;

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
        if (transform.position.y < -500.0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerMovementScript = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovementScript != null)
            {
                if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.TOGETHER)
                {
                    _rigidbody2D.constraints = RigidbodyConstraints2D.None;
                }
            }
        }
        else if (collision.gameObject.layer == LayerMask.GetMask("Draggable"))
        {
            var otherRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (otherRigidbody != null)
            {
                _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            }
        }
        // TODO: add another if case for Enemy Tag
    }

    protected Rigidbody2D _rigidbody2D;
    protected BoxCollider2D _boxCollider2D;
}