using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour {
    public GameObject startScreen;
    public Button button1, button2, button3;
    public GameObject launchScreen;
    public Button launch1, launch2, launch3, launch4;
    public GameObject optionsScreen;
    public Slider MVolume, SVolume;
    public Toggle full;
    public Button optionsReturn;
    // Added
    public Button credits;
    bool movingUp;
    bool movingDown;
    bool movingToOptions;
    bool movingBack;

	// Use this for initialization
	void Start () {
        print("StartScreen: " + startScreen.transform.position.y);
        print("LaunchScreen: " + launchScreen.transform.position.y);
        Button btn1 = button1.GetComponent<Button>();
        Button btn2 = button2.GetComponent<Button>();
        Button btn3 = button3.GetComponent<Button>();
        btn1.onClick.AddListener(moveToSecond);
        btn2.onClick.AddListener(getOptions);
        btn3.onClick.AddListener(exitGame);

        Button l1 = launch1.GetComponent<Button>();
        Button l2 = launch2.GetComponent<Button>();
        Button l3 = launch3.GetComponent<Button>();
        Button l4 = launch4.GetComponent<Button>();

        l1.onClick.AddListener(load1);
        l2.onClick.AddListener(load2);
        l3.onClick.AddListener(load3);
        l4.onClick.AddListener(moveToFirst);

        Slider opt1 = MVolume.GetComponent<Slider>();
        Slider opt2 = SVolume.GetComponent<Slider>();
        Toggle opt3 = full.GetComponent<Toggle>();
        Button opt4 = optionsReturn.GetComponent<Button>();
        // Added
        Button creditsButton = credits.GetComponent<Button>();

        opt1.onValueChanged.AddListener(delegate { volumeCheck(); });
        opt2.onValueChanged.AddListener(delegate { volumeCheck(); });
        opt3.onValueChanged.AddListener(delegate { fullScreenCheck(opt3); });
        opt4.onClick.AddListener(moveToMenu);
        // Added
        creditsButton.onClick.AddListener(openCredits);

        movingUp = false;
        movingDown = false;
        movingToOptions = false;
        movingBack = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (movingDown)
        {
            /*
            if (startScreen.transform.position.y < -256)
            {
                startScreen.transform.position += new Vector3(0, -10, 0);
                launchScreen.transform.position += new Vector3(0, 10, 0);
            }
            else
            {
                movingDown = false;
            }
            */
            SceneManager.LoadScene("Opening");
        }
        else if (movingUp)
        {
            /*
            if (startScreen.transform.position.y < 244)
            {
                startScreen.transform.position += new Vector3(0, 10, 0);
                launchScreen.transform.position += new Vector3(0, -10, 0);
            }
            else
            {
                movingUp = false;
            }
            */
        }

        if (movingToOptions)
        {
            /*
            if (startScreen.transform.position.y < 744)
            {
                startScreen.transform.position += new Vector3(0, 10, 0);
                optionsScreen.transform.position += new Vector3(0, -10, 0);
            }
            else
            {
                movingToOptions = false;
            }
            */
            startScreen.GetComponent<CanvasGroup>().alpha = 0.0f;
            startScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
            optionsScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
            optionsScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        if (movingBack)
        {
            /*
            //if (startScreen.transform.position.y > 244)
            if (startScreen.transform.position.y > 363)
            {
                startScreen.transform.position += new Vector3(0, -10, 0);
                optionsScreen.transform.position += new Vector3(0, 10, 0);
            }
            else
            {
                movingBack = false;
            }
            */
            startScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
            startScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
            optionsScreen.GetComponent<CanvasGroup>().alpha = 0.0f;
            optionsScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

	}

    //Shifts to screen 2
    void moveToSecond()
    {
        print("Switching to Launch screen.");
        movingDown = true;
    }

    void getOptions()
    {
        print("Moving to Options");
        movingToOptions = true;
        // Added
        movingBack = false;
    }

    void moveToMenu()
    {
        print("Returning from Options");
        movingBack = true;
        // Added
        movingToOptions = false;
    }

    public void exitGame()
    {
        print("Exiting game...");
        Application.Quit();
    }

    void load1()
    {
        print("This should switch to scene 1.");
    }

    void load2()
    {
        print("This should switch to scene 2.");
    }

    void load3()
    {
        print("This should switch to scene 3.");
    }

    void moveToFirst()
    {
        movingUp = true;
    }

    void volumeCheck()
    {
        print("Volume was changed, resetting values in player prefs.");
        print("Volumes are now " + MVolume.value + ", " + SVolume.value);
        PlayerPrefs.SetInt("Sound Volume", (int)SVolume.value);
        PlayerPrefs.SetInt("Music Volume", (int)MVolume.value);
    }

    void fullScreenCheck(Toggle change)
    {
        print("Toggle is now " + full.isOn);
        if (Screen.fullScreen != full.isOn)
        {
            print("Switching fullscreen mode to " + full.isOn);
            Screen.fullScreen = full.isOn;
        }
    }

    // Added
    void openCredits() {
        SceneManager.LoadScene("Credits");
    }
}
