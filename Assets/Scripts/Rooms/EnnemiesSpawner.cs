using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesSpawner : MonoBehaviour
{

    [SerializeField] GameObject monsterPortal;
    [SerializeField] int roomMonsterCapacity = 0;
    [SerializeField] GameObject magic;

    Vector3 spawnPosition;
    RaycastHit2D[] hit;
    Collider2D[] hit2;
   [SerializeField] LayerMask roomLayer;

    bool selectSpawn = false;
    bool alreadySPawned = false;

    Rooms room;

    // Start is called before the first frame update
    void Start()
    {
        
        room = GetComponent<Rooms>();
    }



    enum State
    {
        WAIT,
        SELECT_SPAWN,
        CHECK_SPAWN,
        SPAWN,
        STOP
    }

    State state = State.WAIT;

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.WAIT:
                if(selectSpawn)
                {
                    Debug.Log("SPAWN WAITING");
                    state = State.SELECT_SPAWN;
                }
                break;
            case State.SELECT_SPAWN:
                spawnPosition = new Vector3(Random.Range(transform.position.x - (transform.localScale.x / 2), transform.position.x + (transform.localScale.x / 2)), Random.Range(transform.position.y - (transform.localScale.y / 2), transform.position.y + (transform.localScale.y / 2)),0f);
                Debug.Log("SELECTING SPAWN");
                state = State.CHECK_SPAWN;
                break;
            case State.CHECK_SPAWN:
                Debug.Log("CHECKING SPAWN");
                 hit = Physics2D.CircleCastAll(spawnPosition, 0.1f,Vector2.zero,roomLayer);

                if (hit.Length>0)
                {
                    Debug.Log("HIT" + hit[0].transform.gameObject.name);
                    Debug.Log("HIT" + hit[0].transform.gameObject.layer);
                    if (hit[0].transform.gameObject.layer == LayerMask.NameToLayer("Walls"))
                    {
                        state = State.SELECT_SPAWN;
                    }
                    else
                    {
                        state = State.SPAWN;
                    }
                }
                else
                {
                    state = State.SPAWN;
                }
                break;
            case State.SPAWN:
                Debug.Log("SPAWN");
                Instantiate(monsterPortal, spawnPosition, Quaternion.identity);
                Instantiate(magic, spawnPosition, Quaternion.identity);
                roomMonsterCapacity--;
                if(roomMonsterCapacity>0)
                {
                    state = State.SELECT_SPAWN;
                }
                else
                {
                    state = State.STOP;
                }
                break;
            case State.STOP:

                break;
        }


    }

   public void ActivateSpawn()
    {
        Debug.Log("SPAWNACTIVATED");
        selectSpawn = true;
    }

   

   

}
