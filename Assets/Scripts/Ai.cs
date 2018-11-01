using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour {

	public GameObject spawnPoint;
	public GameObject player;
	private GameObject[] population;

	// Use this for initialization
	void Start ()
	{
		Instantiate(player, spawnPoint.transform.position, spawnPoint.transform.rotation);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
