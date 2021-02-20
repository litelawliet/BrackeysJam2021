using System.Collections.Generic;
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
                    _rigidbody2D.mass = 100.0f;
                    AkSoundEngine.PostEvent("Woodbreak", gameObject); // OK
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
                    _rigidbody2D.mass = 100.0f;
                    AkSoundEngine.PostEvent("Woodbreak", gameObject); // Ok
                }
            }
        }
        // TODO: add another if case for Enemy Tag
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Draggable"))
        {
            var draggableObject = collision.gameObject;
            var boxColliderDraggable = draggableObject.GetComponent<BoxCollider2D>();
            List<ContactPoint2D> contactList = new List<ContactPoint2D>();
            var contactsCount = boxColliderDraggable.GetContacts(contactList);
            foreach (var contact in contactList)
            {
                if (contact.collider.gameObject.CompareTag("Player"))
                {
                    var playerState = contact.collider.gameObject.GetComponent<PlayerMovement>().PlayerState;
                    if (playerState == PlayerMovement.EPlayerState.TOGETHER)
                    {
                        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                        _rigidbody2D.mass = 100.0f;

                        if (!playerCollapseSound)
                        {
                            playerCollapseSound = true;
                            AkSoundEngine.PostEvent("Woodbreak", gameObject);
                        }
                        break;
                    }
                }
            }
        }
    }

    protected Rigidbody2D _rigidbody2D;
    protected BoxCollider2D _boxCollider2D;
    private bool playerCollapseSound = false;
}
