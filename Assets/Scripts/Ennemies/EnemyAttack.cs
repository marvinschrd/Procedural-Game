using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject normalAttack;
    [SerializeField] GameObject HeavyAttack;
    AI.PathFinder PathFinder;
    PlayerController player;
    float playerDistance = 0;
    bool isAttacking = false;
    bool isCloseEnough = false;
    [SerializeField] float attackCooldown = 1;
    float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        PathFinder = FindObjectOfType<AI.PathFinder>();
        player = FindObjectOfType<PlayerController>();
        attackTimer = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = PathFinder.ManhattanDistance(transform.position, player.GivePosition());
        
        if(playerDistance<=1.5f)
        {
            isCloseEnough = true;
            
        }
        else
        {
            isCloseEnough = false;
            attackTimer = attackCooldown;
        }

        if (isCloseEnough)
        {
           
            Attack();
            isAttacking = true;
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
                attackTimer = attackCooldown;
                isCloseEnough = false;
            }
        }
      
        //Debug.Log("ATTACKTIMER = " + attackTimer);
    }

     void Attack()
    {
        if (!isAttacking)
        {
            Debug.Log("ATTACK");
            anim.SetTrigger("Attacking");
        }
    }

    void ActivateNormalAttack()
    {
        normalAttack.SetActive(true);
    }
    void DisableNormalAttack()
    {
        normalAttack.SetActive(false);
    }

}
