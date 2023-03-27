using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;

    private SceneLoader()
    {}

    public static SceneLoader getInstance()
    {
        if (instance == null) instance = new SceneLoader();
        return instance;
    }
    
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
