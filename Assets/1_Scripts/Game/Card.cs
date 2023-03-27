using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool InGame;

    public int value;

    public GameManager.PlayerType type;

    private Animation _animation;
    
    private void Start()
    {
        _animation = GetComponent<Animation>();
    }

    public void PlayAnim(string animationName)
    {
        _animation.Play(animationName);
    }
}
