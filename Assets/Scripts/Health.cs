using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 0;
    [SerializeField] float maxHealth = 0;
    [SerializeField] float hurtTime = 0;
    float hurtTimer = 0;
    bool hurt = false;

    SpriteRenderer sprite;
    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject playablePrefab;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            health = 0;
            Instantiate(deadBody, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        if(hurt)
        {
            hurtTimer -= Time.deltaTime;
            sprite.color = Color.red;
            if (hurtTimer<=0)
            {
                hurt = false;
                sprite.color = Color.white;
            }
        }

       
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        hurt = true;
        hurtTimer = hurtTime;
    }


    public void InstatiatePlayablePrefab()
    {
        Instantiate(playablePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
