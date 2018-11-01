using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {

	public GameObject box;
	private Vector3 speed;
	private float[] genom;
	private double fitness;

	// Use this for initialization
	void Start ()
	{
		box = GameObject.Find("Box");
		genom = new float[] {1.2f, 3.7f, -4.4f, 4.2f};
		speed = new Vector3(genom[0] + genom[1], genom[2] + genom[3], 0.0f);
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Wall")
		{
			Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		transform.position += speed * Time.fixedDeltaTime;
		fitness = fitnessFunctuion(box);
	}

	double fitnessFunctuion(GameObject box)
	{
		float x = transform.position.x - box.transform.position.x;
		float y = transform.position.y - box.transform.position.y;
		float z = transform.position.z - box.transform.position.z;

		return Math.Sqrt((x * x) + (y * y) + (z * z));
	}

}
