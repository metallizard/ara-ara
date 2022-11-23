using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOverUI;

    [SerializeField]
    private GameObject _winUI;

    private void Awake()
    {
        GameContext.OnGameOver += GameContext_OnGameOver;
        GameContext.OnGameWin += GameContext_OnGameWin;

        _gameOverUI.SetActive(false);
    }

    private void GameContext_OnGameWin()
    {
        _winUI.SetActive(true);
    }

    private void GameContext_OnGameOver()
    {
        _gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        GameContext.OnGameOver -= GameContext_OnGameOver;
    }
}
