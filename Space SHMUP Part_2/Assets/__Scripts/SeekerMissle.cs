using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekerMissle : Projectile
{
	[Header("Set in Inspector")]
	public float acceleration = 5f;
	public float lockOnAcceleration = 10f;

	public float rotationSpeed = 25f;


	private List<GameObject> _targets = new List<GameObject>();

	private float _speed = 0;

	protected override void Awake()
	{
		bounds = GetComponent<BoundsCheck>();
		
		rend = transform.Find("Head").GetComponent<Renderer>();
		rigid = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		//transform.position = Vector3.zero;
		
	}

	private void OnTriggerEnter(Collider other)
	{
		
		_targets.Add(other.gameObject);
		
	}

	//private void OnTriggerStay(Collider other)
	//{
		 
	//}

	private void OnTriggerExit(Collider other)
	{
		
		_targets.Remove(other.gameObject);
		
	}


	void Update()
	{
		_targets.RemoveAll(x => x == null);

		_speed += Time.deltaTime * acceleration;

		if (_targets.Any())
		{


			var closest = _targets.OrderBy(x => GetDistance(x)).First();


			var targetLocation = closest.transform.position;
			targetLocation.z = transform.position.z;
			var enemyDirection = (targetLocation - transform.position).normalized;


			var lookRotation = Quaternion.LookRotation(Vector3.forward, enemyDirection);

			

			//var deltaAngle = 0f;

			
			
			
			//deltaAngle = (transform.localEulerAngles.z) - (lookRotation.eulerAngles.z);

			
			

			//if ( Mathf.Abs(deltaAngle) > 5f)
			//{
			//	var smoothedTurnAngle = deltaAngle * Time.deltaTime * rotationSpeed;


				
				// this will make x-axis of sprite face target instead of y-axis
			transform.rotation = Quaternion.Euler(0, 0, lookRotation.eulerAngles.z );
			//}

			

			_speed += Time.deltaTime * lockOnAcceleration;

		}

		transform.Translate(Vector3.up * _speed * Time.deltaTime, Space.Self);


		if (bounds.offUp)
		{
			Destroy(gameObject);
		}
	}

	private float NormalizeAngle(float angle)
	{
		
		
		if(angle > 180 && angle < 360)
		{
			return angle - 360;
		}

		return angle;
		
	}

	private float GetDistance(GameObject target)
	{
		return Vector3.Distance(gameObject.transform.position, target.transform.position);
	}
}
