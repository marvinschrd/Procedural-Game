using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    BoxCollider2D collider;
    float colliderYSize = 1f;
    float colliderXSize = 1f;
    [SerializeField] float maxColliderSize = 3f;

    [SerializeField] float timeForRoomsToStop = 0;
    float timerForRooms = 4f;
    bool stopped = false;
    bool startTimer = false;

    List<Transform> aroundRoomsPosition;

    Transform closestRoom;

    [SerializeField] float circleCastRadius = 0;

    RoomSpawner RoomSpawner;
    bool canStart = false;

    List<Transform> neighbours;



    [SerializeField] GameObject wallBlockPrefab;
    bool placeWall = true;
    

    // Start is called before the first frame update
    void Start()
    {
        RoomSpawner = FindObjectOfType<RoomSpawner>();
        collider = GetComponent<BoxCollider2D>();
        neighbours = new List<Transform>();
        // aroundRoomsPosition = new List<Transform>();

        //colliderYSize = Random.Range(1f, maxColliderSize);
        //colliderXSize = Random.Range(1f, maxColliderSize);
        //collider = GetComponent<BoxCollider2D>();
        //collider.size = new Vector2(colliderXSize, colliderYSize);

        
    }

    enum Step
    {
        WAITING,
        GET_NEIGHBOURS,
        CHECK_COLLISIONS,
        DELETE_COMPONENTS,
        OPEN_WALLS
    }

    Step step = Step.WAITING;

    // Update is called once per frame
    void Update()
    {
        timerForRooms -= Time.deltaTime;
        if (placeWall && timerForRooms<=0)
        {
            placeWall = false;
            Destroy(collider);
            SpawnLeftWall();
            SpawnRightWall();
            SpawnUpWall();
            SpawnDownWall();
        }

        //switch(step)
        //{
        //    case Step.WAITING:
        //        if(canStart)
        //        {
        //            step = Step.GET_NEIGHBOURS;
        //        }
        //        break;
        //    case Step.GET_NEIGHBOURS:
        //        Vector2 colliderSize = new Vector2(collider.size.x, collider.size.y);
        //        circleCastRadius = colliderSize.magnitude;
        //        RaycastHit2D[] hits = Physics2D.CircleCastAll(gameObject.transform.position, circleCastRadius, Vector2.zero);
        //        foreach (RaycastHit2D hit in hits)
        //        {
        //            if (hit.collider != gameObject.GetComponent<BoxCollider2D>())
        //            {
        //                neighbours.Add(hit.transform);
        //            }
        //        }
        //        step = Step.CHECK_COLLISIONS;
        //        break;
        //    case Step.CHECK_COLLISIONS:
        //        // mettre en place la detection des points de collisions pour calculer le point ou ouvrir l'accès
        //        break;
        //}

        if (startTimer)
        {
            timerForRooms -= Time.deltaTime;
            if (timerForRooms <= 0)
            {
                //stopped = true;
                float newXPosition;
                float newYPosition;
                newXPosition = Mathf.Round(transform.position.x);
                newYPosition = Mathf.Round(transform.position.y);
                transform.position = new Vector3(newXPosition, newYPosition, 0);
                startTimer = false;
            }
        }
        //FixPosition();
        //float newXPosition;
        //float newYPosition;
        //newXPosition = Mathf.Round(transform.position.x);
        //newYPosition = Mathf.Round(transform.position.y);
        //transform.position = new Vector3(newXPosition, newYPosition, 0);

        //if (stopped)
        //{
            
        //}
    }

   

    void GetClosestEnemy()
    {
        stopped = false;
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in aroundRoomsPosition)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
               closestRoom = potentialTarget;
            }
        }
        
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(transform.position, closestRoom.position);
    //}


        void SpawnLeftWall()
    {
        if (wallBlockPrefab != null)
        {
            float blockSize = wallBlockPrefab.transform.localScale.x;
            float roomHeight = transform.localScale.y ;
            float roomWidth = transform.localScale.x ;

            float numberOfWallToSpawn = roomHeight ;

            Debug.Log("room position" + transform.position.x);

            for (int i = 0; i < numberOfWallToSpawn*2+1; i++)
            {
               
                Instantiate(wallBlockPrefab, new Vector3(transform.position.x - (roomWidth/2), transform.position.y+(roomHeight/2) + i * (-0.5f), -1), Quaternion.identity);
               // wallBlockPrefab.transform.parent = gameObject.transform;
            }
                Debug.Log(transform.position.x - (roomWidth / 2));
        }

        
        
    }

    void SpawnRightWall()
    {
        if (wallBlockPrefab != null)
        {
            float blockSize = wallBlockPrefab.transform.localScale.x;
            float roomHeight = transform.localScale.y;
            float roomWidth = transform.localScale.x;

            float numberOfWallToSpawn = roomHeight ;

            Debug.Log("room position" + transform.position.x);

            for (int i = 0; i < numberOfWallToSpawn *2+1 ; i++)
            {
                
               Instantiate(wallBlockPrefab, new Vector3(transform.position.x + (roomWidth / 2), transform.position.y + (roomHeight / 2) + i * (-0.5f), -1), Quaternion.identity);
                wallBlockPrefab.transform.parent = gameObject.transform;
            }
            Debug.Log(transform.position.x - (roomWidth / 2));
        }
    }

    void SpawnUpWall()
    {
        if (wallBlockPrefab != null)
        {
            float blockSize = wallBlockPrefab.transform.localScale.x;
            float roomHeight = transform.localScale.y;
            float roomWidth = transform.localScale.x;

            float numberOfWallToSpawn = roomWidth;

            Debug.Log("room position" + transform.position.x);

            for (int i = 0; i < numberOfWallToSpawn * 1f + 1; i++)
            {
                
                Instantiate(wallBlockPrefab, new Vector3(transform.position.x + (roomWidth / 2) + i * (-1f), transform.position.y + (roomHeight / 2), -1), Quaternion.identity);
                wallBlockPrefab.transform.parent = gameObject.transform;
            }
            Debug.Log(transform.position.x - (roomWidth / 2));
        }
    }

    void SpawnDownWall()
    {
        if (wallBlockPrefab != null)
        {
            float blockSize = wallBlockPrefab.transform.localScale.x;
            float roomHeight = transform.localScale.y;
            float roomWidth = transform.localScale.x;

            float numberOfWallToSpawn = roomWidth;

            Debug.Log("room position" + transform.position.x);

            for (int i = 0; i < numberOfWallToSpawn * 1 + 1; i++)
            {
                
               Instantiate(wallBlockPrefab, new Vector3(transform.position.x + (roomWidth / 2) + i * (-1f), transform.position.y - (roomHeight / 2), -1), Quaternion.identity);
                wallBlockPrefab.transform.parent = gameObject.transform;
            }
            Debug.Log(transform.position.x - (roomWidth / 2));
        }
    }

    
    public void FixPosition()
    {
        startTimer = true;
       
        
    }
}
