using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private BoundsCheck bounds;

	private void Awake()
	{
		bounds = GetComponent<BoundsCheck>();
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (bounds.offUp)
		{
			Destroy(gameObject);
		}
    }
}
