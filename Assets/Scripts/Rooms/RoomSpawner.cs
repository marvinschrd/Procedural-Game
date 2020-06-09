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
   

    int roomIndex = 0;
    void Start()
    {
        roomsToSpawn = new List<GameObject>();
        roomsSpawned = new List<GameObject>();
        spawnPosition = new Vector3(0, 0, 0);
        secondSpawnPosition = new Vector3(-2, 0, 0);
    }

    enum Step
    {
        SELECT_ROOMS,
        SPAWNS_ROOMS,
        ORDER_ROOMS_BY_DYSTANCE_FROM_CENTER,
        LOCK_ROOMS_POSITON_BY_ORDER,
        SPAWN_PLAYER,
        STOP
    }

    Step step = Step.SELECT_ROOMS;

    void Update()
    {

        switch(step)
        {
            case Step.SELECT_ROOMS: // select rooms from prefab list
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
                    Rooms room = roomsSpawned[i].GetComponent<Rooms>();
                    room.FixPosition(timeToAdd);
                    timeToAdd += 2f;
                }
                    step = Step.SPAWN_PLAYER;
                break;
            case Step.SPAWN_PLAYER:
                Rooms roomForPlayer = roomsSpawned[roomsSpawned.Count-1].GetComponent<Rooms>();
                roomForPlayer.SpawnPlayer();
                step = Step.STOP;
                break;
            case Step.STOP:
                Debug.Log("STOP");
                break;
        }
    }

    int SortByDistanceToCenter(GameObject a, GameObject b)
    {
        float squaredRangeA = (a.transform.position - spawnPosition).sqrMagnitude;
        float squaredRangeB = (b.transform.position - spawnPosition).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    void FillRoomList()
    {
        for (int x = 0; x < numberOfRoomsForThisLevel; x++)
        {

            RandomlyChosePrefab();
            GameObject room = roomsPrefabs[roomIndex];
            roomsToSpawn.Add(room);
        }
        
    }

    void RandomlyChosePrefab()
    {
        roomIndex = Random.Range(0, roomsPrefabs.Count);
    }

    void SpawnRooms()
    {
        while (roomsToSpawn.Count > 0)
        {
            roomsSpawned.Add(Instantiate(roomsToSpawn[0],secondSpawnPosition, Quaternion.identity));
            roomsToSpawn.RemoveAt(0);
        }
    }

}
