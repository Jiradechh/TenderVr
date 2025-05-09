using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    private int score = 0;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddScore(1);
        }
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

    public void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public int GetScore() => score;
}
