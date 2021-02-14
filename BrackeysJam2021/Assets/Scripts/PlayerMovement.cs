using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        var axis = context.ReadValue<float>();
        //Debug.Log("Axis- x:" + axis);
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var usePressed = context.ReadValueAsButton();
            Debug.Log("Use state:" + usePressed);
        }
        else if(context.canceled)
        {
            var usePressed = context.ReadValueAsButton();
            Debug.Log("Use state:" + usePressed);
        }
    }

    public void Split(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var splitComplete = context.ReadValueAsButton();
            Debug.Log("Splitting state:" + splitComplete);
        }
        else if (context.canceled)
        {
            var splitComplete = context.ReadValueAsButton();
            Debug.Log("Splitting state:" + splitComplete);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var jumping = context.ReadValueAsButton();
            Debug.Log("Jump state:" + jumping);
        }
        else if (context.canceled)
        {
            var jumping = context.ReadValueAsButton();
            Debug.Log("Jump state:" + jumping);
        }
    }
}
