using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

    public float distToPlayer;

    private Vector3 startPos; //position before each frame
    private Vector3 newPos; //position change during each frame
    private Vector3 center; //center of neutral phase
    private float StartVerSpeed = -0.15f;
    private float StartHoriSpeed = 0.075f;
    private float verSpeed;
    private float horiSpeed;
    private float gravity = 0.0024f;
    private float speed = 0.0f; //-0.15f;
    private float force; //accelaration to the center when neutral
    private float upperRange;
    private int mode; //0 for neutral, 1 for divebomb, 2 for reset
    private int attackDirection;
    private float timer;
    private bool needIni; //to initialize neutral() and divebomb()

    public float hp;
    bool dead;

    GameObject character;
    Vector3 characterPos;

    // Added for animation
    private bool attacking;
    private bool alerted;
    private bool alive;
    private bool hurt;
    private Animator animator;

    void Start()
    {
        distToPlayer = 5.0f;

        startPos = transform.position;
        mode = 0;
        upperRange = transform.position.y + 0.1f;
        timer = 0.0f;
        verSpeed = StartVerSpeed;
        horiSpeed = StartHoriSpeed;
        attackDirection = 0;
        needIni = true;
        character = GameObject.Find("Player");

        dead = false;

        // Added for animation
        attacking = false;
        alerted = false;
        alive = true;
        hurt = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            //Debug.Log(timer);
            if (mode == 0)
            {
                Neutral();
            }

            else if (mode == 1)
            {
                Divebomb();
            }

            startPos = transform.position;
        } else {
            // Added in for death effect
            transform.Translate(Vector3.down * 5 * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.forward * 10, Space.Self);
            if (!GetComponent<SpriteRenderer>().isVisible) {
                Destroy(this.gameObject);
            }
        }

        animator.SetBool("attacking", attacking);
        animator.SetBool("alerted", alerted);
        animator.SetBool("alive", alive);
        animator.SetBool("hurt", hurt);
    }

    void Neutral()
    {
        // Added for animation
        attacking = false;
        hurt = false;
        if (attackDirection < 0) GetComponent<SpriteRenderer>().flipX = true;
        else GetComponent<SpriteRenderer>().flipX = false;

        timer += 1;
        if (Distance() < distToPlayer)
        {
            // Added for animation
            alerted = true;
            if (timer > 150) {
                mode = 1; //switch to divebomb mode
                needIni = true;
                timer = 0;
                return;
            }
        }
        if(needIni)
        {
            center.Set(startPos.x + 1.0f, startPos.y, startPos.z);
            needIni = false;
        }
        force = (center.x - startPos.x) * 0.002f;
        speed = speed + force;
        newPos.Set(speed, 0, 0);
        transform.position = startPos + newPos;
    }

    void Divebomb()
    {
        // Added for animation
        alerted = false;
        attacking = true;

        timer += 1;
        if(needIni)
        {
            DivebombIni();
        }
        verSpeed = verSpeed + gravity;
        newPos.Set(horiSpeed, verSpeed, 0);
        if (timer <= 10 || transform.position.y <= upperRange)
        {
            transform.position = startPos + newPos;
        }
        else 
        {
            timer = 0.0f;
            speed = 0;
            mode = 0;
            needIni = true;
        }
    }

    void DivebombIni()
    {
        timer = 0;
        if (transform.position.x > character.transform.position.x)
            attackDirection = -1;
        else
            attackDirection = 1;
        verSpeed = StartVerSpeed;
        horiSpeed = StartHoriSpeed * attackDirection;
        needIni = false;

        SoundManager.S.makeWoosh();
    }

    float Distance()
    {
        characterPos = character.transform.position;
        float distance = 0;
        distance = Math.Abs(transform.position.x - characterPos.x);
        return distance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            hp--;
            hurt = true;
        }
        if (collision.gameObject.tag == "Projectile")
        {
            hp -= 0.5f;
            hurt = true;
        }

        if (hp < 0.5f)
        {
            dead = true;
            GetComponent<Collider2D>().enabled = false;

            // Added in for animation
            alive = false;
        }
    }



}
