using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {

	// private GameObject goal;
	// private GameObject box;
	private Vector3 move;
	private Rigidbody2D rb;
	private float[] genom;
	public double fitness;
	private float maxTime;
	private float dx;
	private float dy;
	private float speed;
	

	// Use this for initialization
	void Start ()
	{
		// box = GameObject.Find("Box");
		// genom = new float[] {1.2f, 3.7f, -4.4f, 4.2f};
		// speed = new Vector3(genom[0] + genom[1], genom[2] + genom[3], 0.0f);
		InitPlayer();
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
		// TODO: Make movment vector change.
		transform.position += move * Time.fixedDeltaTime * speed;

		Vector3 detlaDir = new Vector3(move.x + dx, move.y + dy, 0f);
		rb.AddForce(detlaDir);

		// fitness = fitnessFunctuionBoxToGoal(box, goal);
		Destroy(gameObject, maxTime);
	}

	double fitnessFunctuionPlayerToBox(Box box)
	{
		float x = transform.position.x - box.transform.position.x;
		float y = transform.position.y - box.transform.position.y;
		float z = transform.position.z - box.transform.position.z;

		return Math.Sqrt((x * x) + (y * y) + (z * z));
	}

	double fitnessFunctuionBoxToGoal(Box box, Goal goal)
	{
		float x = goal.transform.position.x - box.transform.position.x;
		float y = goal.transform.position.y - box.transform.position.y;
		float z = goal.transform.position.z - box.transform.position.z;

		return Math.Sqrt((x * x) + (y * y) + (z * z));
	}
	
	public void InitPlayer()
	{ 
		rb = GetComponent<Rigidbody2D>();
		// box = GameObject.Find("Box");
		// goal = GameObject.Find("Goal");
		maxTime = 10f;

		genom = new float[] {UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5),
							 UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5)};

		dx = genom[0];
		dy = genom[1];

		move = new Vector3(genom[2] + genom[3], genom[4] + genom[5], 0.0f);

		speed = 1.2f;
	}

	//////////////////// Getters and Setters ////////////////////
	public float[] GetGenom()
	{
		return genom;
	}

	public double GetFitness()
	{
		return fitness;
	}

	public void SetGenom(float[] newGenom)
	{
		genom = newGenom;
	}

	public void SetFitness(Box box, Goal goal)
	{
		// fitness = fitnessFunctuionBoxToGoal(box, goal);
		fitness = fitnessFunctuionPlayerToBox(box) + fitnessFunctuionBoxToGoal(box, goal);
	}
}
