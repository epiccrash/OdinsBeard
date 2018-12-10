using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button backButton = GetComponent<Button>();
        backButton.onClick.AddListener(back);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void back() {
        SceneManager.LoadScene("Menu");
    }
}
