using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
public class MenuManager : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject menu;
    public GameObject playerUI;
    public GameObject tutorial;
    public TMP_Text scoreText;
    public ScoreSystem scoreSystem;

    public bool isMenu = true;
    public bool isTutorial = false;
    public bool isGameOver = false;
    public bool isInGame = false;
    public bool canReplay = false;
    public float secondsBeforeCanReplay = 2.0f;
    public float secondsBeforePlay = 2.0f;

    Camera cam;
    private void Awake()
    {
        cam = Camera.main;
        
        isMenu = true;
        isGameOver = false;
        canReplay = false;
        isInGame = false;
        menu.SetActive(true);
        gameOver.SetActive(false);
        playerUI.SetActive(false);
        tutorial.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
        {
            if(isMenu)
            {
                Play();
            }
            else if(isTutorial)
            {
                tutorial.SetActive(false);
                isTutorial = false;
                isInGame = true;
                playerUI.SetActive(true);
            }
            else if(isGameOver && canReplay)
            {
                isInGame = false;
                SceneManager.LoadScene("Game");
            }
            
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Play()
    {
        cam.GetComponent<Animator>().SetTrigger("CamTrigger");
        //SoundManager.Instance.PlaySound("BackgroundKitchen");
        //SoundManager.Instance.PlaySound("BackgroundRoom");
        StartCoroutine(PlayDelay());        
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOver.SetActive(true);
        playerUI.SetActive(false);
        //SoundManager.Instance.PlaySound("HappyEndgame");
        //SoundManager.Instance.StopSound("BackgroundKitchen"); 
        //SoundManager.Instance.StopSound("BackgroundRoom");
        //SoundManager.Instance.StopSound("Clock");
        scoreText.text = scoreSystem.Score.ToString();
        StartCoroutine(ReplayDelay());
    }

    IEnumerator ReplayDelay()
    {
        yield return new WaitForSeconds(secondsBeforeCanReplay);
        canReplay = true;
    }

    IEnumerator PlayDelay()
    {
        yield return new WaitForSeconds(secondsBeforePlay);
        isMenu = false;
        menu.SetActive(false);
        isTutorial = true;
        tutorial.SetActive(true);
    }
}
