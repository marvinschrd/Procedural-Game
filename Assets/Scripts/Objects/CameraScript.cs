using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    Transform transform;
    Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (player != null)
        //{
        //    transform.position = player.position;
        //}
    }

    public void FindPlayer(Transform playerPosition)
    {
        transform.position = new Vector3(playerPosition.position.x,playerPosition.position.y,-10f);

    }
}
