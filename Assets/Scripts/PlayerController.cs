using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;
    Vector2 direction;
    [SerializeField] float speed;
    //[SerializeField] PlayerAttack playerAttack;
    [SerializeField] Collider2D attackCollider;
    CameraScript mainCamera;
    BlackBoard blackboard;

    //provisoir
    //[SerializeField] private Transform[] spawn;
    // [SerializeField] GameObject trap;
    // [SerializeField] int actualCooldown;
    // bool trapspawned;
    // CooldownTrap cooldownTrap;

    bool facingRight = false;
    bool facingLeft = true;
    float horizontalSpeed;
    Animator anim;

    int count;
    //provisoir

    // Start is called before the first frame update
    void Start()
    {
       body = GetComponent<Rigidbody2D>();
       // attackCollider.enabled = false;
       //cooldownTrap = FindObjectOfType<CooldownTrap>();
       anim = GetComponent<Animator>();
       mainCamera = FindObjectOfType<CameraScript>();
        //Debug.Log(camera.name);
       
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            mainCamera.FindPlayer(transform);
        }
        direction = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
        //if (Input.GetKeyDown("q"))
        //{
        //    attackCollider.enabled = true;
        //}
        horizontalSpeed = Input.GetAxis("Horizontal");

        //anim.SetFloat("speed", Mathf.Abs(horizontalSpeed * speed));
        //anim.SetFloat("speed", Mathf.Abs(Input.GetAxis("Vertical") * speed));

        if(Mathf.Abs(horizontalSpeed * speed) >= 0.01|| Mathf.Abs(Input.GetAxis("Vertical") * speed) >= 0.01)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (horizontalSpeed > 0 && !facingLeft)
        {

            facingLeft = true;
            facingRight = false;
            anim.transform.Rotate(0, 180, 0);
        }
        if (horizontalSpeed < 0 && !facingRight)
        {

            facingRight = true;
            facingLeft = false;
            anim.transform.Rotate(0, 180, 0);
        }


    }

    public Vector3 GivePosition()
    {
        Vector3 playerPosition = transform.position;
        return playerPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Rooms>())
        {
            Rooms room = collision.gameObject.GetComponent<Rooms>();
            Debug.Log("FDP");
            room.SpawnInNeighbours();
        }
    }
}
  
