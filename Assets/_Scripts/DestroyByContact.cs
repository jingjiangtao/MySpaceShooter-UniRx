using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;

	private GameController gameController;

	private void Start()
	{
		GameObject go = GameObject.FindWithTag("GameController");
		if(go != null)
		{
			gameController = go.GetComponent<GameController>();
		}
		else
		{
			Debug.LogError("Can't find GameObject named GameController");
		}
		if(gameController == null)
		{
			Debug.LogError("Can't find GameController.cs script");
		}
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Boundary")
		{
			return;
		}
		gameController.AddScore(scoreValue);
		Instantiate(explosion, transform.position, transform.rotation);
		if(other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver();
		}

		Destroy(other.gameObject);
		Destroy(gameObject);
	}

}

