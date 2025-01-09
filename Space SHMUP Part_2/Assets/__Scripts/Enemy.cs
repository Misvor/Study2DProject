using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	[Header("Set in Inspector: Enemy")]
	public float speed = 10f;
	public float fireRate = 0.3f;
	public float health = 10;
	public int reward = 100;
	public float showDamageDuration = 0.1f;
	public float powerUpDropChance = 1f;

	[Header("Set Dynamically: Enemy")]
	public Color[] originalColors;
	public Material[] materials;
	public bool showingDamage = false;
	public float damageDoneTime;
	public bool notifiedOfDestruction = false;


	private int processedCollisions;
	private bool isHit = false;


	public Vector3 pos
	{
		get { return this.transform.position; }
		set { this.transform.position = value; }
	}

	protected BoundsCheck bounds;

	private void Awake()
	{
		bounds = GetComponent<BoundsCheck>();

		materials = Utils.GetAllMaterials(gameObject);
		originalColors = materials.Select(x => x.color).ToArray();
	}

	void Update()
	{
		Move();

		if (showingDamage && Time.time > damageDoneTime)
		{
			UnShowDamage();
		}

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

	void ShowDamage()
	{
		foreach (var material in materials)
		{
			material.color = Color.red;
		}
		showingDamage = true;
		damageDoneTime = Time.time + showDamageDuration;
	}
	void UnShowDamage()
	{
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].color = originalColors[i];
		}
		showingDamage = false;

	}


	private void OnCollisionEnter(Collision collision)
	{
		var otherGO = collision.gameObject;



		switch (otherGO.tag)
		{
			case "Projectile_Hero":
				var p = otherGO.GetComponent<Projectile>();

				if (collision.contactCount > 1)
				{
					if (processedCollisions >= 1)
					{
						processedCollisions = 0;
						Destroy(otherGO);
						break;
					}
					processedCollisions++;
				}


				if (!bounds.isOnScreen)
				{
					Destroy(otherGO);
					break;
				}

				health -= Main.GetWeaponDefinition(p.type).damage;
				ShowDamage();
				if (health <= 0)
				{
					if (!notifiedOfDestruction)
					{
						Main.Self.ShipDestroyed(this);
						notifiedOfDestruction = true;
					}
					Destroy(this.gameObject);
				}

				Destroy(otherGO);

				break;
			default:
				print($"Enemy hit by non-projectileHero: {otherGO.name}");
				break;
		}


	}

}
