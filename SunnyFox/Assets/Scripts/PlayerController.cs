using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    private Animator anime;
    public LayerMask ground;
    public Collider2D coll;
    public Transform groundCheck;
    public float speed, jumpForce;
    public int cherryNum;
    public bool isGround, isJump;
    bool jumpPressed;
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
    }
    // Update is called once per frame
    // 利用FixedUpdate优化性能
    // Update runs once per frame. FixedUpdate can run once, zero, or several times per frame, 
    // depending on how many physics frames per second are set in the time settings, and how fast/slow the framerate is.
    // FixedUpdate is used for being in-step with the physics engine, so anything that needs to be applied to a rigidbody should happen in FixedUpdate. 
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        GroundMovement();
        Jump();
        SwitchAnime();
    }

    void GroundMovement() 
    {
        // 返回-1，0，1。-1是左，0是没动，1是右
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }
    void SwitchAnime()
    {
        anime.SetFloat("running", Mathf.Abs(rb.velocity.x));
        if (isGround)
        {
            anime.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anime.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anime.SetBool("jumping", false);
            anime.SetBool("falling", true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            cherryNum += 1;
        }
    }
}
