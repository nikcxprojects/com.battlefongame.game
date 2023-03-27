using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IControllable : MonoBehaviour
{
    public abstract void Move(GameObject card);
    
    public GameManager.ConfigPlayer configPlayer;
    public GameManager gameManager;

    [HideInInspector]
    public int WinCount;
    public Text winCountText;
    public AudioClip moveClip;

    public GameManager.PlayerType type;

    protected readonly List<GameObject> cards = new List<GameObject>();
    
    protected void Init()
    {
        gameManager.GenerateCards(this);
        UpdateCards();
    }

    protected IEnumerator Moving(IControllable player, GameObject card)
    {
        AudioManager.getInstance().PlayAudio(moveClip);
        card.GetComponent<Card>().PlayAnim("Open");
        float time = 0;
        while (time < 1)
        {
            float t = time / 1;
            t = t * t * (3f - 2f * t);
            time += Time.deltaTime;
            var currentVector = card.transform.position;
            currentVector.x = Mathf.Lerp(player.configPlayer.startPos.position.x, 
                player.configPlayer.gamePos.position.x, t);
            currentVector.y = Mathf.Lerp(player.configPlayer.startPos.position.y, 
                player.configPlayer.gamePos.position.y, t);
            card.transform.position = currentVector;
            yield return 1;  
        }
    }

    protected IEnumerator Resize(GameObject card, float size)
    {
        float time = 0;
        while (time < 1)
        {
            float t = time / 1;
            t = t * t * (3f - 2f * t);
            time += Time.deltaTime;
            var startSize = card.transform.localScale.x;
            var currentSize = Mathf.Lerp(startSize, 
                size, t);
            card.transform.localScale = Vector3.one * currentSize;
            yield return 1;  
        }
    }

    public void UpdateCards()
    {
        cards.Clear();
        var tr = transform.Find("Cards");
        foreach (Transform t in tr)
        {
            cards.Add(t.gameObject);
        }
    }
    
    public GameObject GetTopCard()
    {
        return cards[cards.Count - 1];
    }
    
}