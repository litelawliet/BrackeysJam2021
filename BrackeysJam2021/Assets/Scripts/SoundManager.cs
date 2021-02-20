using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // Sound test
        AkSoundEngine.PostEvent("MainMusic_Start", gameObject);

        AkSoundEngine.PostEvent("MainAmbiance_Start", gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
