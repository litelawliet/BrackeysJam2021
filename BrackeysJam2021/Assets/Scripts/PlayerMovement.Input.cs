using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerMovement
{
    #region PlayerActions Events
    public void Move(InputAction.CallbackContext context)
    {
        var axis = context.ReadValue<float>();
        
        if (!Mathf.Approximately(Mathf.Abs(axis), 0.0f))
        {
            if (!SameSign(axis, previousSpeedDirection))
            {
                transform.Rotate(Vector3.up, 180.0f);
            }
        }

        speedDirection = axis;
        if (!Mathf.Approximately(Mathf.Abs(speedDirection), 0.0f))
        {
            previousSpeedDirection = speedDirection;
        }
    }

    public void Use(InputAction.CallbackContext context)
    {
        var usePressed = context.ReadValueAsButton();
        if (context.performed)
        {
            if (CanInteract())
            {
                UseClosest();
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetJumpState(context.ReadValueAsButton());
        }
    }

    public void Split(InputAction.CallbackContext context)
    {
        if (!isJumping)
        {
            if (context.started)
            {
                if (PlayerState == EPlayerState.ALONE)
                {
                    if (CanFusion())
                    {
                        SwitchPlayerState();
                        transform.position = aloneStayGO.transform.position;
                        aloneStayGO.SetActive(false);
                    }
                }
            }
            if (context.performed)
            {
                if (PlayerState == EPlayerState.TOGETHER)
                {
                    SwitchPlayerState();
                    aloneStayGO.transform.position = transform.position;
                    aloneStayGO.SetActive(true);
                }
            }
        }
    }
    #endregion
}
