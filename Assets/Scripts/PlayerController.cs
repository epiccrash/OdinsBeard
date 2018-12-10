using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : PhysicsObject
{
    public int hp = 3;
    public int lives = 3;
    public bool alive = true;
    private bool isHit = false;
    private bool falling;

    private SpriteRenderer sp;
    public Animator animator;

    public GameObject mainCam;

    public bool isFacingRight;
    private bool spriteCanChange;

    public float jumpTakeOffSpeed = 7;
    public float maxSpeed = 7;
    public float jumpDecrease = 0.5f;

    private float hAxis;

    private PlayerAttackController attackController;

    private Rigidbody2D rb;

    private Vector3 respawnPosition;

    public float minCamX = -16.6f;
    public float maxCamX = 60.0f;

    public float minCamY = -5.0f;

    public float maxInvulnTime = 3.0f;
    private float invulnTimer = 0.0f;

    private bool invulnerable;
    private bool pinwheeling;

    public GameObject hp1;
    public GameObject hp2;
    public GameObject hp3;
    public GameObject livesText;

    public LevelEnd LevelEndScript;

    private int invisTimer;

    private bool isGrounded;

    // Use this for initialization
    void Start ()
    {
        hp = 3;
        lives = 3;
        alive = true;
        isHit = false;

        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        mainCam = GameObject.Find("Main Camera");

        attackController = GetComponent<PlayerAttackController>();
        
        isFacingRight = true;
        spriteCanChange = true;
        respawnPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();

        invulnerable = false;

        pinwheeling = false;
        invisTimer = 0;

        isGrounded = true;
    }

    // Tap Jump activates falling animation but player doesn't immediately break out of it
    // 
    // 

    protected override void ComputeVelocity()
    {
        //Code to act in "Update:
        if (invisTimer != 0)
        {
            invisTimer--;
            if (invisTimer == 0)
            {
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        // base.ComputeVelocity();
        Vector2 move = Vector2.zero;

        hAxis = Input.GetAxis("Horizontal");

        float pX = transform.position.x;
        float pY = transform.position.y;

        mainCam.transform.position = new Vector3(
            Mathf.Clamp(pX, minCamX, maxCamX),
            pY + 1.48f, -10.0f);

        if (hAxis != 0)
        {
            // Flip sprites and direction
            if (hAxis < 0 && !attackController.attacking && spriteCanChange)
            {
                isFacingRight = false;
            }
            if (hAxis > 0 && !attackController.attacking && spriteCanChange)
            {
                isFacingRight = true;
            }

            if (isFacingRight)
            {
                sp.flipX = true;
            }
            else
            {
                sp.flipX = false;
            }

            if (!attackController.attacking && !attackController.holding)
            {
                spriteCanChange = true;
            }
            else
            {
                spriteCanChange = false;
            }

            if (hAxis < 0.2 && hAxis < -0.2)
            {
                // deadband logic
            }

            move.x = hAxis;
        }

        /* Note to Joey:
         * I just moved this code block out of the deadband logic.
         * I don't really know why it was in there, but the player couldn't
         * jump unless they were moving. Let me know if it should be in there.
         * --Nathan
         */

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            SoundManager.S.MakePlayerJump();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            // Reduce velocity if player stops holding jump.
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * jumpDecrease;
                falling = true;
            }
        }
        /* End of code block I moved. */

        //targetVelocity = move * maxSpeed;

        targetVelocity = move * maxSpeed;

        if (velocity.y <= jumpTakeOffSpeed / 2)
        {
            falling = true;
        }

        if (grounded) {
            falling = false;
        }

        // Invulnerable code
        if (invulnerable)
        {
            Color col = sp.color;

            if (sp.color.a < 1.0f) {
                col.a = 1.0f;
            } else {
                col.a = 0.5f;
            }
            sp.color = col;

            invulnTimer += Time.deltaTime;
            if (invulnTimer >= maxInvulnTime / 2) {
                isHit = false;
            }
            if (invulnTimer >= maxInvulnTime)
            {
                invulnTimer = 0.0f;
                invulnerable = false;
                col.a = 1.0f;
                sp.color = col;
            }
        }

        if (attackController.pinwheeling) {
            pinwheeling = true;
        } else {
            pinwheeling = false;
        }

        animator.SetBool("alive", alive);
        animator.SetBool("attacking", attackController.attacking || attackController.holding);
        animator.SetBool("pinwheeling", pinwheeling);
        //animator.SetBool("attacking", attackController.attacking);
        animator.SetBool("pinwheeling", attackController.pinwheeling);
        animator.SetBool("falling", falling);
        animator.SetBool("grounded", grounded);
        animator.SetBool("hit", isHit);
        animator.SetFloat("speed", Mathf.Abs(Input.GetAxis("Horizontal")));
    }

    public void setHPState(bool activeState) {
        if (hp == 1) {
            hp1.SetActive(activeState);
        }
        else if (hp == 2)
        {
            hp2.SetActive(activeState);
        }
        else
        {
            hp3.SetActive(activeState);
        }
    }

    public void Heal() {
        if (hp < 3) {
            hp++;
            setHPState(true);
        }
    }

    public void TakeDamage() {

        setHPState(false);
        hp--;
        if (hp == 0) {
            lives--;
            livesText.GetComponent<Text>().text = 
                livesText.GetComponent<Text>().text[0] + lives.ToString();
            hp = 3;
            hp1.SetActive(true);
            hp2.SetActive(true);
            hp3.SetActive(true);
            transform.position = respawnPosition;
            LevelEndScript.animateRespawn();
        }

        if (lives == 0) {
            GameOver();
        }
    }

    // Does nothing
    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !invulnerable && !pinwheeling)
        {
            //Debug.Log("Player taking Damage");
            TakeDamage();
            invulnerable = true;


            /*Vector3 dir = new Vector3(collision.contacts[0].point.x,
                                     collision.contacts[0].point.x,
                                      transform.position.z) - transform.position;
            dir = -dir.normalized;


            if (isFacingRight) rb.AddForce(dir * 3);
            else rb.AddForce(dir * -3);*/

            if (isFacingRight) rb.AddForce(8 * Vector3.left);
            else rb.AddForce(8 * Vector3.right);

            isHit = true;
        }

        if (collision.gameObject.tag == "Bound")
        {
            //Debug.Log("Player touched bounds");
            LevelEndScript.animateFall();
            transform.position = respawnPosition;
            TakeDamage();
            invisTimer = 20;
            GetComponent<SpriteRenderer>().enabled = false;
        }

        if (collision.gameObject.tag == "Level Map") {
            isGrounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Level Map")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Level Map")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemySword") 
            && !invulnerable && !pinwheeling)
        {
            TakeDamage();
            invulnerable = true;

        }

    }

    public void setCheckpoint(Vector3 position)
    {
        if (position.x > respawnPosition.x)
        {
            //Debug.Log("Player checkpoint updated");
            respawnPosition = position;
        }
    }

    /*
    public void SetAnimationState() {

        if (hAxis == 0.0f) {
            animator.SetBool("isWalking", false);
        }

        if (rb.velocity.y == 0.0f) {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        if (Mathf.Abs(hAxis) > 0.0f && rb.velocity.y == 0.0f) {
            animator.SetBool("isWalking", true);
        }

        if (rb.velocity.y > 0.0f) {
            animator.SetBool("isJumping", true);
        }

        if (rb.velocity.y < 0.0f) {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
    }*/
}
