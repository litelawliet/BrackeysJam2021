using UnityEngine;


public abstract class IInteractible : MonoBehaviour
{
    public virtual void Use()
    {
        Debug.Log("IInteracble interaction");
    }
}

