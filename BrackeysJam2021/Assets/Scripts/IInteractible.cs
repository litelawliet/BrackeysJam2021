using UnityEngine;


public abstract class IInteractible : MonoBehaviour
{
    public virtual void Use()
    {
        Debug.Log("IInteracble interaction");
    }

    [SerializeField]
    [Tooltip("Define if this LOD can be reactivated")]
    public bool Reusable = true;
    public bool Used { get; set; } = false;
}

