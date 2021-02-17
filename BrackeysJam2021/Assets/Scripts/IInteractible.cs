using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class IInteractible : MonoBehaviour
{
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (Draggable)
        {
            _boxCollider2D.isTrigger = false;
            _rigidbody2D.isKinematic = false;
            transform.gameObject.layer = LayerMask.NameToLayer("Draggable");
            Reusable = false;
            Used = true;
        }
        else
        {
            _boxCollider2D.isTrigger = true;
            _rigidbody2D.isKinematic = true;
            transform.gameObject.layer = LayerMask.NameToLayer("Interactible");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var go = collision.gameObject;
        if (go.CompareTag("Player"))
        {
            var playerMovementScript = go.GetComponent<PlayerMovement>();

            if (playerMovementScript != null)
            {
                if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.ALONE)
                {
                    if (!AloneCanDrag)
                    {
                        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                }
                else if (playerMovementScript.PlayerState == PlayerMovement.EPlayerState.TOGETHER)
                {
                    _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }
    }

    public virtual void Use()
    {
        Debug.Log("IInteracble interaction");
    }

    [SerializeField]
    [Tooltip("Define if this LOD can be reactivated")]
    private bool Reusable = true;
    public bool Used { get; set; } = false;
    public bool Usable
    {
        get
        {
            if (Reusable || !Used)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [SerializeField]
    [Tooltip("Define if this LOD can be drag. This boolean is in priority and will disable interactivity of Reusable and Used")]
    public bool Draggable = false;
    [SerializeField]
    [Tooltip("Define if this LOD can be dragged by alone player")]
    public bool AloneCanDrag = false;

    protected Rigidbody2D _rigidbody2D;
    protected BoxCollider2D _boxCollider2D;
}

