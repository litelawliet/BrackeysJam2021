using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerState = collision.gameObject.GetComponent<PlayerMovement>().PlayerState;

            if (playerState == PlayerMovement.EPlayerState.TOGETHER)
            {
                SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1));
            }

        }
    }


    // Update is called once per frame
    void Update()
    {

    }

}

