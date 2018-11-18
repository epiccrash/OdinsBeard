using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(transform.position.x - player.transform.position.x) < 1)
        {
            player.GetComponent<PlayerController>().setCheckpoint(transform.position);
        }
	}
}
