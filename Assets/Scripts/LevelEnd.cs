using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

    GameObject player;
    public GameObject EndOfLevelMarker;
    public GameObject blackScreen;
    public Text levelText;
    public string nextSceneName;
    public int startDelay;
    public string nextScene;
    

    bool moveOff;
    bool moveOn;
    int count;

    // Use this for initialization
    void Start () {
        blackScreen.SetActive(true);
		player = GameObject.FindGameObjectWithTag("Player");
        PlayerController script = player.GetComponent<PlayerController>();
        script.enabled = false;
        levelText.text = nextSceneName;
        startDelay = startDelay * 24;
        moveOff = true;
        moveOn = false;
        count = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if ((player.transform.position.x >= EndOfLevelMarker.transform.position.x) && !moveOn)
        {
            moveOn = true;
            PlayerController script = player.GetComponent<PlayerController>();
            script.enabled = false;
        }

        if (moveOff)
        {
            if (count < startDelay)
            {
                count++;
                if (count > 3 * startDelay/4)
                {
                    levelText.text = "";
                }
            }
            else
            {
                if (blackScreen.transform.position.x < 150)
                {
                    blackScreen.transform.position += new Vector3(5, 0, 0);
                }
                else
                {
                    blackScreen.transform.position = new Vector3(-150, 0, 0);
                    moveOff = false;
                    PlayerController script = player.GetComponent<PlayerController>();
                    script.enabled = true;
                }
            }
        }

        if (moveOn)
        {
            if (blackScreen.transform.position.x < 0)
            {
                blackScreen.transform.position += new Vector3(5, 0, 0);
            }
            else
            {
                //Time to load a new scene!
                Debug.Log("Loading new Scene: " + nextScene);
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
        }
		
	}
}
