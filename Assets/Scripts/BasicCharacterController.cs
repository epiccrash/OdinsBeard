using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacterController : MonoBehaviour {

    // Life variables
    public int hp;
    public int lives;
    public bool alive;

    // Private component variables
    private Collider2D c;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator animator;

    // Variables to control animnation
    private bool isFacingRight;
    private bool spriteCanChange;

    // Variable controlling horizontal movement
    private float hAxis;

    // Main camera
    public GameObject mainCam;

    // Variables for platform colliders
    public Collider2D tilemapCollider;
    public Collider2D platformCollider;

    // Bools used for jumping logic
    private bool grounded;
    private bool isJumping;

    // Use?
    private float jumpTimeCounter = 0.0f;
    public float maxJumpTime = 0.5f;

    // Speed variable
    public float speed = 7.0f;
    // These control the jump
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    public float jumpTakeOffSpeed = 7.0f;

    // Import the attacking script
    private PlayerAttackController attackController;

    // 
    private void Awake() {
        c = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {

        hp = 3;
        lives = 3;
        alive = true;

        mainCam = GameObject.Find("Main Camera");

        attackController = GetComponent<PlayerAttackController>();

        isFacingRight = true;
        spriteCanChange = true;

        tilemapCollider = GameObject.Find("Tilemap").GetComponent<Collider2D>();

        grounded = true;
        isJumping = false;
	}

    // Update is called once per frame
    void FixedUpdate() {

        hAxis = Input.GetAxis("Horizontal");
        
        float pX = transform.position.x;
        float pY = transform.position.y;

        mainCam.transform.position = new Vector3(
            Mathf.Clamp(pX, -16.6f, 60.0f),
            pY + 1.48f, -10.0f);

        if (Input.GetButtonDown("Jump")) {
            rb.velocity = Vector2.up * jumpTakeOffSpeed;
            isJumping = true;
        }

        if (rb.velocity.y < 0 && !isJumping)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y
                * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y < 0 && isJumping) {
            isJumping = false;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y
                * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        rb.velocity = new Vector2(hAxis * speed, rb.velocity.y);

        animator.SetFloat("speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        animator.SetBool("grounded", grounded);
        animator.SetBool("alive", alive);
        animator.SetBool("attacking", attackController.attacking);
        
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
        } else {
            sp.flipX = false;
        }

        if (!attackController.attacking && !attackController.holding)
        {
            spriteCanChange = true;
        } else {
            spriteCanChange = false;
        }

        if (hAxis < 0.2 && hAxis < -0.2)
        {
            // deadband logic
        }
    }

    public void Heal() {
        if (hp < 3)
        {
            hp++;
        }
    }

    // Does nothing
    public void TakeDamage() {
        hp--;
        if (hp == 0)
        {
            lives--;
            Respawn();
        }

        if (lives == 0)
        {
            GameOver();
        }
    }

    // Does nothing
    public void GameOver() {

    }

    public void Respawn() {

    }
}
