using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance { get { return _instance; } }

    public static bool startedEvent = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        StartMainMusicOnce();
    }

    public void StartMainMusicOnce()
    {
        if (!startedEvent)
        {
            AkSoundEngine.PostEvent("MainMusic_Start", Instance.gameObject);
            AkSoundEngine.PostEvent("MainAmbiance_Start", Instance.gameObject);

            startedEvent = true;
        }
        AkSoundEngine.PostEvent("HandTimer_Stop", Instance.gameObject); //reset du RTPC
    }

    public void PlayMainMusic(bool state)
    {
        startedEvent = !state;
    }
}
