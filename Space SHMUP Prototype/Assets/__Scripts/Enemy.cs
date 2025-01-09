using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	[Header("Set in Inspector: Enemy")]
	public float speed = 10f;
	public float fireRate = 0.3f;
	public float health = 10;
	public int reward = 100;

	public Vector3 pos
	{
		get { return this.transform.position; }
		set { this.transform.position = value; }
	}

	private BoundsCheck bounds;

	private void Awake()
	{
		bounds = GetComponent<BoundsCheck>();
	}

	void Update()
	{
		Move();

		if (bounds != null && bounds.offBottom)
		{
			Destroy(gameObject);
		}
	}

	public virtual void Move()
	{
		var tempPosition = pos;
		tempPosition.y -= speed * Time.deltaTime;
		pos = tempPosition;
	}

	private void OnCollisionEnter(Collision collision)
	{
		var otherGO = collision.gameObject;

		if (otherGO.tag == "Projectile_Hero")
		{
			Destroy(otherGO);
			Destroy(gameObject);
		}
		else
		{
			print($"Enemy hit by non-projectileHero: {otherGO.name}");
		}
	}
}
