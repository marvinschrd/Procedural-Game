using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    BoxCollider2D collider;
    float colliderYSize = 1f;
    float colliderXSize = 1f;
    [SerializeField] float maxColliderSize = 3f;
    // Start is called before the first frame update
    void Start()
    {
        colliderYSize = Random.Range(1f, maxColliderSize);
        colliderXSize = Random.Range(1f, maxColliderSize);
        collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(colliderXSize, colliderYSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
