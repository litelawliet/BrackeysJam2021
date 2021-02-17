using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Dictionary<GameObject, IInteractible> interactibles = new Dictionary<GameObject, IInteractible>();
    // Start is called before the first frame update
    void Start()
    {
        var gos = GameObject.FindGameObjectsWithTag("Interactible");
        foreach(var go in gos)
        {
            interactibles.Add(go, go.GetComponent<IInteractible>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
