using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    // 
    public GameObject applePrefab;

    public float speed = 1f;

    public float leftAndRightEdge = 10f;

    public float chanceToChangeDirection = 0.1f;

    public float secondsBetweenAppleDrops = 1f;

    private int moveDirection = 1;   

    private float objectSpeed = 0;   

    private enum Direction
	{
        left = -1,
        right = 1
	}

    private void SetLeftDirection()
	{
        moveDirection = (int)Direction.left;
        objectSpeed = speed * moveDirection;
    }
    private void SetRightDirection()
	{
        moveDirection = (int)Direction.right;
        objectSpeed = speed * moveDirection;
    }

    private bool CrossedRightBorder(float posX)
	{
        return posX > leftAndRightEdge;
    } 

    private bool CrossedLeftBorder(float posX)
	{
        return posX < -leftAndRightEdge;
    }

	// physics update, Called 50 times a second
	private void FixedUpdate()
	{        
        Vector3 pos = transform.position;

        if(CrossedLeftBorder(pos.x))
		{
            SetRightDirection();
        }
        else if(CrossedRightBorder(pos.x))
		{
            SetLeftDirection();
        }

        if(Random.value < chanceToChangeDirection)
		{
            objectSpeed *= -1;            
		}


        pos.x += objectSpeed * Time.deltaTime;
        transform.position = pos;
    }


	// Start is called before the first frame update
	void Start()
	{
        Invoke(nameof(DropApple), 2f);
    }

    void DropApple()
	{
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = this.transform.position;
        Invoke(nameof(DropApple), secondsBetweenAppleDrops);

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}

