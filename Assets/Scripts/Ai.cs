using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour {

	public GameObject spawnPoint;
	public GameObject player;
	private GameObject[] population;
	private int count = 0;

	// Use this for initialization
	void Start ()
	{
		// Instantiate(player, spawnPoint.transform.position, spawnPoint.transform.rotation);

		population = new GameObject[100];

		for(int i = 0; i < 100; i++)
		{
			population[i] = player;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(count < 100)
		{
			Instantiate(population[count], spawnPoint.transform.position, spawnPoint.transform.rotation);
			count++;
		}
	}
}
