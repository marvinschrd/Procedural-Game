using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 0;
    [SerializeField] float maxHealth = 0;
    [SerializeField] float hurtTime = 0;
    float hurtTimer = 0;

    SpriteRenderer sprite;
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
            Destroy(gameObject);
        }

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

       
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        sprite.color = Color.red;
    }

    public void TakeHealth(float heal)
    {
        health += heal;
    }

}
