using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D body;
    Vector2 direction;
    [SerializeField] float speed;

    bool facingRight = false;
    bool facingLeft = true;
    float horizontalSpeed;
    Animator anim;

    Transform player;
    bool follow = false;

    float xVelocity = 0;
    float yVelocity = 0;

    [SerializeField]  GameObject playablePrefab;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }
    // Update is called once per frame
    void Update()
    {
        if(follow)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            body.velocity = (player.position - transform.position).normalized * speed;
        }
        xVelocity = body.velocity.x;
        yVelocity = body.velocity.y;
        //Debug.Log(body.velocity);
        if((Mathf.Abs(xVelocity * speed) >= 0.01)|| (Mathf.Abs(yVelocity * speed) >= 0.01))
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (xVelocity> 0 && !facingLeft)
        {

            facingLeft = true;
            facingRight = false;
            anim.transform.Rotate(0, 180, 0);
        }
        if (xVelocity < 0 && !facingRight)
        {

            facingRight = true;
            facingLeft = false;
            anim.transform.Rotate(0, 180, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.transform;
            follow = true;
        }
    }

    public void InstatiatePlayablePrefab()
    {
        Instantiate(playablePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
