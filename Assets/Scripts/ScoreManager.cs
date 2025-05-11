using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private int score = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
        UpdateHighScoreUI(); 
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void RemoveScore(int amount)
    {
        score = Mathf.Max(0, score - amount);
        UpdateUI();
    }

    public int GetScore() => score;

    public void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

        public void UpdateHighScoreUI()
    {
        int high = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null)
            highScoreText.text = "High Score: " + high;
    }


    public void CheckAndSetHighScore()
    {
        int currentHigh = PlayerPrefs.GetInt("HighScore", 0);
        if (score > currentHigh)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

}
