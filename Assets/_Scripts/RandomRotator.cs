using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
	public float tumble = 10.0f;

    void Start()
    {
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }
}
