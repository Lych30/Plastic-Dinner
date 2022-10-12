using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject gameOver;
    public CommandSystem commandSystem;

    public bool isGameOver = false;
    public bool canReplay = false;
    public float secondsBeforeCanReplay = 2.0f;

    private void Awake()
    {
        gameOver.SetActive(false);
        isGameOver = false;
        canReplay = false;
    }

    void Update()
    {
        if(isGameOver && Input.GetKeyDown(KeyCode.JoystickButton0) && canReplay)
        {
            commandSystem.isInGame = false;
            SceneManager.LoadScene("Game");
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOver.SetActive(true);

    }

    IEnumerator ReplayDelay()
    {
        yield return new WaitForSeconds(secondsBeforeCanReplay);
        canReplay = true;
    }
}
