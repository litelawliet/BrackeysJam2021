using UnityEngine;

public class Lever : IInteractible
{
    /**
     * We can add Start, Update etc. here
     * */
    [SerializeField]
    [Tooltip("Cooldown between each activation")]
    private float activationCooldown;
    private float currentTime = 0.0f;
    private bool inCooldown = false;

    [SerializeField]
    [Tooltip("Set the platform to move")]
    private GameObject targetToMove;
    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    [Tooltip("End position")]
    private Vector3 endPosition;
    [SerializeField]
    [Tooltip("Time to move the platforms from point A to point B")]
    private float timeToMove = 2.0f;

    private float t = 0.0f;
    private float movingTime = 0.0f;
    private bool moving = false;
    private bool back = true;

    private void Update()
    {
        if (inCooldown)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= activationCooldown)
            {
                inCooldown = false;
                currentTime = 0.0f;
            }
        }

        if (moving)
        {
            movingTime += Time.deltaTime;
            if (movingTime >= timeToMove)
            {
                AkSoundEngine.PostEvent("PlatformLoop_Stop", targetToMove);
                moving = false;
                movingTime = 0.0f;
                t = 0.0f;
            }
        }
        if (moving)
        { 
            if (targetToMove != null)
            {
                t += Time.deltaTime / timeToMove;
                Vector3 start;
                Vector3 end;
                if (back)
                {
                    start = endPosition;
                    end = startPosition;
                }
                else
                {
                    start = startPosition;
                    end = endPosition;
                }
                targetToMove.transform.position = Vector3.Lerp(start, end, t);
            }
        }

    }

    public override void Use()
    {
        if (!inCooldown)
        {
            if (Usable)
            {
                if (Reusable)
                {
                    // Lever sound
                    AkSoundEngine.PostEvent("LeverActivate", gameObject);
                }
                else
                {
                    // Bulbe sound
                    AkSoundEngine.PostEvent("BulbExplode", gameObject);
                }
                StartCooldown();
            }
        }
    }

    private void StartCooldown()
    {
        inCooldown = true;
        moving = true;
        back = !back;

        currentTime = 0.0f;
        movingTime = 0.0f;
        t = 0.0f;

        AkSoundEngine.PostEvent("PlatformLoop_Start", targetToMove);
    }
}
