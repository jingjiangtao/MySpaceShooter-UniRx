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

    private void Init()
    {
        GameObject go = GameObject.FindWithTag("GameController");
        if (go != null)
        {
            gameController = go.GetComponent<GameController>(); // 初始化GameController脚本
        }
        else
        {
            Debug.LogError("Can't find GameObject named GameController");
        }
        if (gameController == null)
        {
            Debug.LogError("Can't find GameController.cs script");
        }
    }

    private void Start()
    {
        Init();

        // 子弹碰撞事件
        var boltEnterStream = this.OnTriggerEnterAsObservable()
            .Where(other => other.CompareTag("Bolt"));

        // 飞船碰撞事件
        var playEnterStream = this.OnTriggerEnterAsObservable()
            .Where(other => other.CompareTag("Player"))
            .Do(other =>
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
            });
        
        // 合并子弹碰撞事件和飞船碰撞事件
        Observable.Merge(boltEnterStream, playEnterStream)
            .Subscribe(other =>
            {
                // 实例化爆炸粒子，计算分数
                gameController.AddScore(scoreValue);
                Instantiate(explosion, transform.position, transform.rotation);

                // 销毁当前小行星和碰撞的物体
                Destroy(other.gameObject);
                Destroy(gameObject);
            }).AddTo(this);
    }
}

