using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject homePanel;
    [SerializeField] Button retryButton;
    public static Action ResetScore;

    private void OnEnable()
    {
        BallController.OnDeath += EnableGameOverPanel;
    }

    private void Awake()
    {
        retryButton.onClick.AddListener(() =>
        {
            ResetScore?.Invoke();
            gameOverPanel.SetActive(false);
        });
    }

    private void EnableGameOverPanel() 
    {
        gameOverPanel.SetActive(true);
    }


    private void OnDestroy()
    {
        BallController.OnDeath -= EnableGameOverPanel;
        retryButton.onClick.RemoveAllListeners();
    }
}
