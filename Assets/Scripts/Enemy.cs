using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LifeObject
{
    private GameObject target;
    Collider2D col;
    Rigidbody2D rigid;
    public int hp;
    public int range;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        
    }

    private void FixedUpdate() {

    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
    }

    private void OnTriggerExit2D(Collider2D collider2D) {
    }
}
