using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // Reference to the UI Text component
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
    }

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Update the score text
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
