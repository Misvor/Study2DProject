using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hero : MonoBehaviour
{

    static public Hero Self;

    [Header("Set in Inspector")]

    public float speed = 5;
    public float acceleration = 15;
    public float rollMult = -45;
    public float pitchMult = 30;

    public float YStopMult = 1;
    public float YStartMult = 1;

    public float XStopMult = 1;
    public float XStartMult = 1;

    public float gameRestartDelay = 2f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

    public Weapon[] weapons;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private float currYSpeed = 0;
    private float currXSpeed = 0;
    public float shootingAngle = 0;
    protected BoundsCheck bounds;

    private void Start()
	{
       if(Self == null)
		{
            Self = this;
            bounds = GetComponent<BoundsCheck>();
        }
		else
		{
            Debug.LogError("Hero.Awake() - Attempted to assign second hero.Self!");
		}

        //fireDelegate += TempFire;
        ClearWeapons();
        weapons[0].SetType(WeaponType.seekerMissle);
	}

    private GameObject lastTriggerGameObject = null;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

	private void OnTriggerEnter(Collider other)
	{
        Transform root = other.gameObject.transform.root;
        var go = root.gameObject;
        print($"Triggered: {go.name}");

        if(go == lastTriggerGameObject)
		{
            return;
		}

        lastTriggerGameObject = go;

        if(go.tag == "Enemy")
		{
            ShieldLevel--;
            Destroy(go);
		}
        else if (go.tag == "PowerUp")
		{
            AbsorbPowerUp(go);
		}
		else
		{
            print($"Triggered by non-Enemy : {go.name}");
		}
	}

    public void AbsorbPowerUp(GameObject go)
	{
        var powerUp = go.GetComponent<PowerUp>();

		switch (powerUp.type)
		{
            case WeaponType.shield:
                ShieldLevel++;
                break;
            default:
                if(powerUp.type == weapons[0].type)
				{
                    var weaponSlot = GetEmptyWeaponSlot();
                    if(weaponSlot != null)
					{
                        weaponSlot.SetType(powerUp.type);
					}					
				}
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(powerUp.type);
                }
                break;
		}
        powerUp.AbsorbedBy(this.gameObject);
	}

	public float ShieldLevel
	{
		get { return _shieldLevel; }
		set { _shieldLevel = Mathf.Min(value, 4);
        if(value < 0)
			{
                //Destroy(this.gameObject);
                //Main.Self.DelayedRestart(gameRestartDelay);
			}
		}
	}

    Weapon GetEmptyWeaponSlot()
	{
        return weapons.FirstOrDefault(x => x.type == WeaponType.none);
	}

    void ClearWeapons()
	{
        foreach(var weapon in weapons)
		{
            weapon.SetType(WeaponType.none);
		}
	}

    void Update()
    {
        var xAxis = Input.GetAxis("Horizontal");
        var xMult = 1f;
        var yMult = 1f;
        var yAxis = Input.GetAxis("Vertical");

        var pos = transform.position;

        if( !sameSign(xAxis, currXSpeed)) //Mathf.Abs(currXSpeed) > speed/2 &&
        {
            xMult *= (1 + XStopMult * Mathf.Abs((speed - Mathf.Abs(currXSpeed)) / speed));
        }
        else if( sameSign(xAxis, currXSpeed) && Mathf.Abs(currXSpeed) * 2 < speed) 
        {
            xMult *= (1 + XStartMult * Mathf.Abs((speed - Mathf.Abs(currXSpeed)) / speed));
        }


        if (!sameSign(yAxis, currYSpeed)) //Mathf.Abs(currYSpeed) > speed/2 && 
        {
            yMult = (1 + YStopMult * ((speed - Mathf.Abs(currYSpeed)) / speed));
        }
        else if (sameSign(yAxis, currYSpeed) && Mathf.Abs(currYSpeed) * 2 < speed) 
        {
            yMult = (1 + YStartMult * ((speed - Mathf.Abs(currYSpeed)) / speed));
        }

        currXSpeed += xAxis * Time.deltaTime * acceleration * xMult;
        currYSpeed += yAxis * Time.deltaTime * acceleration * yMult;
        pos.x += currXSpeed * Time.deltaTime;
        pos.y += currYSpeed * Time.deltaTime;
        transform.position = pos;


  //          currXSpeed = Mathf.MoveTowards(currXSpeed, 0, Time.deltaTime * Mathf.Abs(currXSpeed));
  //          currYSpeed = Mathf.MoveTowards(currYSpeed, 0, Time.deltaTime * Mathf.Abs(currYSpeed));
		

        shootingAngle = -transform.rotation.z;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        if (bounds != null && bounds.offBottom || bounds.offLeft || bounds.offRight)
        {
            //Destroy(this.gameObject);
            //Main.Self.DelayedRestart(gameRestartDelay);
        }

        //if (Input.GetKeyDown(KeyCode.Space)) 
        //{
        //    TempFire();
        //}

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
		{
            fireDelegate();
		}
    }

    bool sameSign(float num1, float num2)
    {
        if (num1 > 0 && num2 < 0)
            return false;
        if (num1 < 0 && num2 > 0)
            return false;
        return true;
    }

    void TempFire()
	{
        GameObject projectile = Instantiate<GameObject>(projectilePrefab);
        projectile.transform.position = transform.position;
        var rigidBody = projectile.GetComponent<Rigidbody>();
        var shootingDir = Vector3.up;
        shootingDir.x = shootingAngle;

        var proj = projectile.GetComponent<Projectile>();

        proj.type = WeaponType.blaster;

        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        
        rigidBody.velocity = shootingDir * tSpeed;


	}
}
