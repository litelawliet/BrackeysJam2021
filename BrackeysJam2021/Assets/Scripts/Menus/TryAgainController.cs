using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgainController : MonoBehaviour
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

    public void Retry()
    {
        Debug.Log("Retry");
        GameManager.levelDataComponentScript.LoadLevel();

        var previousLevelIndex = GameManager.levelDataComponentScript.indexLevel;
        levelLoader.LoadNextLevel(previousLevelIndex);
    }
}
