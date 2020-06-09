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
    bool startTimer = false;
    bool stopped = false;
    List<Transform> aroundRoomsPosition;
    [SerializeField] float circleCastRadius = 0;
    RoomSpawner RoomSpawner;
    bool canStart = false;
    List<EnnemiesSpawner> neighbours;


    [SerializeField] GameObject wallBlockPrefab;
    [SerializeField] GameObject[] DifferentWallBlocks;
    bool placeWall = true;


    List<ContactPoint2D> contacts;
    List<Vector2> inBetweenPoints;
    bool getContacts = true;

    [SerializeField] GameObject playerPortal;
    [SerializeField] GameObject magic;

    void Start()
    { 
        RoomSpawner = FindObjectOfType<RoomSpawner>();
        collider = GetComponent<BoxCollider2D>();
        polyCollider = GetComponent<PolygonCollider2D>();
        neighbours = new List<EnnemiesSpawner>();
        contacts = new List<ContactPoint2D>();
        inBetweenPoints = new List<Vector2>();

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

    void Update()
    {
        switch (step)
        {
            case Step.WAITING:
               
                if (canStart)
                {
                    step = Step.GET_NEIGHBOURS;
                }
                break;
            case Step.GET_NEIGHBOURS:  // instantiate a circle cast to detect and store rooms next to this room
                Debug.Log("GETTING NEIGBOURS");
                collider.enabled = false;
                Vector2 colliderSize = new Vector2(transform.localScale.x, transform.localScale.y);
                circleCastRadius = colliderSize.magnitude/2;
                RaycastHit2D[] hits = Physics2D.CircleCastAll(gameObject.transform.position, circleCastRadius, Vector2.zero);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != gameObject.GetComponent<BoxCollider2D>()&&hit.collider.gameObject.GetComponent<EnnemiesSpawner>())
                    {
                        neighbours.Add(hit.transform.gameObject.GetComponent<EnnemiesSpawner>());
                    }
                }
                step = Step.CHECK_COLLISIONS;
                break;
            case Step.CHECK_COLLISIONS:  // Finding and storing contacts made by the room polygon collider
                if (getContacts)
                {
                   polyCollider.GetContacts(contacts); 
                    while (contacts.Count > 0)
                    {
                        Vector2 firstPointPosition = new Vector2(contacts[0].point.x, contacts[0].point.y);
                        Vector2 secondPointPosition = new Vector2(contacts[1].point.x, contacts[1].point.y);
                        Vector2 inBetweenPointPosition = (firstPointPosition + secondPointPosition) / 2;
                        inBetweenPoints.Add(inBetweenPointPosition);
                        contacts.RemoveAt(0);  // removing first two collision point each time because they come in pairs
                        contacts.RemoveAt(0);   
                    }
                    getContacts = false;
                    DrawCircles();
                    Destroy(body);
                }
                step = Step.BUILD_WALLS;
                break;
            case Step.BUILD_WALLS:
                    DestroyRoomsWithNoConnections();
                if (placeWall)
                {
                    placeWall = false; //Spawning each side of walls
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
                step = Step.END;
                break;
            case Step.END:
                break;
        }
    }

        void SpawnLeftWall()
    {
        if (wallBlockPrefab != null)
        {
            float roomHeight = transform.localScale.y ;
            float roomWidth = transform.localScale.x ;
            float numberOfWallToSpawn = roomHeight ;
            for (int i = 0; i < numberOfWallToSpawn*2; i++)
            {
               
                Instantiate(wallBlockPrefab, new Vector3(transform.position.x - (roomWidth/2 - 0.475f), transform.position.y+(roomHeight/2) + i * (-0.5f), -1), Quaternion.identity);
            }
        }

        
        
    }

    void SpawnRightWall()
    {
        if (wallBlockPrefab != null)
        {
            float roomHeight = transform.localScale.y;
            float roomWidth = transform.localScale.x;
            float numberOfWallToSpawn = roomHeight ;
            for (int i = 0; i < numberOfWallToSpawn *2 ; i++)
            {
               Instantiate(wallBlockPrefab, new Vector3(transform.position.x + (roomWidth / 2-0.475f), transform.position.y + (roomHeight / 2) + i * (-0.5f), -1), Quaternion.identity);
            }
        }
    }

    void SpawnUpWall()
    {
        if (wallBlockPrefab != null)
        {
            float roomHeight = transform.localScale.y;
            float roomWidth = transform.localScale.x;
            float numberOfWallToSpawn = roomWidth;

            for (int i = 0; i < numberOfWallToSpawn -2 ; i++)
            {

                int index = Random.Range(0, DifferentWallBlocks.Length);

                
                Instantiate(DifferentWallBlocks[index], new Vector3(transform.position.x + (roomWidth / 2-1.5f) + i * (-1f), transform.position.y + (roomHeight / 2), -1), Quaternion.identity);
            }
        }
    }

    void SpawnDownWall()
    {
        if (wallBlockPrefab != null)
        {
            float roomHeight = transform.localScale.y;
            float roomWidth = transform.localScale.x;
            float numberOfWallToSpawn = roomWidth;
            for (int i = 0; i < numberOfWallToSpawn -2 ; i++)
            {
                
               Instantiate(wallBlockPrefab, new Vector3(transform.position.x + (roomWidth / 2-1.5f) + i * (-1f), transform.position.y - (roomHeight / 2)+0.475f, -1), Quaternion.identity);
            }
        }
    }

    
    public void FixPosition(float additionalTime)
    {
        canStart = true; 
    }

    void DrawCircles()
    {

        for (int i = 0; i < inBetweenPoints.Count; i++)
        {
            //Gizmos.DrawWireSphere(inBetweenPoints[i], 0.5f);
            //Gizmos.color = Color.black;
           // Debug.Log("inbetween point" + inBetweenPoints[i]);
        }
    }

    void WallOpening()
    {
        //Using prevously stored positions in between each pair of collision points to detect and destroy walls

        for (int i = 0; i < inBetweenPoints.Count; i++)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(inBetweenPoints[i], 0.4f, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
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
            Destroy(gameObject);
        }
    }

    public void SpawnPlayer()
    {
       Vector3 spawnPosition = new Vector3(Random.Range(transform.position.x - (transform.localScale.x / 2), transform.position.x + (transform.localScale.x / 2)), Random.Range(transform.position.y - (transform.localScale.y / 2), transform.position.y + (transform.localScale.y / 2)), 0f);
       RaycastHit2D[] hit = Physics2D.CircleCastAll(spawnPosition, 0.1f, Vector2.zero);
        int maxLoop = 0;
        while (hit.Length > 0&&maxLoop<100)
        {
             spawnPosition = new Vector3(Random.Range(transform.position.x - (transform.localScale.x / 2), transform.position.x + (transform.localScale.x / 2)), Random.Range(transform.position.y - (transform.localScale.y / 2), transform.position.y + (transform.localScale.y / 2)), 0f);
             hit = Physics2D.CircleCastAll(spawnPosition, 0.1f, Vector2.zero);
            maxLoop++;
        }
        Instantiate(playerPortal, spawnPosition, Quaternion.identity);

    }


   public void SpawnInNeighbours()
    {
        foreach (EnnemiesSpawner neighbour in neighbours)
        {
            Debug.Log("SPAWNNEIGHBOURS");
            neighbour.ActivateSpawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("PLAYER IN !");
            SpawnInNeighbours();
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
        Gizmos.DrawWireSphere(transform.position, new Vector2(transform.localScale.x, transform.localScale.y).magnitude/2);
    }
}
