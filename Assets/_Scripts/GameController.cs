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
    private IntReactiveProperty score = new IntReactiveProperty(0);
    private BoolReactiveProperty gameOver = new BoolReactiveProperty(false);
    private BoolReactiveProperty restart = new BoolReactiveProperty(false);

    void Start()
    {
        // 绑定分数、游戏结束、重新开始的text
        score.SubscribeToText(scoreText, s => $"得分: {score}");
        gameOverText.text = string.Empty;        
        restartText.text = string.Empty;
        gameOver.Where(x => x).SubscribeToText(gameOverText, _ => "游戏结束");
        restart.Where(x => x).SubscribeToText(restartText, _ => "按R键重新开始");

        // 生成小行星
        var spawnStream = Observable.Interval(TimeSpan.FromSeconds(spawnWait))
            .Where(_ => !gameOver.Value)
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
            .Where(_ => restart.Value);
        restartStream.Subscribe(_ => SceneManager.LoadScene(0));
    }


    public void AddScore(int newScoreValue)
    {
        score.Value += newScoreValue;
    }

    public void GameOver()
    {
        gameOver.Value = true;
        restart.Value = true;
    }
}
