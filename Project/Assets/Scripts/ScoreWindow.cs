using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreWindow : MonoBehaviour {

    private TMP_Text scoreText;
    public TMP_Text highScoreText;

    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();

        int highscore = Score.GetHighScore();
        highScoreText.GetComponent<TMP_Text>().text = "Highscore: " + highscore.ToString();
    }

    private void Update()
    {
        scoreText.text = "Score: " + GameHandler.Score.ToString();
    }
}
