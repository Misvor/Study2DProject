using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectLine : MonoBehaviour
{

	public static ProjectLine S;

	[Header("Set in Inspector")]
	public float minDist = 0.1f;

	private LineRenderer line;
	private GameObject _poi;
	private List<Vector3> points;

	private void Awake()
	{
		S = this;

		line = GetComponent<LineRenderer>();

		line.enabled = false;

		points = new List<Vector3>();
	}

	public GameObject poi
	{
		get { return (_poi); }
		set
		{
			_poi = value;
			if (_poi != null)
			{
				line.enabled = false;
				points = new List<Vector3>();
				AddPoint();
			}
		}
	}

	public void Clear()
	{
		_poi = null;
		line.enabled = false;
		points = new List<Vector3>();
	}

	public void AddPoint()
	{
		var pt = _poi.transform.position;

		if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
		{
			return;
		}

		if (points.Count == 0)
		{
			var launchPosDiff = pt - Slingshot.LAUNCH_POS;

			points.Add(pt + launchPosDiff);
			points.Add(pt);
			line.positionCount = 2;
			line.SetPosition(0, points[0]);
			line.SetPosition(1, points[1]);
			line.enabled = true;

		}
		else
		{
			points.Add(pt);
			line.positionCount = points.Count;
			line.SetPosition(points.Count - 1, lastPoint);
			line.enabled = true;
		}
	}

	public Vector3 lastPoint
	{
		get
		{
			if (points == null)
			{
				return Vector3.zero;
			}
			return points.Last();
		}
	}

	private void FixedUpdate()
	{
		if(poi == null && FollowCam.POI != null && FollowCam.POI.tag == "Projectile")
		{						
			poi = FollowCam.POI;					
		}
		if(poi == null)
		{
			return;
		}
		AddPoint();
		if (FollowCam.POI == null)
		{
			poi = null;
		}
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
