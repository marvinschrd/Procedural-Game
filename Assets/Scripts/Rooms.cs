using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    BoxCollider2D collider;
    PolygonCollider2D polyCollider;
    Rigidbody2D body;
    float colliderYSize = 1f;
    float colliderXSize = 1f;
    [SerializeField] float maxColliderSize = 3f;

    [SerializeField] float timeForRoomsToStop = 0;
    float timerForRooms = 6f;
    float timerForPositionFix = 2f;
    bool stopped = false;
    bool startTimer = false;

    List<Transform> aroundRoomsPosition;

    Transform closestRoom;

    [SerializeField] float circleCastRadius = 0;

    RoomSpawner RoomSpawner;
    bool canStart = false;

    List<Transform> neighbours;



    [SerializeField] GameObject wallBlockPrefab;
    [SerializeField] GameObject[] DifferentWallBlocks;
    bool placeWall = true;


    List<ContactPoint2D> contacts;
    List<Vector2> inBetweenPoints;
    bool getContacts = true;

    // Start is called before the first frame update
    void Start()
    {
       // Debug.Break();


        RoomSpawner = FindObjectOfType<RoomSpawner>();
        collider = GetComponent<BoxCollider2D>();
        polyCollider = GetComponent<PolygonCollider2D>();
        //body = GetComponent<Rigidbody2D>();
        //polyCollider.enabled = false;
        neighbours = new List<Transform>();

        contacts = new List<ContactPoint2D>();
        inBetweenPoints = new List<Vector2>();
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
        BUILD_WALLS,
        OPEN_WALLS,
        END
    }

    Step step = Step.WAITING;

    // Update is called once per frame
    void Update()
    {
        //timerForRooms -= Time.deltaTime;
        //if (placeWall && timerForRooms<=0)
        //{
        //    placeWall = false;
           
        //    SpawnUpWall();
        //    SpawnLeftWall();
        //    SpawnRightWall();
        //    SpawnDownWall();
        //}
        //if (startTimer)
        //{
        //        Debug.Log("FIXED_POSITION");
        //    timerForPositionFix -= Time.deltaTime;
        //    if (timerForPositionFix<=0&&!stopped)
        //    {
        //        //stopped = true;
        //        float newXPosition;
        //        float newYPosition;
        //        newXPosition = Mathf.Round(transform.position.x);
        //        newYPosition = Mathf.Round(transform.position.y);
        //        transform.position = new Vector3(newXPosition, newYPosition, 0);
        //        Destroy(body);
        //        stopped = true;
        //        startTimer = false;
        //    }
        //}

        switch (step)
        {
            case Step.WAITING:
               
                if (canStart)
                {
                    step = Step.GET_NEIGHBOURS;
                }
                break;
            case Step.GET_NEIGHBOURS:
                collider.enabled = false;
                Vector2 colliderSize = new Vector2(collider.size.x, collider.size.y);
                circleCastRadius = colliderSize.magnitude;
                RaycastHit2D[] hits = Physics2D.CircleCastAll(gameObject.transform.position, circleCastRadius, Vector2.zero);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != gameObject.GetComponent<BoxCollider2D>())
                    {
                        neighbours.Add(hit.transform);
                    }
                }
                step = Step.CHECK_COLLISIONS;
                break;
            case Step.CHECK_COLLISIONS:
                
                // mettre en place la detection des points de collisions pour calculer le point ou ouvrir l'accès
                if (getContacts)
                {
                    
                   polyCollider.GetContacts(contacts);
                    //Vector2 test = (new Vector2(-1.45f, 0.7f) + new Vector2(-1.7f, 0.7f)) / 2;
                    //Vector2 test2 = (new Vector2(-0.9f, 0.7f).normalized + new Vector2(-0.7f, 0.7f).normalized).normalized;
                    //Debug.Log("TEST =" + test);
                    //Debug.Log("TEST2 =" + test2);
                    //Debug.Log(contacts.Count);
                    //Debug.Log(contacts[0].collider.transform.name);
                    //Debug.Log(contacts[1].collider.transform.name);
                  //  Debug.Log(contacts[2].collider.transform.name + "x =" + contacts[2].point.x + "y=" + contacts[2].point.y);
                    //Debug.Log(contacts[3].collider.transform.name + "x =" + contacts[3].point.x + "y=" + contacts[3].point.y);
                    //Debug.Log(contacts[3].collider.transform.name);

                    while (contacts.Count > 0)
                    {
                       // Debug.Log(contacts[0].collider.transform.name);
                        Vector2 firstPointPosition = new Vector2(contacts[0].point.x, contacts[0].point.y);
                       // Debug.Log("first point position = " + firstPointPosition);
                        //Debug.Log(contacts[1].point.x + contacts[1].point.y);
                        Vector2 secondPointPosition = new Vector2(contacts[1].point.x, contacts[1].point.y);
                       // Debug.Log("second point position =" + secondPointPosition);
                        // Vector2 inBetweenPointPosition = (firstPointPosition.normalized+ secondPointPosition.normalized).normalized;
                        Vector2 inBetweenPointPosition = (firstPointPosition + secondPointPosition) / 2;
                        inBetweenPoints.Add(inBetweenPointPosition);
                        contacts.RemoveAt(0);
                        contacts.RemoveAt(0);
                        //Debug.Log("POINT 0" +contacts[0].point.x + contacts[0].point.y);
                        //Debug.Log("POINT 1"+contacts[1].point.x + contacts[1].point.y);
                        //Debug.Log("inbetween point =" +inBetweenPoints[0]);
                        //maintenant, utiliser la liste de points entre deux pour dessiner des gizmos;
                    }
                    getContacts = false;
                    DrawCircles();
                    //WallOpening();
                    Destroy(body);
                }
                step = Step.BUILD_WALLS;
                break;
            case Step.BUILD_WALLS:
               
                    DestroyRoomsWithNoConnections();
                if (placeWall)
                {
                    placeWall = false;
                    Destroy(body);
                    SpawnUpWall();
                    SpawnLeftWall();
                    SpawnRightWall();
                    SpawnDownWall();
                }
                step = Step.OPEN_WALLS;
                break;
            case Step.OPEN_WALLS:
                Destroy(polyCollider);
                Destroy(collider);
                WallOpening();
                EnnemiesSpawner ennemies = GetComponent<EnnemiesSpawner>();
                ennemies.ActivateSpawn();
                step = Step.END;
                break;
            case Step.END:

                break;
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

           // Debug.Log("room position" + transform.position.x);

            for (int i = 0; i < numberOfWallToSpawn*2; i++)
            {
               
                Instantiate(wallBlockPrefab, new Vector3(transform.position.x - (roomWidth/2 - 0.475f), transform.position.y+(roomHeight/2) + i * (-0.5f), -1), Quaternion.identity);
               // wallBlockPrefab.transform.parent = gameObject.transform;
            }
               // Debug.Log(transform.position.x - (roomWidth / 2));
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

           // Debug.Log("room position" + transform.position.x);

            for (int i = 0; i < numberOfWallToSpawn *2 ; i++)
            {
                
               Instantiate(wallBlockPrefab, new Vector3(transform.position.x + (roomWidth / 2-0.475f), transform.position.y + (roomHeight / 2) + i * (-0.5f), -1), Quaternion.identity);
               // wallBlockPrefab.transform.parent = gameObject.transform;
            }
            //Debug.Log(transform.position.x - (roomWidth / 2));
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

            //Debug.Log("room position" + transform.position.x);

            for (int i = 0; i < numberOfWallToSpawn -2 ; i++)
            {

                int index = Random.Range(0, DifferentWallBlocks.Length);

                
                Instantiate(DifferentWallBlocks[index], new Vector3(transform.position.x + (roomWidth / 2-1.5f) + i * (-1f), transform.position.y + (roomHeight / 2), -1), Quaternion.identity);
               // wallBlockPrefab.transform.parent = gameObject.transform;
            }
           // Debug.Log(transform.position.x - (roomWidth / 2));
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

            //Debug.Log("room position" + transform.position.x);

            for (int i = 0; i < numberOfWallToSpawn -2 ; i++)
            {
                
               Instantiate(wallBlockPrefab, new Vector3(transform.position.x + (roomWidth / 2-1.5f) + i * (-1f), transform.position.y - (roomHeight / 2)+0.475f, -1), Quaternion.identity);
                //wallBlockPrefab.transform.parent = gameObject.transform;
            }
           // Debug.Log(transform.position.x - (roomWidth / 2));
        }
    }

    
    public void FixPosition(float additionalTime)
    {
       // timerForPositionFix += additionalTime;
        Debug.Log(timerForPositionFix);
        startTimer = true;
        body = GetComponent<Rigidbody2D>();
        if (body == null)
        {
            Debug.Log("ERROR");
        }
        canStart = true;
        //body.bodyType = RigidbodyType2D.Static;
        //Debug.Log(gameObject.name + transform.position);
        //float newXPosition;
        //float newYPosition;
        //newXPosition = Mathf.Round(transform.position.x);
        //newYPosition = Mathf.Round(transform.position.y);
        //transform.position = new Vector3(newXPosition, newYPosition, 0);
        //Debug.Log("IN ROOM");
        
    }

    void DrawCircles()
    {

        for (int i = 0; i < inBetweenPoints.Count; i++)
        {
            //Gizmos.DrawWireSphere(inBetweenPoints[i], 0.5f);
            //Gizmos.color = Color.black;
            Debug.Log("inbetween point" + inBetweenPoints[i]);
        }
    }

    void WallOpening()
    {
        for (int i = 0; i < inBetweenPoints.Count; i++)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(inBetweenPoints[i], 0.4f, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                //Debug.Log("NAME IS = " +hit.collider.gameObject.name);
                //if (hit.collider != gameObject.GetComponent<BoxCollider2D>())
                //{
                //    Destroy(hit.collider.transform.gameObject);
                //}
                if (hit.collider.gameObject.CompareTag("Walls"))
                {
                    Destroy(hit.collider.transform.gameObject);
                }
            }
        }
    }

    void DestroyRoomsWithNoConnections()
    {
        if(inBetweenPoints.Count <=0)
        {
            placeWall = false;
            Debug.Log("NO CONNECTIONS");
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (inBetweenPoints != null)
        {
            for (int i = 0; i < inBetweenPoints.Count; i++)
            {
                Gizmos.DrawWireSphere(inBetweenPoints[i], 0.4f);
                Gizmos.color = Color.black;
            }
        }
    }
}
