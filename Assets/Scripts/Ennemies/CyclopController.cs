using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    enum State
    {
        PATROLING,
        FOLLOWING,
        ATTACKING
    }
    State state = State.PATROLING;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.PATROLING:
                break;
            case State.FOLLOWING:
                break;
            case State.ATTACKING:
                break;
        }
    }
}
