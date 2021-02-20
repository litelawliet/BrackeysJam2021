using UnityEngine;

public class Draggable : IInteractible
{
    private bool startPushing = false;
    private void Update()
    {
        if (_rigidbody2D.velocity.x < -0.01f || _rigidbody2D.velocity.x > 0.01f)
        {
            if (!startPushing)
            {
                AkSoundEngine.PostEvent("RockPush_Start", gameObject);
                startPushing = true;
            }
        }
        else
        {
            if (startPushing)
            {
                // was pushing
                AkSoundEngine.PostEvent("RockPush_Stop", gameObject);
                startPushing = false;
            }
        }
    }
}
