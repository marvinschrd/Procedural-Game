using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarController : MonoBehaviour
{
    AI.PathFinder PathFinder;
    EnemyAttack EnemyAttack;
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
    int randomNumber = 0;
    Vector3 randomPosition;
    bool choseAnotherRandomPosition = true;
    float recalculationTimer = 3;
    PlayerController player;

    bool detectedTarget = false;
    float playerDistance = 0;

    [SerializeField] float attackTime;
    float attackTimer;

    BlackBoard blackBoard;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        wayPoints = new List<AI.WayPoint>();
        path = new List<Vector3>();
        PathFinder = FindObjectOfType<AI.PathFinder>();
       // player = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
        EnemyAttack = GetComponent<EnemyAttack>();
        attackTimer = attackTime;
      //  blackBoard = FindObjectOfType<BlackBoard>();
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
       // Debug.Log(state);
        switch(state)
        {
            case State.IDLE:
                detectedTarget = false;
                body.velocity = new Vector2(0f,0f);
                break;
            case State.PATROLING:
                speed = 1;
                Debug.Log("PATROLING");
                if(choseAnotherRandomPosition)
                {
                    Debug.Log("getting random path");
                    randomPosition = (Vector3)Random.insideUnitCircle * 3 + transform.position;
                    //Debug.Log("random position =" + randomPosition);
                    GetPath(randomPosition);
                    choseAnotherRandomPosition = false;
                }
                GoToTarget(path[0]);
                float patrolDistance = PathFinder.ManhattanDistance(transform.position,path[0]);
                if(patrolDistance<=0.1f)
                {
                    Debug.Log("already at point");
                    path.RemoveAt(0);
                }
                if (path.Count<=2)
                {
                    choseAnotherRandomPosition = true;
                }
                break;
            case State.GETPATH:
                path = PathFinder.GetPath(transform.position, targetPosition);
                pathIndex = 0;
                state = State.FOLLOWING;
                break;
            case State.FOLLOWING:
                speed = 2;
                GoToTarget(path[0]);
                float distance = PathFinder.ManhattanDistance(transform.position, path[0]);
                if(distance<=0.1f&&path.Count>=2)
                {
                    path.RemoveAt(0);
                    Debug.Log("path count = " + path.Count);
                }
                recalculationTimer -= Time.deltaTime;
                if(recalculationTimer<=0)
                {
                    path = PathFinder.GetPath(transform.position, player.GivePosition());
                    recalculationTimer = 2;
                    playerDistance = PathFinder.ManhattanDistance(transform.position, player.GivePosition());
                }
                if(path.Count==1)
                {
                    Debug.Log("GOT TO ZERO");
                    path = PathFinder.GetPath(transform.position, player.GivePosition());
                }
                if(playerDistance>=10f)
                {
                    choseAnotherRandomPosition = true;
                    state = State.PATROLING;
                }
                if(playerDistance<=1f)
                {
                    Debug.Log("YOOOOOOOOO");
                    state = State.ATTACKING;
                }
                
                if (player==null)
                {
                    Debug.Log("PLAYER NOT HERE");
                    choseAnotherRandomPosition = true;
                    player = FindObjectOfType<PlayerController>();
                    detectedTarget = false;
                    state = State.PATROLING;
                }
                break;
            case State.ATTACKING:
                // Debug.Log("ATTACKING");
                Debug.Log("close enough");
                body.velocity = (player.GivePosition() - transform.position).normalized * speed;
                playerDistance = PathFinder.ManhattanDistance(transform.position, player.GivePosition());
                if (playerDistance >= 1f)
                {
                    path = PathFinder.GetPath(transform.position, player.GivePosition());
                    state = State.FOLLOWING;
                }
               
               // StopMovement();
                //attackTimer -= Time.deltaTime;
                //if(attackTimer<=0)
                //{
                //    EnemyAttack.Attack();
                //    attackTimer = attackTime;
                //}

                break;
        }
        if (player == null)
        {
            Debug.Log("PLAYER NOT HERE");
            choseAnotherRandomPosition = true;
            player = FindObjectOfType<PlayerController>();
            detectedTarget = false;
            state = State.PATROLING;
        }
        //Debug.Log("XVELOCITY =" + xVelocity);
        if ( xVelocity * speed !=0 || yVelocity * speed !=0)
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
        Debug.Log("GO TO TARGET");
        //Debug.Log("MOVE TO TARGET");
        //Debug.Log("TARGET =" + targetPosition);
         body.velocity = (targetPosition - transform.position).normalized * speed ;
        xVelocity = body.velocity.x;
        yVelocity = body.velocity.y;
        //Debug.Log("BODY VELOCITY = " + body.velocity);
        //direction = new Vector2(path[pathIndex].x * speed, path[pathIndex].y * speed);
      //  transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
        //Debug.Log(direction);
        //Debug.Log(direction.x);
      // Debug.Log("BODY VELOCITY =" + body.velocity);
    }

    void GetPath(Vector3 targetPosition)
    {
        path = PathFinder.GetPath(transform.position, targetPosition);
    }

    void StopMovement()
    {
       body.velocity = new Vector2(path[pathIndex].x * 0, path[pathIndex].y * 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")&&!detectedTarget)
        {
            player = collision.GetComponent<PlayerController>();
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
