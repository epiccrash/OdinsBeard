using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    // Singleton
    public static SoundManager S;

    public GameObject emptySlashObj;
    public GameObject golemSolidifyObj;
    public GameObject golemThrowObj;
    public GameObject HitSlashObj;
    public GameObject monsterRoarObj;
    public GameObject odinThunderObj;
    public GameObject playerJumpObj;
    public GameObject victPercussionObj;
    public GameObject wooshObj;
    public GameObject txtObj;

    private AudioSource emptySlash;
    private AudioSource golemSolidify;
    private AudioSource golemThrow;
    private AudioSource HitSlash;
    private AudioSource monsterRoar;
    private AudioSource odinThunder;
    private AudioSource playerJump;
    private AudioSource victPercussion;
    private AudioSource woosh;
    private AudioSource txt;

    // Use this for initialization
    void Start () {
        S = this;

        emptySlash = emptySlashObj.GetComponent<AudioSource>();
        golemSolidify = golemSolidifyObj.GetComponent<AudioSource>();
        golemThrow = golemThrowObj.GetComponent<AudioSource>();
        HitSlash = HitSlashObj.GetComponent<AudioSource>();
        monsterRoar = monsterRoarObj.GetComponent<AudioSource>();
        odinThunder = odinThunderObj.GetComponent<AudioSource>();
        playerJump = playerJumpObj.GetComponent<AudioSource>();
        victPercussion = victPercussionObj.GetComponent<AudioSource>();
        woosh = wooshObj.GetComponent<AudioSource>();
        txt = txtObj.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MakeEmptySlash() {
        emptySlash.Play();
    }

    public void MakeGolemSolidify() {
        golemSolidify.Play();
    }

    public void MakeGolemThrow() {
        golemThrow.Play();
    }

    public void MakeHitSlash() {
        HitSlash.Play();
    }

    public void MakeMonsterRoar() {
        monsterRoar.Play();
    }

    public void MakeOdinThunder() {
        odinThunder.Play();
    }

    public void MakePlayerJump() {
        playerJump.Play();
    }

    public void MakeVictPercussion() {
        victPercussion.Play();
    }

    public void makeWoosh() {
        woosh.Play();
    }

    public void MakeTxt() {
        txt.Play();
    }
}
