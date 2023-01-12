using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    public int speed;
    public Text questionText;

    public Button option1;
    public Button option2;
    public Button option3;

    public AudioSource correctAudio;
    public AudioSource incorrectAudio;

    private Rigidbody2D body;
    private ScoreController scoreController;
    private Vector2 spawnPosition;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        moveRandomly();
        //body.velocity = Vector2.up * speed;
        scoreController = FindObjectOfType<ScoreController>();
        spawnPosition = transform.position;
        gm = Resources.FindObjectsOfTypeAll<GameManager>()[0];
    }

    private void moveRandomly()
    {
        var vector = new Vector2(Random.Range(0f, 1f), Random.value).normalized;
        body.velocity = vector * speed;
        Debug.Log(body.velocity.magnitude);
        Debug.Log("Direccion: " + body.velocity);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.tag == "questionbrick")
        {
            GameObject brick = collision.collider.gameObject;
            Debug.Log("colision con un brick");
            QuestionController questionController = collision.collider.GetComponent<QuestionController>();
            if(questionController == null)
            {
                Debug.Log("no tiene un question controller");
                return;
            }
            questionText.text = questionController.question;
            option1.GetComponentInChildren<Text>().text = questionController.option1;
            option2.GetComponentInChildren<Text>().text = questionController.option2;
            option3.GetComponentInChildren<Text>().text = questionController.option3;

            switch (questionController.correctOption)
            {
                case 1:
                    option1.onClick.RemoveAllListeners();
                    option1.onClick.AddListener(() => { HandleCorrect(); });
                    option2.onClick.RemoveAllListeners();
                    option2.onClick.AddListener(() => { HandleIncorrect(); });
                    option3.onClick.RemoveAllListeners();
                    option3.onClick.AddListener(() => { HandleIncorrect(); });
                    break;
                case 2:
                    option2.onClick.RemoveAllListeners();
                    option2.onClick.AddListener(() => { HandleCorrect(); });
                    option1.onClick.RemoveAllListeners();
                    option1.onClick.AddListener(() => { HandleIncorrect(); });
                    option3.onClick.RemoveAllListeners();
                    option3.onClick.AddListener(() => { HandleIncorrect(); });
                    break;
                case 3:
                    option3.onClick.RemoveAllListeners();
                    option3.onClick.AddListener(() => { HandleCorrect(); });
                    option2.onClick.RemoveAllListeners();
                    option2.onClick.AddListener(() => {HandleIncorrect(); });
                    option1.onClick.RemoveAllListeners();
                    option1.onClick.AddListener(() => { HandleIncorrect(); });
                    break;

            }

            Destroy(brick);
            SetQuestion(true);
            this.transform.position = spawnPosition; //Devuelve a la bola a la posicion inicial
            
        }else if(collision.collider.tag == "brick")
        {
            Destroy(collision.collider.gameObject);
        }
    }

    public void HandleCorrect()
    {
        scoreController.AddScore(); 
        SetQuestion(false); 
        gm.questionBricksToDestroy--;
        correctAudio.Play();
        Debug.Log("Respuesta correcta");
    }

    public void HandleIncorrect()
    {
        SetQuestion(false); 
        gm.questionBricksToDestroy--;
        incorrectAudio.Play();
        Debug.Log("respuesta incorrecta");
    }
    public void SetQuestion(bool active)
    {
        questionText.gameObject.SetActive(active);
        option1.gameObject.SetActive(active);
        option2.gameObject.SetActive(active);
        option3.gameObject.SetActive(active);

        this.gameObject.SetActive(!active);
        var objects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach(var gameObject in objects) //Se activa o desactiva todos los bricks y al player
        {
            if(gameObject.tag == "Player" || gameObject.tag == "questionbrick" || gameObject.tag == "brick")
            {
                gameObject.SetActive(!active);
                //Debug.Log("desactivado");
            }
        }

        if (!active)
            moveRandomly();

    }



}
