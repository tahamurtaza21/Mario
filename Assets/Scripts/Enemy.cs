using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float MoveSpeed = 1f;
    Animator enemyAnimator;
    BoxCollider2D boxCollider2D;

    Rigidbody2D rb2d;
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            rb2d.velocity = new Vector2(MoveSpeed, 0f);
            Debug.Log("Alive");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Die();
            isDead = true;
        }
    }

    void Die()
    {
        this.enabled = false;
        rb2d.velocity = Vector3.zero;
        boxCollider2D.enabled = false;
        enemyAnimator.SetBool("isRunning", false);
        enemyAnimator.SetTrigger("Die");
        Destroy(gameObject,1f);
    }
}
