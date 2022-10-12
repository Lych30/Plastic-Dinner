using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject menu;

    public bool isMenu = true;
    public bool isGameOver = false;
    public bool isInGame = false;
    public bool canReplay = false;
    public float secondsBeforeCanReplay = 2.0f;
    public float secondsBeforePlay = 1.0f;

    private void Awake()
    {
        isMenu = true;
        isGameOver = false;
        canReplay = false;
        isInGame = false;
        menu.SetActive(true);
        gameOver.SetActive(false);
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
        isMenu = false;
        menu.SetActive(false);
        StartCoroutine(PlayDelay());
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOver.SetActive(true);
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
        isInGame = true;
    }
}
