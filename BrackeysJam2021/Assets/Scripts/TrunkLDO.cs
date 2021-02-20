using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkLDO : MonoBehaviour
{
    public void OnFall()
    {
        AkSoundEngine.PostEvent("LogCrack", gameObject);
    }   
}
