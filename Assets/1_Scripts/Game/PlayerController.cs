using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : IControllable
{
    
    private void Start()
    {
        Init();
        
        foreach (var obj in cards) obj.GetComponent<Button>().onClick.AddListener(() => Move(obj));
    }
    
    public bool ShouldPlayerMove()
    {
        return gameManager.MoveCount == 0;
    }

    void Update()
    {

    }

    
    public override void Move(GameObject card)
    {
        if (ShouldPlayerMove())
        {
            StartCoroutine(Moving(this, card));
            StartCoroutine(Resize(card, 1.5f));

            card.GetComponent<Card>().InGame = true;
            
            gameManager.MoveCount++;
            gameManager.cardsInGame.Add(card.GetComponent<Card>());

            StartCoroutine(BotMove());
        }
        UpdateCards();
        if(cards.Count < 2)
        {
            cards.Add(new GameObject());
            StartCoroutine(gameManager.FinishGame());
        }
    }

    private IEnumerator BotMove()
    {
        yield return new WaitForSeconds(1.5f);
        gameManager.PlayerMoved();
    }
    
}
