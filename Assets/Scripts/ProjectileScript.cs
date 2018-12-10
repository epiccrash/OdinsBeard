using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public Rigidbody Projectile;
    public float ProjectileSpeed = 14.0f;
    public GameObject Player;
    private bool shootToRight;

    public GameObject mainCam;

	// Use this for initialization
	void Start () 
    {
        Player = GameObject.Find("Player");
        shootToRight = Player.GetComponent<PlayerController>().isFacingRight;
        // Grab the camera
        mainCam = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (shootToRight)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            transform.position = transform.position + Vector3.right * 0.3f;
        }
        else {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.position = transform.position + Vector3.left * 0.3f;
        }

        // Kills the projectile if it goes off-screen
        if (!GetComponent<Renderer>().isVisible) {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            Destroy(this.gameObject);
        }
    }
}
