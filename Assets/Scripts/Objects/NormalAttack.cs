using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{

    Health touchedObjectHealth;
    PlayerHealth touchedPlayerHealth;
    [SerializeField] float damage = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Health>()!= null|| collision.gameObject.GetComponent<PlayerHealth>()!=null)
        {
            //Health touchedObjectHealth;
            if(collision.gameObject.GetComponent<Health>()!=null)
            {
            touchedObjectHealth = collision.gameObject.GetComponent<Health>();
            touchedObjectHealth.TakeDamage(damage);
            }
            if (collision.gameObject.GetComponent<PlayerHealth>() != null)
            {
                touchedPlayerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                touchedPlayerHealth.TakeDamage(damage);
            }
            
        }
    }

}
