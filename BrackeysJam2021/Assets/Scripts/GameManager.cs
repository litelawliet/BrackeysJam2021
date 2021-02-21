using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Dictionary<GameObject, IInteractible> interactibles = new Dictionary<GameObject, IInteractible>();
    public static float deathLimit = -100.0f;
    [SerializeField]
    [Tooltip("Negative Value which tells when objects have to be disposed in order to eventually reset the level (such as the Player for example).")]
    private GameObject player;

    [SerializeField]
    [Tooltip("Time in second before the alone player dies")]
    public static int timeSplitBeforeDeath = 15;

    [SerializeField]
    [Tooltip("Level loader of the game")]
    public LevelLoader levelLoader;

    private void OnDestroy()
    {
        interactibles.Clear();
    }

    private void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();

        var gos = GameObject.FindGameObjectsWithTag("Interactible");
        foreach(var go in gos)
        {
            interactibles.Add(go, go.GetComponent<IInteractible>());
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    private void Update()
    {
        if (player.transform.position.y <= deathLimit)
        {
            levelLoader.LoadNextLevel("GameOver");
        }
    }
}
