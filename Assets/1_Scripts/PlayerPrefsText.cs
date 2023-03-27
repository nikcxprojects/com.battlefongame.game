using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsText : MonoBehaviour
{

    [SerializeField] private string playerPrefsname;
    
    void Start()
    {
        GetComponent<Text>().text += $"Won by {PlayerPrefs.GetInt(playerPrefsname)} moves";
    }

}
