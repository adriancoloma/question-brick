using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Puntaje " + ScoreController.score);
        GetComponent<Text>().text = ScoreController.score.ToString();
    }


}
