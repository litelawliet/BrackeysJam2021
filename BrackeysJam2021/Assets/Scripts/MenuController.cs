﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public LevelLoader levelLoader;

    public Canvas creditsPanel;
    public Canvas controlsPanel;

    private void Start()
    {
        creditsPanel.enabled = false;
        controlsPanel.enabled = false;
    }

    public void StartGame()
    {
        Debug.Log("We start game");
        controlsPanel.enabled = false;
        creditsPanel.enabled = false;
        levelLoader.LoadNextLevel();
    }

    public void CloseGame()
    {
        Debug.Log("we quit game");
        controlsPanel.enabled = false;
        creditsPanel.enabled = false;
        Application.Quit();
    }

    public void OpenCredits()
    {
        Debug.Log("Roll credits");
        controlsPanel.enabled = false;
        creditsPanel.enabled = true;
    }

    public void OpenControls()
    {
        Debug.Log("Controls");
        creditsPanel.enabled = false;
        controlsPanel.enabled = true;
    }

    public void CloseCredits()
    {
        Debug.Log("CLOSE credits");

        creditsPanel.enabled = false;
    }

    public void CloseControls()
    {
        Debug.Log("CLOSE controls");

        controlsPanel.enabled = false;
    }
}