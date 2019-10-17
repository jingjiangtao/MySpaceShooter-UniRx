using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using System;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float waveWait = 2.0f;
    public Text scoreText;
    public Text gameOverText;
    public Text restartText;

    private Vector3 spawnPosition = Vector3.zero;
    private Quaternion spawnRotation;
    private int score;
    private bool gameOver;
    private bool restart;

    void Start()
    {
        score = 0;
        UpdateScore();
        gameOverText.text = string.Empty;
        gameOver = false;
        restartText.text = string.Empty;
        restart = false;

        // 生成小行星
        var spawnStream = Observable.Interval(TimeSpan.FromSeconds(spawnWait))
            .Where(_ => !gameOver)
            .Do(_ =>
            {
                spawnPosition.x = Random.Range(-spawnValues.x, spawnValues.x);
                spawnPosition.z = spawnValues.z;
                spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
            })
            .Take(hazardCount)
            .Delay(TimeSpan.FromSeconds(waveWait))
            .RepeatSafe();
        spawnStream.Subscribe().AddTo(this);

        // 重新开始游戏
        var restartStream = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.R))
            .Where(_ => restart);
        restartStream.Subscribe(_ => SceneManager.LoadScene(0));
    }


    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = $"得分: {score}";
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverText.text = "游戏结束";
        restartText.text = "按R键重新开始";
        restart = true;
    }
}
