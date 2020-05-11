using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float destructionTime = 3;
    [SerializeField] float damage = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, destructionTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ennemy"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
