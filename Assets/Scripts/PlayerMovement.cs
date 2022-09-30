using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement")]
    Vector2 moveInput;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 8f;
    //[SerializeField] float offsetY = 4f;

    [Header("Audio Clip")]
    [SerializeField] AudioClip Jump;
    [SerializeField] AudioClip DieSound;

    GameManager gameManager;

    float initY;

    BoxCollider2D boxCollider2D;
    Animator playerAnimator;

    bool isTouchingGround = false;
    bool isAlive = true;

    Rigidbody2D rb2d;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();
        initY = Camera.main.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            Move();
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && isTouchingGround)
        {
            rb2d.velocity += new Vector2(0f, jumpSpeed);
            playerAnimator.SetBool("isJumping", true);
            AudioSource.PlayClipAtPoint(Jump, Camera.main.transform.position, 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            isTouchingGround = true;
            playerAnimator.SetBool("isJumping", false);
        }

        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("dead");
            Die();
        }
    }


    
    void OnCollisionExit2D(Collision2D collision)
    {
        if(!boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            isTouchingGround = false;
        }
    }

    void Move()
    {
        Vector2 playervelocity = new Vector2(moveInput.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = playervelocity;

        //Vector3 CameraPosition = new Vector3(Mathf.Clamp(transform.position.x, ,), initY, -10f);

        Vector3 CameraPosition = new Vector3(transform.position.x, initY, -10f);

        //add logic to ensure when the platform ends and thus stop player from moving further

        Camera.main.transform.position = CameraPosition;

        if (moveInput.x > 0)
        {
            this.transform.localScale = new Vector2(1,1);
            playerAnimator.SetBool("isRunning", true);
        }
        else if (moveInput.x < 0)
        {
            this.transform.localScale = new Vector2(-1, 1);
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }

    void Die()
    {
        isAlive = false;
        this.enabled = false;
        Camera.main.GetComponent<AudioSource>().mute = true;
        AudioSource.PlayClipAtPoint(DieSound, Camera.main.transform.position, 1f);
        DieJump();

        boxCollider2D.enabled = false;
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("ded", true);
    }

    void DieJump()
    {
        Vector2 jumpDie = new Vector2(0f, 8f);

        rb2d.velocity = Vector3.zero;
        rb2d.AddForce(jumpDie,ForceMode2D.Impulse);
    }
}
