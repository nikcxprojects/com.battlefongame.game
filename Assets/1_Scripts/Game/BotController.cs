using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : IControllable
{

    private void Start()
    {
        Init();
    }
    
    public bool ShouldBotMove()
    {
        return gameManager.MoveCount == 1;
    }
    
    public override void Move(GameObject card)
    {
        if (ShouldBotMove())
        {
            StartCoroutine(Moving(this, card));
            StartCoroutine(Resize(card, 1.5f));
            
            card.GetComponent<Card>().InGame = true;
            gameManager.cardsInGame.Add(card.GetComponent<Card>());
            gameManager.MoveCount++;
            StartCoroutine(gameManager.CompareCard());
        }
    }
}
