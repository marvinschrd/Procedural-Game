using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarController : MonoBehaviour
{
    AI.PathFinder PathFinder;
    List<AI.WayPoint> wayPoints;
    List<AI.WayPoint> roomWaypoints;
    Vector3 targetPosition;
    List<Vector3> path;

    Rigidbody2D body;
    Vector2 direction;
    float xVelocity = 0;
    float yVelocity = 0;
    bool facingRight = false;
    bool facingLeft = true;
    [SerializeField] float speed;

    int pathIndex;

    float recalculationTimer = 3;
    PlayerController player;

    bool detectedTarget = false;
    float playerDistance = 0;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        wayPoints = new List<AI.WayPoint>();
        path = new List<Vector3>();
        PathFinder = FindObjectOfType<AI.PathFinder>();
        player = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
        roomWaypoints = new List<AI.WayPoint>();
        wayPoints = PathFinder.GiveWaypoints();
        foreach(AI.WayPoint wayPoint in wayPoints )
        {
            if(wayPoint.gameObject.CompareTag(gameObject.tag))
            {
                roomWaypoints.Add(wayPoint);
            }
        }

    }
    private void FixedUpdate()
    {
       // body.velocity = new Vector2(-direction.x * speed * Time.fixedDeltaTime , -direction.y *speed*Time.fixedDeltaTime);
    }

    enum State
    {
        IDLE,
        PATROLING,
        FOLLOWING,
        ATTACKING,
        GETPATH
    }
    State state = State.PATROLING;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(state);
        switch(state)
        {
            case State.IDLE:
                detectedTarget = false;
                body.velocity = new Vector2(0f,0f);
                break;
            case State.PATROLING:
               


                break;
            case State.GETPATH:
                path = PathFinder.GetPath(transform.position, targetPosition);
                pathIndex = 0;
                state = State.FOLLOWING;
                break;
            case State.FOLLOWING:
                GoToTarget(path[0]);
                float distance = PathFinder.ManhattanDistance(transform.position, path[0]);
                if(distance<=0.1f)
                {
                    path.RemoveAt(0);
                }
                recalculationTimer -= Time.deltaTime;
                if(recalculationTimer<=0)
                {
                    path = PathFinder.GetPath(transform.position, player.GivePosition());
                    recalculationTimer = 3;
                    playerDistance = PathFinder.ManhattanDistance(transform.position, player.GivePosition());
                }
                if(path.Count==0)
                {
                    path = PathFinder.GetPath(transform.position, player.GivePosition());
                }
                if(playerDistance>=5f)
                {
                    state = State.IDLE;
                }
                break;
            case State.ATTACKING:
                StopMovement();
                break;
        }
        //Debug.Log("XVELOCITY =" + xVelocity);
        if( xVelocity * speed !=0 || yVelocity * speed !=0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (xVelocity > 0 && !facingLeft)
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

    void GoToTarget(Vector3 targetPosition)
    {
        //Debug.Log("MOVE TO TARGET");
        //Debug.Log("TARGET =" + targetPosition);
         body.velocity = (targetPosition - transform.position).normalized * speed ;
        xVelocity = body.velocity.x;
        yVelocity = body.velocity.y;
        //direction = new Vector2(path[pathIndex].x * speed, path[pathIndex].y * speed);
      //  transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
        Debug.Log(direction);
        Debug.Log(direction.x);
      // Debug.Log("BODY VELOCITY =" + body.velocity);
    }

    void StopMovement()
    {
        direction = new Vector2(path[pathIndex].x * 0, path[pathIndex].y * 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")&&!detectedTarget)
        {
            targetPosition = collision.transform.position;
            state = State.GETPATH;
            detectedTarget = true;
        }
    }

    private void OnDrawGizmos()
    {
        if(path!=null)
        {
            foreach (Vector3 waypoints in path)
            {
                Gizmos.DrawWireSphere(waypoints, 0.1f);
            }
        }
    }

    //public void InstatiatePlayablePrefab()
    //{
    //    Instantiate(playablePrefab, transform.position, Quaternion.identity);
    //    Destroy(gameObject);
    //}
}
