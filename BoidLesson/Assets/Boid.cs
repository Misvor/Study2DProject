using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    [Header("Set Dynamically")]
    public Rigidbody rigid;

    private Neighborhood neighborhood;

    void LookAhead()
	{
        transform.LookAt(pos + rigid.velocity);
	}
    /// <summary>
    /// Vector to position of object
    /// </summary>
    public Vector3 pos
	{
        get { return transform.position; }
		set { transform.position = value; }
	}
	private void Awake()
	{
        neighborhood = GetComponent<Neighborhood>();
        rigid = GetComponent<Rigidbody>();
        // random starting position
        pos = Random.insideUnitSphere * Spawner.spawner.spawnRadius;
        // random starting velocity
        Vector3 vel = Random.onUnitSphere * Spawner.spawner.velocity;

        LookAhead();

        // pick random color
        Color randColor = Color.black;
        while( randColor.r + randColor.g + randColor.b < 1.0f)
		{
            randColor = new Color(Random.value, Random.value, Random.value);
		}
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach(var render in rends)
		{
            render.material.color = randColor;
		}
        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", randColor);
	}


	private void FixedUpdate()
	{
        Vector3 vel = rigid.velocity;
        Spawner spawner = Spawner.spawner;


        // Prevent Collide
        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;

        // if zero, it's ok
        if(tooClosePos != Vector3.zero)
		{
            velAvoid = pos - tooClosePos;
            velAvoid.Normalize();
            velAvoid *= spawner.velocity;
		}

        // align speed of heighbors
        Vector3 velAlign = neighborhood.avgVel;

        if(velAlign != Vector3.zero)
		{
            velAlign.Normalize();

            velAlign *= spawner.velocity;
		}

        // concentrate neighbors
        Vector3 velCenter = neighborhood.avgPos;
        if(velCenter != Vector3.zero)
		{
            velCenter -= transform.position;
            velCenter.Normalize();
            velCenter *= spawner.velocity;
		}


        // Attraction
        Vector3 delta = Attractor.POS - pos;


        bool attracted = (delta.magnitude > spawner.attractPushDist);
        Vector3 velAttract = delta.normalized * spawner.velocity;

        // Apply velocities

        float fdt = Time.fixedDeltaTime;

		if (attracted)
		{
            vel = Vector3.Lerp(vel, velAttract, spawner.attractPull * fdt);
		}
		else
		{
            vel = Vector3.Lerp(vel, -velAttract, spawner.attractPush * fdt);
		}

        // set vel as in spawner
        vel = vel.normalized * spawner.velocity;

        rigid.velocity = vel;

        LookAhead();

	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
