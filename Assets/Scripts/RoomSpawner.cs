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
   
  struct Node
    {
        public Vector2 pos;
        public List<Vector2> neighbourRooms;

        
    }

   [SerializeField] Node[] nodes;
    

    int roomIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        roomsToSpawn = new List<GameObject>();
        roomsSpawned = new List<GameObject>();
        spawnPosition = new Vector3(0, 0, 0);
        secondSpawnPosition = new Vector3(-2, 0, 0);
       

        FillRoomList();
        SpawnRooms();
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

   void CheckNeighbourRooms()
    {
        for(int x = 0; x<nodes.Length;x++)
        {
            Collider2D [] colliders = Physics2D.OverlapCircleAll(nodes[x].pos, 1f);
            for (int i = 0; i<colliders.Length;i++)
            {
                nodes[x].neighbourRooms.Add(colliders[i].gameObject.transform.position);
            }
           
        }
    }

    void RandomSelectInList()
    {

    }

    void FillRoomList()
    {
        for (int x = 0; x < numberOfRoomsForThisLevel; x++)
        {

            RandomlyChosePrefab();
            GameObject room = roomsPrefabs[roomIndex];
            Debug.Log(room.name);
            //while (roomsToSpawn.Contains(roomsPrefabs[roomIndex]))
            //{
            //    RandomlyChosePrefab();
            //}
            roomsToSpawn.Add(room);
        }
        
    }

    void RandomlyChosePrefab()
    {
        roomIndex = Random.Range(0, totalNumberOfRoomsPrefabs);
        Debug.Log(roomIndex);
        Debug.Log(roomsPrefabs[roomIndex].name);
        
       
    }

    void SpawnRooms()
    {
        while(roomsToSpawn.Count>2)
        {
            Instantiate(roomsToSpawn[0], spawnPosition, Quaternion.identity);
            roomsSpawned.Add(roomsToSpawn[0]);
            roomsToSpawn.RemoveAt(0);
        }
        while (roomsToSpawn.Count > 0)
        {
            Instantiate(roomsToSpawn[0], secondSpawnPosition, Quaternion.identity);
            roomsSpawned.Add(roomsToSpawn[0]);
            roomsToSpawn.RemoveAt(0);
        }
    }

}
