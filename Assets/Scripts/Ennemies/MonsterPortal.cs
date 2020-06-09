using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPortal : MonoBehaviour
{

   [SerializeField] List<GameObject> monsters;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        int index = Random.Range(0, monsters.Count);
        GameObject monsterToSpawn = monsters[index];
        Instantiate(monsterToSpawn, new Vector3(transform.position.x + 0.4f, transform.position.y), Quaternion.identity);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
