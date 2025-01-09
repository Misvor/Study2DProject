using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{


	[Header("Set Dynamically")]
	public Text scoreGT;
	// Start is called before the first frame update
	void Start()
	{
		GameObject scoreGO = GameObject.Find("ScoreCounter");

		scoreGT = scoreGO.GetComponent<Text>();
		scoreGT.text = "0";
	}

	// physics update, Called 50 times a second
	private void FixedUpdate()
	{
		Vector3 mousePos2D = Input.mousePosition;

		mousePos2D.z = -Camera.main.transform.position.z;

		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

		Vector3 pos = this.transform.position;
		pos.x = mousePos3D.x;
		this.transform.position = pos;
	}

	private void OnCollisionEnter(Collision collision)
	{
		var collidedWith = collision.gameObject;
		if (collidedWith.tag == "Apple")
		{
			Destroy(collidedWith);

			if(int.TryParse(scoreGT.text, out var score))
			{
				score += 100;
				scoreGT.text = score.ToString();

				if(score > HighScore.score)
				{
					HighScore.score = score;
				}
			}
		}
	}


	// Update is called once per frame
	void Update()
	{

	}
}
