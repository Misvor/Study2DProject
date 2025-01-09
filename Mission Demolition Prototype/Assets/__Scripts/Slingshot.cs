using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{

	private static Slingshot S;

	[Header("Set in Inspector")]
	public GameObject prefabProjectile;

	public float velocityMult = 8f;

	[Header("Set Dynamically")]
	public GameObject launchPoint;

	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;
	private Rigidbody projectileRigidbody;

	static public Vector3 LAUNCH_POS
	{
		get
		{
			if(S == null)
			{
				return Vector3.zero;
			}
			return S.launchPos;
		}
	}

	private void Awake()
	{
		S = this;
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
	}

	void OnMouseEnter()
	{
		//print("Slingshot:OnMouseEnter()");
		launchPoint.SetActive(true);
	}

	private void OnMouseExit()
	{
		launchPoint.SetActive(false);

		//print("Slingshot:OnMouseExit()");
	}

	
	private void OnMouseDown()
	{
		aimingMode = true;		

		projectile = Instantiate(prefabProjectile);

		projectile.transform.position = launchPos;

		//projectile.GetComponent<Rigidbody>().isKinematic = true;
		projectileRigidbody = projectile.GetComponent<Rigidbody>();
		projectileRigidbody.isKinematic = true;


	}

	private void Update()
	{
		if (!aimingMode)
		{
			return;
		}

		var mousePos2D = Input.mousePosition;

		mousePos2D.z = -Camera.main.transform.position.z;
		var mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

		var mouseDelta = mousePos3D - launchPos;

		var maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if(mouseDelta.magnitude > maxMagnitude)
		{
			mouseDelta.Normalize();
			mouseDelta *= maxMagnitude;
		}

		var projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;

		var leftArm = this.transform.Find("LeftArm");
		var leftBand = leftArm.GetComponent<LineRenderer>();
		leftBand.SetPosition(0, leftArm.gameObject.transform.position + (leftArm.transform.up * leftArm.transform.localScale.y));
		leftBand.SetPosition(1, projectile.transform.position);
		leftBand.startWidth = 0.5f * (1-(mouseDelta.magnitude / maxMagnitude)) + 0.2f;

		var rightArm = this.transform.Find("RightArm");
		var rightBand = rightArm.GetComponent<LineRenderer>();
		rightBand.SetPosition(0, rightArm.gameObject.transform.position + (rightArm.transform.up * rightArm.transform.localScale.y));
		rightBand.SetPosition(1, projectile.transform.position);
		rightBand.startWidth = 0.5f * (1 - (mouseDelta.magnitude / maxMagnitude)) + 0.2f;
		rightBand.enabled = aimingMode;
		leftBand.enabled = aimingMode;

		if (Input.GetMouseButtonUp(0))
		{
			aimingMode = false;
			projectileRigidbody.isKinematic = false;
			projectileRigidbody.velocity = -mouseDelta * velocityMult;
			FollowCam.POI = projectile;
			projectile = null;
			MissionDemolition.ShotFired();
			ProjectLine.S.poi = projectile;
		}
	}
}
