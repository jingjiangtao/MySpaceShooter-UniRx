using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public GameObject hazard;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait = 2.0f;
	public Text scoreText;
	public Text gameOverText;
	public Text restartText;

	private Vector3 spawnPosition = Vector3.zero;
	private Quaternion spawnRotation;
	private int score;
	private bool gameOver;
	private bool restart;

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(startWait);
		while (true)
		{
			if (gameOver)
			{				
				break;
			}
			for (int i = 0; i < hazardCount; i++)
			{
				spawnPosition.x = Random.Range(-spawnValues.x, spawnValues.x);
				spawnPosition.z = spawnValues.z;
				spawnRotation = Quaternion.identity;
				Instantiate(hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);
		}
	}

	void Start()
	{
		score = 0;
		UpdateScore();
		gameOverText.text = string.Empty;
		gameOver = false;
		restartText.text = string.Empty;
		restart = false;
		StartCoroutine(SpawnWaves());
	}

	private void Update()
	{
		if (restart)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene(0);
			}
		}
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
