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

    List<Transform> aroundRoomsPosition;

    Transform closestRoom;

    [SerializeField] float circleCastRadius = 0;

    // Start is called before the first frame update
    void Start()
    {
       // aroundRoomsPosition = new List<Transform>();

        //colliderYSize = Random.Range(1f, maxColliderSize);
        //colliderXSize = Random.Range(1f, maxColliderSize);
        //collider = GetComponent<BoxCollider2D>();
        //collider.size = new Vector2(colliderXSize, colliderYSize);
    }

    // Update is called once per frame
    void Update()
    {
        timerForRooms -= Time.deltaTime;
        if (timerForRooms <= 0)
        {
            stopped = true;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, closestRoom.position);
    }
    void FixPosition()
    {
        if(stopped)
        {
            float newXPosition;
            float newYPosition;
            newXPosition = Mathf.Round(transform.position.x);
            newYPosition = Mathf.Round(transform.position.y);
            transform.position = new Vector3(newXPosition, newYPosition, 0);
            
            Mathf.Round(transform.position.y);
            Debug.Log("fixed");
            stopped = false;
        }
    }
}
