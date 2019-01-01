using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private bool hit;

    void Start ()
    {
        hit = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Box")
        {
            hit = true;
        }
        else if (col.gameObject.tag == "Player")
        {
            Destroy(col.gameObject);
        }
    }

    //////////////////// Getters and Setters ////////////////////

    public bool GetHit()
    {
        return hit;
    }
}
