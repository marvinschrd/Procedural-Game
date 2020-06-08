using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 0;
    [SerializeField] float maxHealth = 0;
    [SerializeField] GameObject ghost;
    [SerializeField] GameObject deadPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health<=0)
        {
            health = 0;
            Instantiate(deadPlayer, transform.position, Quaternion.identity);
            Instantiate(ghost, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if(health>= maxHealth)
        {
            health = maxHealth;
        }
    }

   

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void TakeHealth(float heal)
    {
        health += heal;
    }

}
