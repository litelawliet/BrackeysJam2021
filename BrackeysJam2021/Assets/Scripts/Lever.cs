using UnityEngine;

public class Lever : IInteractible
{
    /**
     * We can add Start, Update etc. here
     * */

    public override void Use()
    {
        Debug.Log("Lever interaction");
        if (Usable)
        {
            // Do something with that
            transform.gameObject.SetActive(false);
        }
    }
}
