using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public Rigidbody Projectile;
    public float ProjectileSpeed = 14.0f;
    public GameObject Player;
    private bool shootToRight;



	// Use this for initialization
	void Start () 
    {
        Player = GameObject.Find("Player");
        shootToRight = Player.GetComponent<MovementScript>().isFacingRight;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (shootToRight)
            transform.position = transform.position + Vector3.right * 0.3f;
        else
            transform.position = transform.position + Vector3.left * 0.3f;
    }
}
