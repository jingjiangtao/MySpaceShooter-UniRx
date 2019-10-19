using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Random = UnityEngine.Random;

public class EvasiveManeuver : MonoBehaviour
{
    public Boundary boundary;
    public float tilt;
    public float dodge;
    public float smoothing;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;

    private float currentSpeed;
    private float targetManeuver;

    void Start()
    {
        currentSpeed = GetComponent<Rigidbody>().velocity.z;

        Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                float newManeuver = Mathf.MoveTowards(GetComponent<Rigidbody>().velocity.x, targetManeuver, smoothing * Time.fixedDeltaTime);
                GetComponent<Rigidbody>().velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
                GetComponent<Rigidbody>().position = new Vector3
                (
                    Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                    0.0f,
                    Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
                );

                GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);
            }).AddTo(this);

        Observable.Timer(dueTime: TimeSpan.FromSeconds(Random.Range(startWait.x, startWait.y)),
            period: TimeSpan.FromSeconds(Random.Range(maneuverWait.x, maneuverWait.y)))
            .Do(_ => targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x))
            .Delay(TimeSpan.FromSeconds(Random.Range(maneuverTime.x, maneuverTime.y)))
            .Do(_ => targetManeuver = 0)
            .Subscribe()
            .AddTo(this);
            
    }
}
