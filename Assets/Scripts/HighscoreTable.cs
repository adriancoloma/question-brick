using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        var highscoreLoaded = LoadHighscore();
        highscoreEntryList = highscoreLoaded == null ? new List<HighscoreEntry>() : highscoreLoaded;
        Debug.Log("se inicializo");
    }

    private void RenderHighscoreTable()
    {
        highscoreEntryList.Sort((a, b) => -a.score.CompareTo(b.score));
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHightscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHightscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;

        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;
        entryTransform.Find("scoreText").GetComponent<Text>().text = highscoreEntry.score.ToString();
        entryTransform.Find("nameText").GetComponent<Text>().text = highscoreEntry.name;

        transformList.Add(entryTransform);
    }

    public void RegisterUserScore(string nick)
    {
        this.gameObject.SetActive(true);
        AddScore(nick, ScoreController.score);
        GameObject.Find("FinalMessage").SetActive(false);
        RenderHighscoreTable();
        
    }
    public void AddScore(string nick, int score)
    {
        highscoreEntryList.Add(new HighscoreEntry {score = score, name = nick});
        Debug.Log("nick " + nick);
        SaveHighscoreList();
    }

    private void SaveHighscoreList()
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(highscoreEntryList);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        Debug.Log("highscore guardado " + json + string.Join(", ", highscoreEntryList));
    }

    private List<HighscoreEntry> LoadHighscore()
    {
        var tieneKey = PlayerPrefs.HasKey("highscoreTable");
        Debug.Log("tiene " + tieneKey);
        if (tieneKey)
        {
            Debug.Log("JSON " + PlayerPrefs.GetString("highscoreTable"));
        }
        var highscoreList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HighscoreEntry>>(PlayerPrefs.GetString("highscoreTable"));

        return highscoreList;

    }
    [Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }


}
