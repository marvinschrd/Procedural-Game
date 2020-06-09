using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortal : MonoBehaviour
{
    [SerializeField] List<GameObject> playerPossibilities;
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
        int index = Random.Range(0, playerPossibilities.Count);
        GameObject player = playerPossibilities[index];
        Instantiate(player, new Vector3(transform.position.x + 0.4f, transform.position.y, 0), Quaternion.identity);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
