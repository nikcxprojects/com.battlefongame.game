using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    
    [Header("Main Settings")]
    [SerializeField] private GameObject _prefabCard;
    [SerializeField] private GameObject _finishUI;
    [SerializeField] private Text _moveCountText;
    [SerializeField] private GameConfig _config;

    [HideInInspector]
    public int MoveCount = 0;
    
    private int _moveCount = 1;
    
    [Header("Players")]
    [SerializeField] private BotController _bot;
    [SerializeField] private PlayerController _player;

    [Header("AudioClips")] 
    [SerializeField] private AudioClip _equalityClip;
    [SerializeField] private AudioClip _loseClip;
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _endGameClip;

    public List<Card> cardsInGame = new List<Card>();

    private void Start()
    {
        _finishUI.SetActive(false);
    }
    
    public void PlayerMoved()
    {
        _bot.UpdateCards();
        _bot.Move(_bot.GetTopCard());
    }

    public IEnumerator CompareCard()
    {
        yield return new WaitForSeconds(1f);
        
        var continueGame = false;
        
        for (var i = 0; i < cardsInGame.Count; i += 2)
        {
            if (i == cardsInGame.Count - 1) break;
            continueGame = cardsInGame[i].value == cardsInGame[i + 1].value;
        }
        
        yield return new WaitForSeconds(0.8f);

        MoveCount = 0;
        
        if (!continueGame)
        {
            _moveCount++;
            ResultRound();
            UpdateUI();
            foreach (var obj in cardsInGame) Destroy(obj.gameObject);
            cardsInGame.Clear();
        }
        else
        {
            AudioManager.getInstance().PlayAudio(_equalityClip);
            foreach (var obj in cardsInGame)
            {
                obj.PlayAnim("Close");
                obj.transform.parent = transform;
                obj.transform.localScale = Vector3.one;
            }
        }
    }

    public IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(3);
        
        AudioManager.getInstance().PlayAudio(_endGameClip);
        _finishUI.SetActive(true);
        
        var rec1 = PlayerPrefs.GetInt("R1");
        var rec2 = PlayerPrefs.GetInt("R2");
        var rec3 = PlayerPrefs.GetInt("R3");
        var rec4 = PlayerPrefs.GetInt("R4");
        var rec5 = PlayerPrefs.GetInt("R5");
        List<int> recs = new List<int>()
        {
            rec1,
            rec2,
            rec3,
            rec4,
            rec5
        };
        recs.Add(_moveCount);
        PlayerPrefs.SetInt("R1", recs[0]);
        PlayerPrefs.SetInt("R2", recs[1]);
        PlayerPrefs.SetInt("R3", recs[2]);
        PlayerPrefs.SetInt("R4", recs[3]);
        PlayerPrefs.SetInt("R5", recs[4]);
    }

    private void ResultRound()
    {
        if (Int32.Parse(_config.pathToCard) == 36)
        {
            if (cardsInGame[cardsInGame.Count - 1].value == 6 && cardsInGame[cardsInGame.Count - 2].value == 14)
                AddWins(cardsInGame[cardsInGame.Count - 1].type);
            else if (cardsInGame[cardsInGame.Count - 2].value == 6 && cardsInGame[cardsInGame.Count - 1].value == 14)
                AddWins(cardsInGame[cardsInGame.Count - 2].type);
            else
            {
                cardsInGame = cardsInGame.OrderBy(i => i.value).ToList();
                AddWins(cardsInGame[cardsInGame.Count - 1].type); 
            }
        }
        else if (Int32.Parse(_config.pathToCard) == 52)
        {
            if (cardsInGame[cardsInGame.Count - 1].value == 2 && cardsInGame[cardsInGame.Count - 2].value == 14)
                AddWins(cardsInGame[cardsInGame.Count - 1].type);
            else if (cardsInGame[cardsInGame.Count - 2].value == 2 && cardsInGame[cardsInGame.Count - 1].value == 14)
                AddWins(cardsInGame[cardsInGame.Count - 2].type);
            else
            {
                cardsInGame = cardsInGame.OrderBy(i => i.value).ToList();
                AddWins(cardsInGame[cardsInGame.Count - 1].type); 
            }
        }
    }

    private void AddWins(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Bot:
                AudioManager.getInstance().PlayAudio(_loseClip);
                _bot.WinCount++;
                break;
            case PlayerType.Player:
                AudioManager.getInstance().PlayAudio(_winClip);
                _player.WinCount++;
                break;
        }
    }

    private void UpdateUI()
    {
        _bot.winCountText.text = $"OPPONENT: {_bot.WinCount}";
        _player.winCountText.text = $"YOU: {_player.WinCount}";
        _moveCountText.text = "MOVE #" + (int) _moveCount;
    }
    
    public void GenerateCards(IControllable player)
    {
        var sprites = Resources.LoadAll<Sprite>(_config.pathToCard).ToList();
        sprites = ShuffledCards(sprites);

        var currentPos = player.configPlayer.startPos.position;
        for (var i = 0; i < sprites.Count / 2; i++)
        {
            var obj = Instantiate(_prefabCard, currentPos, Quaternion.identity);
            obj.transform.parent = player.transform.Find("Cards");
            obj.transform.localScale = Vector3.one;
            currentPos = new Vector3(currentPos.x - 0.005f, currentPos.y + 0.005f, currentPos.z);
            
            obj.GetComponent<Image>().sprite = sprites[i];
            var dec = Regex.Replace(sprites[i].name, "[^0-9.]", "");
            obj.GetComponent<Card>().value = Int32.Parse(dec);
            obj.GetComponent<Card>().type = player.type;
        }
    }

    private List<Sprite> ShuffledCards(List<Sprite> list)
    {
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            var k = Random.Range(0, n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }

        return list;
    }

    [Serializable]
    public struct ConfigPlayer
    {
        public Transform startPos;
        public Transform gamePos;
    }

    public enum PlayerType
    {
        Player,
        Bot
    }
}
