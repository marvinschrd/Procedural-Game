using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ennemy"))
        {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            enemyController.InstatiatePlayablePrefab();
            Destroy(gameObject);
        }
    }

}
