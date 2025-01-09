using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{

    [Header("Set Dynamically")]
    public List<Boid> neighbors;
    private SphereCollider border;


    // Start is called before the first frame update
    void Start()
    {
        neighbors = new List<Boid>();
        border = GetComponent<SphereCollider>();
        border.radius = Spawner.spawner.neighborDist;

    }
	private void FixedUpdate()
	{
		if(border.radius != Spawner.spawner.neighborDist / 2)
		{
            border.radius = Spawner.spawner.neighborDist / 2;
        }
	}

	private void OnTriggerEnter(Collider other)
	{
        Boid newcomer = other.GetComponent<Boid>();
        if(newcomer != null && !neighbors.Contains(newcomer))
		{           
            neighbors.Add(newcomer);			
		}
	}
	private void OnTriggerExit(Collider other)
	{
        Boid lefter = other.GetComponent<Boid>();
        if(lefter != null && neighbors.Contains(lefter))
		{
            neighbors.Remove(lefter);
		}
	}
    public Vector3 avgPos
	{
		get 
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;
            for (int i = 0; i < neighbors.Count; i++)
			{
                avg += neighbors[i].pos;
			}
            avg /= neighbors.Count;
            return avg;
        }
		
	}
    public Vector3 avgVel
	{
		get
		{
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;
            for(int i = 0;  i < neighbors.Count; i++)
			{
                avg += neighbors[i].rigid.velocity;
			}
            avg /= neighbors.Count;
            return avg;
		}
	}
    public Vector3 avgClosePos
	{
		get
		{
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;
            for(int i = 0; i < neighbors.Count; i++)
			{
                delta = neighbors[i].pos - transform.position;
                if(delta.magnitude <= Spawner.spawner.collDist)
				{
                    avg += neighbors[i].pos;
                    nearCount++;
				}
			}
            if (nearCount == 0) {
                return avg;
            }
            avg /= nearCount;
            return avg;

		}
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
