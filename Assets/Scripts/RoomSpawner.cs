using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] int numberOfRoomsForThisLevel = 7;
    [SerializeField] int totalNumberOfRoomsPrefabs = 10;
    [SerializeField] List<GameObject> roomsPrefabs;
    List<GameObject> roomsToSpawn;
    List<GameObject> roomsSpawned;
    Vector3 spawnPosition;
    Vector3 secondSpawnPosition;


    List<GameObject> usedRooms;
    List<GameObject> orderedRooms;
    Vector3 currentRoomPosition;

    [SerializeField] float detectionRadius = 1;

    [SerializeField] float waitingTime = 5f;
    float waitingTimer = 0;
    float timeToAdd = 0;

  struct Node
    {
        public Transform pos;
        public List<Node> neighbourRooms;
        
    }

   List<Node> nodes;
    

    int roomIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        roomsToSpawn = new List<GameObject>();
        roomsSpawned = new List<GameObject>();
        spawnPosition = new Vector3(0, 0, 0);
        secondSpawnPosition = new Vector3(-2, 0, 0);
       

        //FillRoomList();
        //SpawnRooms();
       
    }

    enum Step
    {
        SELECT_ROOMS,
        SPAWNS_ROOMS,
        ORDER_ROOMS_BY_DYSTANCE_FROM_CENTER,
        LOCK_ROOMS_POSITON_BY_ORDER,
        STOP
    }

    Step step = Step.SELECT_ROOMS;

    // Update is called once per frame
    void Update()
    {

        switch(step)
        {
            case Step.SELECT_ROOMS:
                waitingTimer = waitingTime;
                FillRoomList();
                step = Step.SPAWNS_ROOMS;
                break;
            case Step.SPAWNS_ROOMS:
                SpawnRooms();
                step = Step.ORDER_ROOMS_BY_DYSTANCE_FROM_CENTER;
                break;
            case Step.ORDER_ROOMS_BY_DYSTANCE_FROM_CENTER:
               roomsSpawned.Sort(SortByDistanceToCenter);
                waitingTime -= Time.deltaTime;
                if(waitingTime<=0)
                {
                    step = Step.LOCK_ROOMS_POSITON_BY_ORDER;
                }
                break;
            case Step.LOCK_ROOMS_POSITON_BY_ORDER:
                for (int i = 0; i < roomsSpawned.Count; i++)
                {
                    
                    Debug.Log("NOW BLOCKING");
                    Debug.Log(roomsSpawned[i].gameObject.name);
                    Rooms room = roomsSpawned[i].GetComponent<Rooms>();
                    room.FixPosition(timeToAdd);
                    timeToAdd += 2f;
                    //Debug.Log(roomsSpawned[i].gameObject.name);

                    //Debug.Log(roomsSpawned[i].transform.position);
                   
                }
                    step = Step.STOP;
                    //float xPosition = Mathf.Round(roomsSpawned[i].transform.position.x);
                    //float yPosition = Mathf.Round(roomsSpawned[i].transform.position.y);
                    //roomsSpawned[i].transform.position = new Vector3(xPosition, yPosition, 0);
                break;
            case Step.STOP:
                Debug.Log("STOP");
                break;
        }


    //   foreach(Node node in nodes)
    //    {
    //        List<Transform> neighbor = GetNeighbor(node);
    //    }
        
    }

    int SortByDistanceToCenter(GameObject a, GameObject b)
    {
        float squaredRangeA = (a.transform.position - spawnPosition).sqrMagnitude;
        float squaredRangeB = (b.transform.position - spawnPosition).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    //List<Transform> GetNeighbor(Node room)
    //{
    //    RaycastHit2D[] hits = Physics2D.CircleCastAll(room.transform, detectionRadius, Vector2.zero);
    //    List<Transform> neighbor = new List<Transform>();

    //    foreach (RaycastHit2D hit in hits)
    //    {
    //        if (hit.collider != room.Collider2D)
    //        {
    //            neighbor.Add(hit.transform);
    //        }
    //    }

    //    return neighbor;
    //}

    //void CheckNeighbourRooms()
    //{



    //    for(int x = 0; x<nodes.Length;x++)
    //    {
    //        RaycastHit2D[] hits = Physics2D.CircleCastAll(nodes[x].pos, detectionRadius, Vector2.zero);

    //        //for (int i = 0; i<colliders.Length;i++)
    //        //{
    //        //    nodes[x].neighbourRooms.Add(colliders[i].gameObject.transform.position);
    //        //}

    //    }
    //}
    void FillRoomList()
    {
        for (int x = 0; x < numberOfRoomsForThisLevel; x++)
        {

            RandomlyChosePrefab();
            GameObject room = roomsPrefabs[roomIndex];
            //Debug.Log(room.name);
            roomsToSpawn.Add(room);
        }
        
    }

    void RandomlyChosePrefab()
    {
        roomIndex = Random.Range(0, roomsPrefabs.Count);
        //Debug.Log(roomIndex);
        //Debug.Log(roomsPrefabs[roomIndex].name);
    }

    void SpawnRooms()
    {
        while(roomsToSpawn.Count>10)
        {
           roomsSpawned.Add( Instantiate(roomsToSpawn[0], spawnPosition, Quaternion.identity));
            //roomsSpawned.Add(roomsToSpawn[0]);
            roomsToSpawn.RemoveAt(0);
        }
        while (roomsToSpawn.Count > 0)
        {
            roomsSpawned.Add(Instantiate(roomsToSpawn[0],secondSpawnPosition, Quaternion.identity));
           // roomsSpawned.Add(roomsToSpawn[0]);
            roomsToSpawn.RemoveAt(0);
        }
    }

}
