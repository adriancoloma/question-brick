using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int questionBricksToDestroy;

    public GameObject mainCanvas;
    public GameObject gameOverCanvas;

    public GameObject ball;
    public Text finalScore;

    private ScoreController scoreController;
    // Start is called before the first frame update
    void Start()
    {
        scoreController = GameObject.FindObjectOfType<ScoreController>();
        Object.DontDestroyOnLoad(GameObject.Find("CorrectSound"));
        Object.DontDestroyOnLoad(GameObject.Find("IncorrectSound"));
    }

    // Update is called once per frame
    void Update()
    {
        if(questionBricksToDestroy == 0)
        {
            
            SceneManager.LoadScene(1);
            //GameOver();
        }
    }

    private void GameOver()
    {
        ball.GetComponent<BallMovement>().SetQuestion(true);
        mainCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        finalScore.text = ScoreController.score.ToString();

        //Debug.Log("finalizo el juego");
    }

    private void OnApplicationQuit()
    {
        Debug.Log("aplicacion finalizada");
        var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var gameObject in gameObjects)
        {
            if (gameObject.tag == "brick" || gameObject.tag == "questionbrick") 
                gameObject.SetActive(true);
        }

    }
}
