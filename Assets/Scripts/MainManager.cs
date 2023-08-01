using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Hardware;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public Text BestScoreText;

    public string MMnamePlayer;
    public int MMMaxScore;

    int MaxScoreText;

    public Text ScoreText;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private void Awake()
    {
        Instance = this;
        LoadString();
        BestScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        MaxScoreText = m_Points;
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (MaxScoreText > MMMaxScore)
        {
            SaveString();
        }
    }

    public void BestScore()
    {
        BestScoreText.text = "Best Score: " + MMnamePlayer + " : " + MMMaxScore;
    }

    [Serializable]
    class SaveData
    {
        public string namePlayer;
        public int MaxScoreText;
    }
    public void SaveString()
    {
        SaveData data = new SaveData();
        data.namePlayer = GameManager.Instance.namePlayer;
        data.MaxScoreText = MaxScoreText;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    public void LoadString()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            MMnamePlayer = data.namePlayer;
            MMMaxScore = data.MaxScoreText;
        }
    }
}
