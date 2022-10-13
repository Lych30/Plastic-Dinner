using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject menu;
    public GameObject playerUI;

    public bool isMenu = true;
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
    }

    void Update()
    {
        if(isMenu && Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Play();
        }
        else if(isGameOver && Input.GetKeyDown(KeyCode.JoystickButton0) && canReplay)
        {
            isInGame = false;
            SceneManager.LoadScene("Game");
        }
    }

    public void Play()
    {
        cam.GetComponent<Animator>().SetTrigger("CamTrigger");
        StartCoroutine(PlayDelay());        
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOver.SetActive(true);
        playerUI.SetActive(false);
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
        isInGame = true;
        playerUI.SetActive(true);
    }
}
