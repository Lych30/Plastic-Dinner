using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public int Score = 0;
    public int Multiplier = 1;
    public TextMeshProUGUI TextScore;

    // Start is called before the first frame update
    void Start()
    {
        TextScore.text = Score.ToString();
    }

    void UpdateScore()
    {
        TextScore.text = Score.ToString();
    }

    public void AddScore(float Addscore)
    {
        Score += (int)(Addscore * Multiplier);
        UpdateScore();
    }
}
