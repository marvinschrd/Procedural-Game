using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    Animator anim;
    [SerializeField] GameObject normalAttack;
    [SerializeField] GameObject HeavyAttack;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")&&normalAttack!=null)
        {
            anim.SetTrigger("Attacking");
           
        }
        if (Input.GetButtonDown("Fire2")&&HeavyAttack !=null)
        {
            anim.SetTrigger("HeavyAttacking");
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

    void EnableHeavyAttack()
    {
        HeavyAttack.SetActive(true);
    }

    void DisableHeavyAttack()
    {
        HeavyAttack.SetActive(false);
    }
}
