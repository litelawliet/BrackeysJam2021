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

        if (jumpCalledInput)
        {
            if (IsGrounded(out RaycastHit2D hit))
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
            if (IsGrounded(out _))
            {
                jumpCalledInput = false;
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

    private bool IsGrounded(out RaycastHit2D hit)
    {
        int layerMask = ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Interactible") + LayerMask.GetMask("DraggableBlocker")
                        + LayerMask.GetMask("StayPlayer"));

        float rayDistance = playerGroundCollider.bounds.extents.y + 0.05f;
        Vector2 boxCastSize = new Vector2(playerGroundCollider.bounds.size.x - 0.1f, playerGroundCollider.bounds.extents.y);

        hit = Physics2D.BoxCast(playerGroundCollider.bounds.center, boxCastSize, 0.0f, Vector2.down, rayDistance, layerMask);

        Debug.DrawLine(playerGroundCollider.bounds.center, Vector3.down * hit.distance);

        Color rayColor = Color.green;
        if (hit.collider == null)
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(playerGroundCollider.bounds.center + new Vector3(playerGroundCollider.bounds.extents.x - 0.01f, 0.0f), Vector3.down * rayDistance, rayColor);
        Debug.DrawRay(playerGroundCollider.bounds.center - new Vector3(playerGroundCollider.bounds.extents.x - 0.01f, 0.0f), Vector3.down * rayDistance, rayColor);
        Debug.DrawRay(playerGroundCollider.bounds.center - new Vector3(playerGroundCollider.bounds.extents.x - 0.01f, rayDistance), Vector2.right * (playerGroundCollider.bounds.size.x - 0.01f), rayColor);
        //Debug.Log(hit.collider);

        return hit.collider != null;
    }
}
