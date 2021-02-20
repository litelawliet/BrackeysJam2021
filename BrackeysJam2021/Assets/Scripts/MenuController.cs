using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("We start game");
    }

    public void CloseGame()
    {
        Debug.Log("we quit game");
        Application.Quit();
    }

    public void OpenCredits()
    {
        Debug.Log("Roll credits");
    }

    public void OpenTutorial()
    {
        Debug.Log("Tutorials");
    }
}
