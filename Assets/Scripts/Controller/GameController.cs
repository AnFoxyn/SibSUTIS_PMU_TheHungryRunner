using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    [Header("Progress Bar")]
    [SerializeField] public Slider progressSlider;
    int winningAmount;
    int progressAmount = 0;

    [Space]
    [Header("Level Complete")]
    [SerializeField] private int nextLevel;
    [SerializeField] public GameObject levelCompleteScreen;
    [SerializeField] public Text levelCompleteText;
    private string nextLevelName;

    [Space]
    [Header("Game Over")]
    [SerializeField] public GameObject gameOverScreen;
    [SerializeField] public Text ateText;

    public static event Action OnReset;

    void Start()
    {
        winningAmount = GameObject.FindGameObjectsWithTag("Fruit").Length * 2 - 2;

        progressSlider.value = 0;
        progressSlider.maxValue = winningAmount;
        Fruit.OnFruitCollect += IncreaseProgressAmount;

        gameOverScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
        gameObject.SetActive(Application.isMobilePlatform);
    }

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;

        if (progressAmount >= winningAmount)
        {
            LevelCompleteScreen();
        }
    }

    public void LevelCompleteScreen()
    {
        levelCompleteScreen.SetActive(true);
        levelCompleteText.text = "Level complete";

        Time.timeScale = 0;
    }

    public void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        ateText.text = "You have eaten " + progressAmount / 2 +
                       " fruits from " + winningAmount / 2;

        Time.timeScale = 0;
    }

    public void OnExitClick()
    {
        gameOverScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
        Time.timeScale = 1;

        SceneManager.LoadScene("Scenes/StartScene");
    }

    public void OnResetClick()
    {
        gameOverScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
        OnReset.Invoke();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnNextLevel()
    {
        gameOverScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
        Time.timeScale = 1;

        if (nextLevel < 10)
            nextLevelName = "Scenes/Levels/Level_0" + nextLevel;
        else
            nextLevelName = "Scenes/Levels/Level_" + nextLevel;

        SceneManager.LoadScene(nextLevelName);
    }
}
