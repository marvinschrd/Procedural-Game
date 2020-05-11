using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] Transform firingPosition;
    [SerializeField] GameObject FireBallPrefab;
    [SerializeField] float bulletForce = 5;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attacking");
            GameObject Fireball;
            Fireball = Instantiate(FireBallPrefab, firingPosition.position, Quaternion.identity);
            Rigidbody2D fireBallBody = Fireball.GetComponent<Rigidbody2D>();
            fireBallBody.AddForce(firingPosition.right * bulletForce, ForceMode2D.Impulse);
        }
    }
}
