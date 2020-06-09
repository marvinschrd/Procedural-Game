using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    PlayerController player;
    Vector3 playerPos;
    // Start is called before the first frame update
    void Start()
    {
       // player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player!=null)
        {
         playerPos = player.GivePosition();
        }
    }

    public void GetPlayer()
    {
        Debug.Log("GOTPLAYER");
        player = FindObjectOfType<PlayerController>();
    }
   
    public Vector3 GivePlayerPosition()
    {
        return playerPos;
    }
}
