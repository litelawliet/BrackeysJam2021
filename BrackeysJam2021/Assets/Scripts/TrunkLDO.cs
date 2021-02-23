using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkLDO : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }
    private void Start()
    {
        
    }

    public void OnFall()
    {
        AkSoundEngine.PostEvent("LogCrack", gameObject);
    }

    public void OnHit()
    {
        AkSoundEngine.PostEvent("LogFallHit", gameObject);
    }
}
