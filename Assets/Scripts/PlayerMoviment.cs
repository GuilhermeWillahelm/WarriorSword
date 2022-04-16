using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D collider2D;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(6, 6, 6);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-6, 6, 6);

        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());

        if(wallJumpCooldown < 0.2f)
        {
            

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 3;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime; ;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            animator.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())
        {
            if (horizontalInput == 0 )
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCooldown = 0;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
