using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    public float speed;
    public float targetDistance;
    private float startPosX;

    public float hp;
    private bool dead;

    // reach end of platform where to move
    private bool movingRight = true;

    public Transform groundDetection;
    // Use this for initialization
    void Start()
    {
        startPosX = Mathf.Abs(transform.position.x);
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            // character move forward
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (groundDetection.name == "Tilemap (One Way)")
            {
                // move left when it reaches end of platform
                // (origin, direction, distance)
                RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
                bool grounded = groundInfo;
                if (groundInfo.collider == false)
                {
                    if (movingRight == true)
                    {
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        movingRight = false;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingRight = true;
                    }
                }
            }
            else
            {
                if (movingRight && Mathf.Abs(transform.position.x) - startPosX >= targetDistance)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }
                else if (!movingRight && Mathf.Abs(transform.position.x) - startPosX <= 0.0f)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
            }
            // Added in for death effect
        } else {
            Color c = GetComponent<SpriteRenderer>().color;
            c.a -= 0.1f;
            GetComponent<SpriteRenderer>().color = c;
            if (c.a <= 0.0f) {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            hp--;
        }
        if (collision.gameObject.tag == "Projectile")
        {
            hp -= 0.5f;
        }

        if (hp < 0.5f)
        {
            dead = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
