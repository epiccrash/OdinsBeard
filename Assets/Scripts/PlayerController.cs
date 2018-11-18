using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : PhysicsObject
{
    public int hp;
    public int lives;
    public bool alive;
    private bool isHit;
    private bool falling;

    private SpriteRenderer sp;
    private Animator animator;

    public GameObject mainCam;

    private bool isFacingRight;
    private bool spriteCanChange;

    public float jumpTakeOffSpeed = 7;
    public float maxSpeed = 7;
    public float jumpDecrease = 0.5f;

    private float hAxis;

    private PlayerAttackController attackController;

    private Rigidbody2D rb;

    private Vector3 respawnPosition;

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

        rb = GetComponent<Rigidbody2D>();
	}

    // Tap Jump activates falling animation but player doesn't immediately break out of it
    // 
    // 

    protected override void ComputeVelocity()
    {
        // base.ComputeVelocity();
        Vector2 move = Vector2.zero;

        hAxis = Input.GetAxis("Horizontal");

        float pX = transform.position.x;
        float pY = transform.position.y;

        mainCam.transform.position = new Vector3(
            Mathf.Clamp(pX, -16.6f, 60.0f),
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

            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
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

            targetVelocity = move * maxSpeed;
        }

        if (velocity.y <= jumpTakeOffSpeed / 2)
        {
            falling = true;
        }

        if (grounded) {
            falling = false;
        }

        animator.SetBool("alive", alive);
        animator.SetBool("attacking", attackController.attacking);
        animator.SetBool("falling", falling);
        animator.SetBool("grounded", grounded);
        //animator.SetBool("hit", isHit);
        animator.SetFloat("speed", Mathf.Abs(Input.GetAxis("Horizontal")));
    }

    public void Heal() {
        if (hp < 3) {
            hp++;
        }
    }

    // Does nothing
    public void TakeDamage() {

        hp--;
        if (hp == 0) {
            lives--;
            Respawn();
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
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player taking Damage");
            TakeDamage();
        }

        if (collision.gameObject.tag == "Bound")
        {
            Debug.Log("Player touched bounds");
            transform.position = respawnPosition;
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage();
        }

    }

    public void setCheckpoint(Vector3 position)
    {
        if (position.x > respawnPosition.x)
        {
            Debug.Log("Player checkpoint updated");
            respawnPosition = position;
        }
    }

    public void Respawn() {

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
