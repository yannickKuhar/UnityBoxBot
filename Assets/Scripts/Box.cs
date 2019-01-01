using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	private Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Ko se je ponastavila pozicija skatle se
	// je ohranila sila iz prejsnjega trka medtem,
	// ko z to funkcijo to tezavo odstranimo.
	public void RemoveForce()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0f;
	}
}
