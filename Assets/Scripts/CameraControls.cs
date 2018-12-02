using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Carter kindly donated this script. Thanks Carter!

public class CameraControls : MonoBehaviour {


		//TODO the dj.enabled middle camera isn't working. Imagine a ball infront of the player

	public GameObject player;

	//private Vector3 patronus;

	[SerializeField]
	Vector2 margin = new Vector2(.15f,.15f);

	private float xVelocity = 0.0f;
	private float yVelocity = 0.0f;
	[SerializeField]
	private float yOffset = 0.0f;
	[SerializeField]
	private float xOffset = 0.0f;
	// In case I want player to be near bottom edge of camera. We'll have offset equal 0 for now.

	private bool Ytracking;
	private bool Xtracking;/*
	private bool XtrackingMid;
	private bool XtrackingFar;
	private float xDir = 1;*/

	[SerializeField]
	float minSize;
	[SerializeField]
	float maxSize;
    

	private float sizeDelta;
	

	private Rigidbody2D prb;
	private float cameraSpeed;


	// Use this for initialization
	void Start () {
		prb = player.GetComponent<Rigidbody2D>();
	}

	
	// Update is called once per frame
	void Update () {
		if(!prb) prb = player.GetComponent<Rigidbody2D>();



        Vector3 playerposition = prb.transform.position;
		Vector3 cameraposition = transform.position;

		Vector3 bias = (Vector3.one * .5f - Camera.main.WorldToViewportPoint(playerposition));

		Debug.Log(bias);


		if (bias.x > .5f + margin.x + xOffset || bias.x < .5f - margin.x + xOffset) {
			Xtracking = true;
		}
		if(Xtracking){
			cameraposition.x = Mathf.SmoothDamp (cameraposition.x, playerposition.x + xOffset, ref xVelocity, 1/cameraSpeed);
			transform.position = cameraposition;
			if(cameraposition.x == playerposition.x) Xtracking = false;
		}


		if (bias.y > .5f + margin.y + yOffset || bias.y < .5f - margin.y + yOffset) {
			Ytracking = true;
		}
		if(Ytracking){
			cameraposition.y = Mathf.SmoothDamp (cameraposition.y, playerposition.y + yOffset, ref yVelocity, .75f/cameraSpeed);
			transform.position = cameraposition;
			if(cameraposition.y == playerposition.y) Ytracking = false;
		}

	}




}
