using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public LevelLoader levelLoader;

    public void MainMenu()
    {
        Debug.Log("Return to main menu");
        levelLoader.LoadNextLevel("MainMenu");
    }
}
