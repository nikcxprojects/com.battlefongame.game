using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConfigController : MonoBehaviour
{

    [SerializeField] private GameConfig _config;

    [SerializeField] private Toggle _toggleBeat;
    [SerializeField] private Toggle _toggleNotBeat;
    [SerializeField] private Toggle _togglePath36;
    [SerializeField] private Toggle _togglePath54;

    void Start()
    {
        Application.targetFrameRate = 90;
        
        SetPath(PlayerPrefs.GetString("Path", "36"));
        SetBeatingAce(PlayerPrefs.GetInt("Beat", 0) == 1);

        if (PlayerPrefs.GetString("Path").Contains("36"))
            _togglePath36.isOn = true;
        else _togglePath54.isOn = true;
        
        if (PlayerPrefs.GetInt("Beat") == 1)
            _toggleBeat.isOn = true;
        else _toggleNotBeat.isOn = true;
    }
    
    public void SetPath(string path)
    {
        _config.pathToCard = path;
        PlayerPrefs.SetString("Path", path);
    }

    public void SetBeatingAce(bool beat)
    {
        _config.beatAce = beat;
        var b = beat ? 1 : 0;
        PlayerPrefs.SetInt("Beat", b);
    }
}
