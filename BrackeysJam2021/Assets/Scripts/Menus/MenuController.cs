using UnityEngine;

public class MenuController : MonoBehaviour
{
    public LevelLoader levelLoader;

    public Canvas creditsPanel;
    public Canvas controlsPanel;

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("MainMenu_Stop", gameObject);
    }

    private void Start()
    {
        AkSoundEngine.PostEvent("MainMenu_Start", gameObject);
        creditsPanel.enabled = false;
        controlsPanel.enabled = false;
    }

    public void StartGame()
    {
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        controlsPanel.enabled = false;
        creditsPanel.enabled = false;
        levelLoader.LoadNextLevel();
    }

    public void CloseGame()
    {
        controlsPanel.enabled = false;
        creditsPanel.enabled = false;
        Application.Quit();
    }

    public void OpenCredits()
    {
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        controlsPanel.enabled = false;
        creditsPanel.enabled = true;
    }

    public void OpenControls()
    {
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        creditsPanel.enabled = false;
        controlsPanel.enabled = true;
    }

    public void CloseCredits()
    {
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        creditsPanel.enabled = false;
    }

    public void CloseControls()
    {
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        controlsPanel.enabled = false;
    }
}
