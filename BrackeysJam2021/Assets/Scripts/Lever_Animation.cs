using UnityEngine;

public class Lever_Animation : IInteractible
{
    [SerializeField]
    [Tooltip("Game object animated to play")]
    private GameObject animatedGameObject = null;

    [SerializeField]
    [Tooltip("Name of the animation to play on use")]
    private string animationName = "";

    public override void Use()
    {
        base.Use();

        _animator = animatedGameObject.GetComponent<Animator>();
        _animator.enabled = true;
        _animator.Play(animationName);
    }
}
