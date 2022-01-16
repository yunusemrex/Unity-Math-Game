using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{

    private int totalScore;
    private int scoreIncrease;

    [SerializeField]
    private Text scoreText;
    void Start()
    {
        scoreText.text = totalScore.ToString();
    }

    public void AddPoýnt(string difficultyLevel)
    {
        switch (difficultyLevel)
        {
            case "easy":
                scoreIncrease = 5;
                break;
            case "medium":
                scoreIncrease = 10;
                break;
            case "hard":
                scoreIncrease = 15;
                break;


         
        }

        totalScore += scoreIncrease;

        scoreText.text = totalScore.ToString();
    }

}
