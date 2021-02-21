using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public LevelLoader levelLoader;
    [SerializeField]
    [Tooltip("Level name to load when reaching")]
    private string LevelName = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerState = collision.gameObject.GetComponent<PlayerMovement>().PlayerState;

            if (playerState == PlayerMovement.EPlayerState.TOGETHER)
            {
                if (LevelName == null)
                {
                    levelLoader.LoadNextLevel();
                }
                else
                {
                    levelLoader.LoadNextLevel(LevelName);
                }
            }

        }
    }
}

