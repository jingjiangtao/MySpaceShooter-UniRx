using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;

    private GameController gameController;

    private void Start()
    {
        GameObject go = GameObject.FindWithTag("GameController");
        if (go != null)
        {
            gameController = go.GetComponent<GameController>();
        }
        else
        {
            Debug.LogError("Can't find GameObject named GameController");
        }
        if (gameController == null)
        {
            Debug.LogError("Can't find GameController.cs script");
        }

        var boltEnterStream = this.OnTriggerEnterAsObservable()
            .Where(other => other.CompareTag("Bolt"));

        var playEnterStream = this.OnTriggerEnterAsObservable()
            .Where(other => other.CompareTag("Player"))
            .Do(other =>
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
            });
        Observable.Merge(boltEnterStream, playEnterStream)
            .Subscribe(other =>
            {
                gameController.AddScore(scoreValue);
                Instantiate(explosion, transform.position, transform.rotation);

                Destroy(other.gameObject);
                Destroy(gameObject);
            }).AddTo(this);
    }
}

