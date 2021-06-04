using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTMP;
    [SerializeField] GameObject scoreGameObject;
    public static int currentScore = 0;
    int scoreTextVariable = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0;
        scoreTMP.text = "Score: " + scoreTextVariable.ToString();
        scoreGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreTextVariable != currentScore) {
            scoreTextVariable = currentScore;
            scoreTMP.text = "Score: " + scoreTextVariable.ToString();
        }
    }
}
