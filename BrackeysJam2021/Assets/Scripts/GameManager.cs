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
    public static int timeSplitBeforeDeath = 20;

    [SerializeField]
    [Tooltip("Level loader of the game")]
    public LevelLoader levelLoader;

    [Tooltip("Save file system data")]
    [SerializeField]
    public GameObject saveSystemPrefab;

    public static LevelDataComponent levelDataComponentScript;

    private void OnDestroy()
    {
        interactibles.Clear();
    }

    private void Awake()
    {
       // DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        levelDataComponentScript = saveSystemPrefab.GetComponent<LevelDataComponent>();

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
        if (player != null)
        {
            if (player.transform.position.y <= deathLimit)
            {
                levelDataComponentScript.SaveLevel(SceneManager.GetActiveScene().buildIndex);
                levelLoader.LoadNextLevel("GameOver");
            }
        }
    }
}
