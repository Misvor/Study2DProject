using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
	private static readonly float bottomY = -20f;
	private void FixedUpdate()
	{
		if(this.transform.position.y < bottomY)
		{
			Destroy(this.gameObject);

			ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();

			apScript.AppleDestroyed();
		}
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
