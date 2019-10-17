using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float fireRate = 0.25f;
    public GameObject shot;
    public Transform shotSpawn;

    public float speed = 5.0f;
    public Boundary boundary;
    public float tilt = 4.0f;

    private void Start()
    {
        // 监听Fire1按钮事件以发射子弹
        Observable.EveryUpdate()
            .Where(_ => Input.GetButton("Fire1"))
            .ThrottleFirst(TimeSpan.FromSeconds(fireRate))
            .Subscribe(_ =>
            {
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                GetComponent<AudioSource>().Play();
            }).AddTo(this);

        // 监听水平按键并获取值
        var moveHorizontalStream = Observable.EveryFixedUpdate()
            .Select(_ => Input.GetAxis("Horizontal"));
        // 监听垂直按键并获取值
        var moveVerticalStream = Observable.EveryFixedUpdate()
            .Select(_ => Input.GetAxis("Vertical"));

        // 合并水平按键和垂直按键
        var movementStream = moveHorizontalStream.CombineLatest(moveVerticalStream,
            (horizontal, vertical) => new Vector3(horizontal, 0.0f, vertical));
        
        movementStream.Subscribe(movement =>
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = movement * speed;
                rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
                rb.position = new Vector3(
                    Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                    0.0f,
                    Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));
            }
        }).AddTo(this);

    }
}
