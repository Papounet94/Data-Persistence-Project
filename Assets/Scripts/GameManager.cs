using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string bestPlayerName;
    public int bestScore;
    public string playerName;
    public Text msgText;
    public InputField nameField;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        // get back previous hiscore
        GetScore();
        // Display it with the player's name
        DisplayHiScore(msgText);
        // fill in the player name in the input field
        FillInField();
    }

    public void StartMain()
    {
        playerName = nameField.text;

        if (playerName != "")
        { 
            SceneManager.LoadScene(1);
        }
        else
        {
            nameField.placeholder.GetComponent<Text>().text = "Please enter your name!";
        }
        
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public string bestPlayerName;
        public int hiScore;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.bestPlayerName = playerName;
        data.hiScore = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void GetScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestPlayerName = data.bestPlayerName;
            bestScore = data.hiScore;
        }
    }

    public void DisplayHiScore(Text msg)
    {
        msg.text = $"Best Score : {bestPlayerName} : {bestScore}";
    }

    private void FillInField()
    {
        if (bestPlayerName != "")
        {
            nameField.text = bestPlayerName;
        }
    }
}

