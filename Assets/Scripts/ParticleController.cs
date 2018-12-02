using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    public ParticleSystem particles;
    public int particlesPerHit;

	// Use this for initialization
	void Start () {
        particles.Stop(true);
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Sword")
        if (collision.gameObject.tag == "Sword")
        {
            //Debug.Log("Script had a collision!!");
            particles.Emit(particlesPerHit);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Sword")
        if (collision.gameObject.tag == "Sword")
        {
            //Debug.Log("Script had a collision!!");
            particles.Emit(particlesPerHit);
        }
        
    }
}
