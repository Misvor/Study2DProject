using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
	[Header("Set in Inspector")]

	public float sinEccentricity = 0.6f;
	public float lifeTime = 10;

	[Header("Set Dynamically")]
	public Vector3 p0;
	public Vector3 p1;
	public float birthTime;

	private float toBottomDistance;

	private void Start()
	{
		p0 = Vector3.zero;

		p0.x = -bounds.camWidth - bounds.radius;

		p0.y = Random.Range(-bounds.camHeight, bounds.camHeight);

		p1 = Vector3.zero;
		p1.x = bounds.camWidth + bounds.radius;
		p1.y = Random.Range(-bounds.camHeight, bounds.camHeight);

		if(Random.value > 0.5f)
		{
			p0.x *= -1;
			p1.x *= -1;
		}
		birthTime = Time.time;
		toBottomDistance = bounds.camHeight - p1.y;
	}

	public override void Move()
	{
		var u = (Time.time - birthTime) / lifeTime;

		if(u > 1)
		{
			Destroy(this.gameObject);
			return;
		}
		u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));

		p1.y = -bounds.camHeight * u;
		pos = (1 - u) * p0 + u * p1;
		

		//base.Move();
	}
}
