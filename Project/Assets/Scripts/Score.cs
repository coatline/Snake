using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score{

	public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }

    public static bool TrySetNewHighScore(int score)
    {
        int highscore = GetHighScore();
        if(score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }
}
