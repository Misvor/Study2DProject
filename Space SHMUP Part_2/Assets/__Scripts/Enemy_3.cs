using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Set in Inspector: Enemy_3")]
    public float lifeTime = 5;

    [Header("Set Dynamically: Enemy_3")]
    public Vector3[] points;
    public float birthtime;

	private void Start()
	{
		points = new Vector3[3];

        points[0] = pos;

        float xMin = -bounds.camWidth + bounds.radius;

        float xMax = bounds.camWidth - bounds.radius;

        var v = Vector3.zero;

        v.x = Random.Range(xMin, xMax);
        v.y = -bounds.camHeight * Random.Range(2, 2.75f);
        points[1] = v;

        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        birthtime = Time.time;

	}
	public override void Move()
	{
        float u = (Time.time - birthtime) / (lifeTime * 2);

        if(u > 1)
		{
            Destroy(this.gameObject);
            return;
		}

        Vector3 p01, p12;

        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1-u)* points[0] + u* points[1];
        p12 = (1-u)* points[1] + u* points[2];

        pos = (1-u)*p01 + u* p12;

	}

}
