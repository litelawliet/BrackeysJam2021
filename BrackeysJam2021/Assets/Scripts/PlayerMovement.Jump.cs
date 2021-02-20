using UnityEngine;

public partial class PlayerMovement
{
    #region Jump settings
    [Header("Jumping settings", order = 1)]
    [SerializeField]
    [Tooltip("Fall multiplier to bring the player faster to ground. 1 = physically accurate")]
    private float fallMultiplier = 2.5f;
    [SerializeField]
    [Tooltip("Percentage of reduction of the air control." +
        "0 = Complete control" +
        "1 = No control")]
    [Range(0.0f, 1.0f)]
    float airControlReducer = 1.0f;
    private float floatHeight = 0.0f;
    private float damping = 0.5f;
    private bool isJumping = false;
    private bool jumpCalledInput = false;
    #endregion

    private void JumpAction()
    {
        if (rb.velocity.y < 0.0f)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1.0f) * Time.fixedDeltaTime;
        }

        if (jumpCalledInput && !isJumping)
        {
            int layerMask = ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Interactible") + LayerMask.GetMask("DraggableBlocker")
                + LayerMask.GetMask("StayPlayer"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, layerMask);

            if (hit.collider != null)
            {
                // Calculate the distance from the surface and the "error" relative
                // to the floating height.
                float distance = Mathf.Abs(hit.point.y - transform.position.y);

                float heightError = floatHeight - distance;

                // The force is proportional to the height error, but we remove a part of it
                // according to the object's speed.
                float force = liftForce * heightError - rb.velocity.y * damping;

                rb.AddForce(Vector3.up * force);

                jumpCalledInput = false;
                isJumping = true;
                playerAnimator.SetBool("IsJumping", true);
            }
        }
        else
        {
            int layerMask = ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Interactible") + LayerMask.GetMask("DraggableBlocker")
                + LayerMask.GetMask("StayPlayer"));
            float rayDistance = playerGroundCollider.bounds.size.y / 2.0f + 0.05f;
            RaycastHit2D hit = Physics2D.Raycast(playerGroundCollider.bounds.center, -Vector2.up, rayDistance, layerMask);
            Debug.DrawLine(playerGroundCollider.bounds.center, transform.position - transform.up * rayDistance, Color.red);

            if (hit.collider != null)
            {
                isJumping = false;
                playerAnimator.SetBool("IsJumping", false);
            }
        }
    }

    public void SetJumpState(bool jumpingState)
    {
        if (!isJumping)
        {
            jumpCalledInput = jumpingState;
        }
    }
}
