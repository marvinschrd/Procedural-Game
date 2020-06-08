using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] float waitingTime;
    bool canTakeControl = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        waitingTime -= Time.deltaTime;
        if(waitingTime<=0)
        {
            canTakeControl = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ennemy")&&canTakeControl)
        {
            Health health = collision.gameObject.GetComponent<Health>();
            health.InstatiatePlayablePrefab();
            Destroy(gameObject);
        }
    }

}
