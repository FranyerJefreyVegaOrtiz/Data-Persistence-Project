using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text Score1;
    public Text Faltanombre;
    public string namePlayer;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        MainManager.Instance.LoadString();
        BestScore();
    }

    //private void Start()
    //{
    //    if (MainManager.Instance != null)
    //    {
    //        Debug.Log("Player Name: " + MainManager.Instance.MMnamePlayer);
    //    }
    //    else
    //    {
    //        Debug.LogError("MainManager.Instance is null. Make sure MainManager is present in the scene.");
    //    }
    //}
    public void StartGame(string scene)
    {

        if (namePlayer == "")
        {
            Faltanombre.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReadStringInput(string name)
    {
        namePlayer = name;
    }
    public void BestScore()
    {
        Score1.text = MainManager.Instance.MMnamePlayer + " : " + MainManager.Instance.MMMaxScore;
    }
}
