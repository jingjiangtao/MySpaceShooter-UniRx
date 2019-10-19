using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class WeaponController : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float delay;

    void Start()
    {
        Observable.Timer(dueTime: TimeSpan.FromSeconds(delay),
            period: TimeSpan.FromSeconds(fireRate))
            .Subscribe(_ =>
            {
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                GetComponent<AudioSource>().Play();
            }).AddTo(this);
    }
}
