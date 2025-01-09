using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserShot : Projectile
{
    [Header("Set in Inspector")]
    public float amlpitude = 25;
    public float frequency = 10;


    private float birthTime;

	private void Start()
	{
		birthTime = Time.time;
	}

	// Update is called once per frame
	void Update()
    {
        var newPosition = transform.position;

        newPosition.x += Mathf.Cos((Time.time - birthTime) * frequency) * amlpitude * Time.deltaTime * (Time.time - birthTime);
        transform.position = newPosition;

        if (bounds.offUp)
        {
            Destroy(gameObject);
        }
    }
}
