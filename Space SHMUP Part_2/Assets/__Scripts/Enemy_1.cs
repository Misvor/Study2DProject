using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
	[Header("Set in Inspector")]

	public float waveFrequency = 2;
	public float waveWidth = 4;
	public float waveRotY = 45;

	private float x0;
	private float birthTime;

	private void Start()
	{
		x0 = pos.x;

		birthTime = Time.time;

	}

	public override void Move()
	{
		var tempPos = pos;
		var age = Time.time - birthTime;

		var theta = Mathf.PI * 2 * age / waveFrequency;

		var sin = Mathf.Sin(theta);
		tempPos.x = x0 + waveWidth * sin;
		pos = tempPos;

		var rot = new Vector3(0, sin * waveRotY, 0);
		this.transform.rotation = Quaternion.Euler(rot);



		base.Move();
	}
}
