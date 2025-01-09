using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy_4 : Enemy
{
	[Header("set in Inspector: Enemy_4")]
	public Part[] parts;

	private Vector3 start, dest;
	private float timeStart;
	private float duration = 4;

	private void Start()
	{
		start = dest = pos;
		InitMovement();


		// Cache every part's gameObject and material
		Transform t;

		foreach(var part in parts)
		{
			t = transform.Find(part.name);
			if(t != null)
			{
				part.go = t.gameObject;
				part.mat = part.go.GetComponent<Renderer>().material;
			}
		}
	}

	void InitMovement()
	{
		start = dest;

		var widMinRad = bounds.camWidth - bounds.radius;
		var hgtMinRad = bounds.camHeight - bounds.radius;

		dest.x = Random.Range(-widMinRad, widMinRad);
		dest.y = Random.Range(-hgtMinRad, hgtMinRad);

		timeStart = Time.time;

	}

	public override void Move()
	{
		var howOld = (Time.time - timeStart) / duration;

		if(howOld > 1)
		{
			InitMovement();
			howOld = 0;
		}

		howOld = 1 - Mathf.Pow(1 - howOld, 2);
		pos = (1 - howOld) * start + howOld * dest;

	}

	Part FindPart(string name) => parts.FirstOrDefault(x => x.name == name);
	Part FindPart(GameObject go) => parts.FirstOrDefault(x => x.go == go);
	bool Destroyed(GameObject go) => Destroyed(FindPart(go));
	bool Destroyed(string name) => Destroyed(FindPart(name));
	bool Destroyed(Part prt) => prt == null ? true : prt.health <= 0;

	void ShowLocalizedDamage(Material m)
	{
		m.color = Color.red;
		damageDoneTime = Time.time + showDamageDuration;
		showingDamage = true;
	}
	private void OnCollisionEnter(Collision collision)
	{
		var other = collision.gameObject;
		switch (other.tag)
		{
			case "Projectile_Hero":
				var proj = other.GetComponent<Projectile>();
				if (!bounds.isOnScreen)
				{
					// can't damage off screen
					Destroy(other);
					break;
				}
				var goHit = collision.contacts[0].thisCollider.gameObject;
				var partHit = FindPart(goHit);
				if(partHit == null)
				{
					goHit = collision.contacts[0].otherCollider.gameObject;
					partHit = FindPart(goHit);
				}

				if (partHit.protectedBy != null)
				{
					if(partHit.protectedBy.Any(x=> !Destroyed(x)))
					{
						Destroy(other);
						return;
					}
				}

				partHit.health -= Main.GetWeaponDefinition(proj.type).damage;
				ShowLocalizedDamage(partHit.mat);
				if (partHit.health <= 0)
				{
					partHit.go.SetActive(false);
				}

				bool fullyDestroyed = true;
				if(parts.Any(x=> !Destroyed(x)))
				{
					fullyDestroyed = false;
				}
				if (fullyDestroyed)
				{
					Main.Self.ShipDestroyed(this);
					Destroy(this.gameObject);
				}
				Destroy(other);
				break;

		}
	}


}

[System.Serializable]
public class Part
{
	public string name;
	public float health;
	public string[] protectedBy;

	[HideInInspector]
	public GameObject go;
	[HideInInspector]
	public Material mat;

}
