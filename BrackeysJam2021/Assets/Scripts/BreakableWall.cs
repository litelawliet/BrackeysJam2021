using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class BreakableWall : MonoBehaviour
{
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _animator.enabled = false;

        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
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
                    StartCoroutine(WallDestruction());
                }
            }
        }
    }

    private IEnumerator WallDestruction()
    {
        _animator.enabled = true;
        _animator.Play("BreakableWall_Destroy");
        AkSoundEngine.PostEvent("Woodbreak", gameObject);

        var clips = _animator.GetCurrentAnimatorClipInfo(0);
        
        yield return new WaitForSeconds(clips[0].clip.length);
        _boxCollider2D.isTrigger = true;
        gameObject.SetActive(false);
    }


    protected Rigidbody2D _rigidbody2D;
    protected BoxCollider2D _boxCollider2D;
    protected Animator _animator;
}
