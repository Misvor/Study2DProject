using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prevents the exit of game object out of bounds
/// Works ONLY with orthographic camera
/// </summary>
public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnLeave = true;


    [Header("Set Dynamically")]
    public float camWidth;
    public float camHeight;
    public bool isOnScreen = true;

    [HideInInspector]
    public bool offRight, offLeft, offUp, offBottom;

	private void Awake()
	{
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
	}

	private void LateUpdate()
	{
        offRight = offLeft = offUp = offBottom = false;
        var pos = transform.position;
        isOnScreen = true;

        var rightBorder = camWidth - radius;
        if(pos.x > rightBorder)
		{
            pos.x = rightBorder;
            offRight = true;

        }

        var leftBorder = -camWidth + radius;
        if(pos.x < leftBorder)
		{
            pos.x = leftBorder;
            offLeft = true;
        }

        var upperBorder = camHeight - radius;
        if(pos.y > upperBorder)
		{
            pos.y = upperBorder;
            offUp = true;
        }

        var bottom = -camHeight + radius;
        if(pos.y < bottom)
		{
            pos.y = bottom;
            offBottom = true;
        }

        isOnScreen = !(offBottom || offLeft || offRight || offUp);
        if (keepOnLeave && !isOnScreen)
		{
            transform.position = pos;
            isOnScreen = true;
            //offRight = offLeft = offUp = offBottom = false;

        }

    }

	private void OnDrawGizmos()
	{
        if (!Application.isPlaying) return;
        var boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);

        Gizmos.DrawWireCube(Vector3.zero, boundSize);
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
