using UnityEngine;

public class ScoreIncrement : MonoBehaviour
{
    private int currentScore;
    [SerializeField] TMPro.TMP_Text scoreText;
    [SerializeField] TMPro.TMP_Text gameoverText;
    private void OnEnable()
    {
        BallController.OnTileHit += IncrementScore;
        UIManager.ResetScore += ResetScore;
        BallController.OnDeath += OnGameOver;
    }

    private void IncrementScore() 
    {
        currentScore++;
        scoreText.text = currentScore.ToString(); 
    }
    private void OnGameOver() 
    {
        gameoverText.text = scoreText.text;
    }

    private void ResetScore() 
    {
        currentScore = 0;
        scoreText.text = currentScore.ToString();
        gameoverText.text = scoreText.text;
    }

    private void OnDestroy()
    {
        BallController.OnTileHit -= IncrementScore;
        UIManager.ResetScore -= ResetScore;
        BallController.OnDeath -= OnGameOver;
    }
}
