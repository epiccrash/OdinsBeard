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

    private int distTraveled;
    //
    private Vector3 originalPos;
    private Vector3 scale;
    

    bool moveOff;
    bool moveOn;
    int count;

    bool respawn;
    bool movingUp;

    float width;
    float height;

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
        //
        distTraveled = 0;
        originalPos = transform.position;

        respawn = false;
        movingUp = false;

        width = GetComponentInParent<RectTransform>().rect.width;
        height = GetComponentInParent<RectTransform>().rect.height;
    }
	
	// Update is called once per frame
	void Update () {

        if ((player.transform.position.x >= EndOfLevelMarker.transform.position.x) && !moveOn)
        {
            SoundManager.S.MakeVictPercussion();

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
                PlayerController script = player.GetComponent<PlayerController>();
                script.enabled = true;
                Debug.Log(width);
                if (blackScreen.GetComponent<RectTransform>().transform.position.x < 1.5 * width)
                //if (distTraveled < 150)
                {
                    //blackScreen.transform.position += new Vector3(5, 0, 0);
                    blackScreen.transform.position += new Vector3(60, 0, 0);
                    distTraveled += 5;
                }
                else
                {
                    //blackScreen.transform.position = new Vector3(-150, 0, 0);
                    blackScreen.transform.position = originalPos;
                    moveOff = false;
                    //
                    blackScreen.SetActive(false);
                }
            }
        }

        if (moveOn)
        {
            if (blackScreen.transform.position.x < 0)
            {
                blackScreen.transform.position += new Vector3(10, 0, 0);
            }
            else
            {
                //Time to load a new scene!
                Debug.Log("Loading new Scene: " + nextScene);
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
        }

        if (movingUp)
        {

            //if (blackScreen.transform.position.y < 236)
            if (blackScreen.transform.position.y < height / 2)
            {
                blackScreen.transform.position += new Vector3(0, 30, 0);

            }
            else if (blackScreen.transform.position.x < 1.5 * width)
            //else if (blackScreen.transform.position.x < 1500)
            {
                blackScreen.transform.position = new Vector3(
                    blackScreen.transform.position.x, height / 2, 
                    blackScreen.transform.position.z);
                blackScreen.transform.position += new Vector3(60, 0, 0);
                distTraveled += 30;
                if (blackScreen.transform.position.x >= 1.5 * width)
                //if (blackScreen.transform.position.x > 1500)
                {
                    movingUp = false;
                }
            }
        }


        if (respawn)
        {
            Debug.Log("Moving the Respawn Screen");

            blackScreen.transform.position += new Vector3(60, 0, 0);

            distTraveled += 30;
            if (blackScreen.transform.position.x >= 1.5 * width)
            //if (distTraveled > 750)
            {
                respawn = false;
            }
        }
    }

    public void animateRespawn()
    {
        respawn = true;
        blackScreen.transform.position = new Vector3(-.5f * width, height / 2, 0.0f);
        //blackScreen.transform.position = new Vector3(378, 246, 0);
        blackScreen.SetActive(true);
        distTraveled = 0;
        Debug.Log("Respawn Screen Playing");
        movingUp = false;
    }

    public void animateFall()
    {
        movingUp = true;
        blackScreen.transform.position = new Vector3(width / 2, -100, 0);
        //blackScreen.transform.position = new Vector3(378, -100, 0);
        blackScreen.SetActive(true);
        respawn = false;
    }
}
