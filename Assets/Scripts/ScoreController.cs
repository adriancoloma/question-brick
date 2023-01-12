using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static int score = 0;

    public void AddScore()
    {
        score++;
        GetComponent<Text>().text = score.ToString();
    }
}
