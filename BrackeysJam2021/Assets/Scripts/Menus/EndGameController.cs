using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public LevelLoader levelLoader;

    private GameObject soundManager;
    private SoundManager soundManagerComponent;

    private void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager");
        soundManagerComponent = soundManager.GetComponent<SoundManager>();
    }

    public void MainMenu()
    {
        Debug.Log("Return to main menu");
        AkSoundEngine.PostEvent("MainMusic_Stop", soundManager);
        AkSoundEngine.PostEvent("MainAmbiance_Stop", soundManager);
        soundManagerComponent.PlayMainMusic(true);

        levelLoader.LoadNextLevel("MainMenu");
    }
}
