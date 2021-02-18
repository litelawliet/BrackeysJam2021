﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTimerSplit : MonoBehaviour
{
    private bool playerIsTogether = true;

    [SerializeField]
    [Tooltip("Time in second before the alone player dies")]
    int timeSplitBeforeDeath = 15;

    [SerializeField]
    private TMP_Text timerText;

    private float currentTime = 0.0f;

    private float currentSecondsTimer = 0.0f;
    private const  float second = 1.0f;
    private int countDown = 0;

    private void Awake()
    {
        countDown = timeSplitBeforeDeath;
    }

    private void Start()
    {
        PlayerMovement.OnPlayerStateChange += TimerStarter;
    }

    private void Update()
    {
        if (!playerIsTogether)
        {
            currentTime += Time.deltaTime;
            currentSecondsTimer += Time.deltaTime;
            timerText.text = countDown.ToString();
            
            if (currentSecondsTimer >= second)
            {
                currentSecondsTimer -= second;
                countDown--;
            }

            if (currentTime >= timeSplitBeforeDeath)
            {
                // Game over
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
        else
        {
            timerText.text = "Safe";
        }

    }

    private void TimerStarter()
    {
        playerIsTogether = !playerIsTogether;
        if (playerIsTogether)
        {
            currentTime = 0.0f;
            currentSecondsTimer = 0.0f;
            countDown = timeSplitBeforeDeath;
        }
    }
}
