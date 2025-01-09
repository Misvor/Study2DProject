using System.Collections;
using System.Collections.Generic;
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

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private float currYSpeed = 0;
    private float currXSpeed = 0;
    public float shootingAngle = 0;

    
	private void Awake()
	{
       if(Self == null)
		{
            Self = this;
		}
		else
		{
            Debug.LogError("Hero.Awake() - Attempted to assign second hero.Self!");
		}
	}

    private GameObject lastTriggerGameObject = null;

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
		else
		{
            print($"Triggered by non-Enemy : {go.name}");
		}
	}
	// Start is called before the first frame update
	public float ShieldLevel
	{
		get { return _shieldLevel; }
		set { _shieldLevel = Mathf.Min(value, 4);
        if(value < 0)
			{
                Destroy(this.gameObject);
                Main.Self.DelayedRestart(gameRestartDelay);
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        var xAxis = Input.GetAxis("Horizontal");
        var xMult = 1f;
        var yMult = 1f;
        var yAxis = Input.GetAxis("Vertical");

        var pos = transform.position;

        if(Mathf.Abs(currXSpeed) > speed/2 && !sameSign(xAxis, currXSpeed)) 
        {
            xMult *= XStopMult * ((speed - Mathf.Abs(currXSpeed)) / speed);
        }
        else if(sameSign(xAxis, currXSpeed) && currXSpeed < speed) 
        {
            xMult *= XStartMult * ((speed - Mathf.Abs(currXSpeed)) / speed);
        }


        if (Mathf.Abs(currYSpeed) > speed / 2 && !sameSign(yAxis, currYSpeed)) 
        {
            yMult *= YStopMult * ((speed - Mathf.Abs(currYSpeed)) / speed);
        }
        else if (sameSign(yAxis, currYSpeed) && Mathf.Abs(currYSpeed) < speed) 
        {
            yMult *= YStartMult * ((speed - Mathf.Abs(currYSpeed)) / speed);
        }

        currXSpeed += xAxis * Time.deltaTime * acceleration * xMult;
        currYSpeed += yAxis * Time.deltaTime * acceleration * yMult;
        pos.x += currXSpeed * Time.deltaTime;
        pos.y += currYSpeed * Time.deltaTime;
        transform.position = pos;

  //      if(xAxis == 0 && yAxis == 0)
		//{
  //          currXSpeed = Mathf.MoveTowards(currXSpeed, 0, Time.deltaTime * Mathf.Abs(currXSpeed));
  //          currYSpeed = Mathf.MoveTowards(currYSpeed, 0, Time.deltaTime * Mathf.Abs(currYSpeed));
		//}

        shootingAngle = -transform.rotation.z;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
       

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            TempFire();
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
        rigidBody.velocity = shootingDir * projectileSpeed;

	}
}
