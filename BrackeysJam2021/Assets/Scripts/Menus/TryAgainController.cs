using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgainController : MonoBehaviour
{
    public LevelLoader levelLoader;

    public void MainMenu()
    {
        Debug.Log("Return to main menu");
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
