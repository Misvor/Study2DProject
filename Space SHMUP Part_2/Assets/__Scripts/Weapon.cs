using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
	none,
	blaster,
	spread,
	phaser,
	heavyMissle,
	seekerMissle,
	shield
}

[System.Serializable]
public class WeaponDefinition
{
	public WeaponType type = WeaponType.none;

	public string letter;

	public Color color = Color.white;
	public GameObject projectilePrefab;
	public Color projectileColor = Color.white;
	public float damage = 0;
	public float damageOverTime = 0;

	public float delayBetweenShots = 0;
	public float velocity = 20;
}
public class Weapon : MonoBehaviour
{
	static public Transform PROJECTILE_ANCHOR;

	[Header("Set Dynamically")]
	[SerializeField]
	private WeaponType _type = WeaponType.none;
	public WeaponDefinition weaponDef;
	public GameObject collar;
	public float lastShotTime;
	private Renderer collarRend;
	private float shootingAngle = 0;


	private void Start()
	{
		collar = transform.Find("Collar").gameObject;

		collarRend = collar.GetComponent<Renderer>();

		SetType(_type);

		if (PROJECTILE_ANCHOR == null)
		{
			var gameObject = new GameObject("_ProjectileAnchor");
			PROJECTILE_ANCHOR = gameObject.transform;
		}

		var rootGO = transform.root.gameObject;
		if (rootGO.GetComponent<Hero>() != null)
		{
			rootGO.GetComponent<Hero>().fireDelegate += Fire;
			

		}
	}
	public void SetType(WeaponType type)
	{
		_type = type;

		if (type == WeaponType.none)
		{
			this.gameObject.SetActive(false);
			return;
		}
		else
		{
			this.gameObject.SetActive(true);
		}
		weaponDef = Main.GetWeaponDefinition(_type);
		collarRend.material.color = weaponDef.color;
		lastShotTime = 0;
	}

	public void Fire()
	{
		if (!this.gameObject.activeInHierarchy)
		{
			return;
		}

		if (Time.time - lastShotTime < weaponDef.delayBetweenShots)
		{
			return;
		}

		//Projectile p;

		if(gameObject.GetComponent<Hero>() != null)
			shootingAngle = gameObject.GetComponent<Hero>().shootingAngle;

		var shootingDir = Vector3.up;
		shootingDir.x += shootingAngle;

		var vel = shootingDir * weaponDef.velocity;
		if (transform.up.y < 0)
		{
			vel.y = -vel.y;
		}

		switch (type)
		{
			case WeaponType.blaster:
				BlasterShot(vel);
				break;

			case WeaponType.spread:
				ScatterShot(vel);
				break;

			case WeaponType.phaser:
				PhaserShot(vel);
				break;

			case WeaponType.heavyMissle:
				HeavyMissle(vel);
				break;

			case WeaponType.seekerMissle:
				SeekerMissle(vel);
				break;

		}
	}

	private void BlasterShot(Vector3 vel)
	{
		Projectile p;
		p = MakeProjectile();
		p.rigid.velocity = vel;
	}

	private void ScatterShot(Vector3 vel)
	{
		Projectile p;
		p = MakeProjectile();
		p.rigid.velocity = vel;
		p = MakeProjectile();
		p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
		p.rigid.velocity = p.transform.rotation * vel;
		p = MakeProjectile();
		p.transform.rotation = Quaternion.AngleAxis(10, Vector3.forward);
		p.rigid.velocity = p.transform.rotation * vel;
	}

	private void PhaserShot(Vector3 vel) 
	{
		PhaserShot p;
		p = (PhaserShot)MakeProjectile();


		p.rigid.velocity = vel;
	}

	private void HeavyMissle(Vector3 vel)
	{

	}

	private void SeekerMissle(Vector3 vel)
	{
		SeekerMissle p;
		p = (SeekerMissle)MakeProjectile();
		p.rigid.velocity = vel;
	}


	public WeaponType type
	{
		get { return _type; }
		set { SetType(value); }
	}

	public Projectile MakeProjectile()
	{
		var missle = Instantiate<GameObject>(weaponDef.projectilePrefab);

		if (transform.parent.gameObject.tag == "Hero")
		{
			missle.tag = "Projectile_Hero";
			missle.layer = LayerMask.NameToLayer("Projectile_Hero");
		}
		else
		{
			missle.tag = "Projectile_Enemy";
			missle.layer = LayerMask.NameToLayer("Projectile_Enemy");
		}

		missle.transform.position = collar.transform.position;
		missle.transform.SetParent(PROJECTILE_ANCHOR, true);

		var p = missle.GetComponent<Projectile>();
		p.type = type;
		lastShotTime = Time.time;
		return p;
	}


}
