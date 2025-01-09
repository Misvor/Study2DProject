using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

	protected BoundsCheck bounds;
	protected Renderer rend;

	[Header("Set Dynamically")]
	public Rigidbody rigid;

	[SerializeField]
	private WeaponType _type;

	public WeaponType type { get { return _type; } set { SetType(value); } }



	protected virtual void Awake()
	{
		bounds = GetComponent<BoundsCheck>();
		rend = GetComponent<Renderer>();
		rigid = GetComponent<Rigidbody>();
	}
	
	/// <summary>
	/// Changes WeaponType and sets the color of projectile
	/// </summary>
	public void SetType( WeaponType weaponType)
	{
		_type = weaponType;
		WeaponDefinition def = Main.GetWeaponDefinition(_type);
		rend.material.color = def.projectileColor;

	}

	void Update()
	{
		if (bounds.offUp)
		{
			Destroy(gameObject);
		}
	}
}
