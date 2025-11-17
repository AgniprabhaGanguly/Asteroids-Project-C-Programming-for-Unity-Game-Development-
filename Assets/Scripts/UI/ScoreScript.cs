using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript Instance;
    public TextMeshProUGUI scoreText;

    private int score = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"SCORE: {score}";
    }

    public float GetScore
    {
        get { return score; }
    }
}
