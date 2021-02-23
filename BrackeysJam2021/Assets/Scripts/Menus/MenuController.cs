using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour, IPointerEnterHandler
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
        Debug.Log("We start game");
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        controlsPanel.enabled = false;
        creditsPanel.enabled = false;
        levelLoader.LoadNextLevel();
    }

    public void CloseGame()
    {
        Debug.Log("we quit game");
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        controlsPanel.enabled = false;
        creditsPanel.enabled = false;
        Application.Quit();
    }

    public void OpenCredits()
    {
        Debug.Log("Roll credits");
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        controlsPanel.enabled = false;
        creditsPanel.enabled = true;
    }

    public void OpenControls()
    {
        Debug.Log("Controls");
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        creditsPanel.enabled = false;
        controlsPanel.enabled = true;
    }

    public void CloseCredits()
    {
        Debug.Log("CLOSE credits");
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        creditsPanel.enabled = false;
    }

    public void CloseControls()
    {
        Debug.Log("CLOSE controls");
        AkSoundEngine.PostEvent("MenuItemClick", gameObject);
        controlsPanel.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AkSoundEngine.PostEvent("MenuItemMouseHover", gameObject);
    }
}
