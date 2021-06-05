using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    Rigidbody2D rb;

    //공격력
    //사거리
    //이동속도
    //공격속도

    private void Awake()
    {
        base.Awake();
    }

    protected override void Init()
    {
        hp = 100;
        range = 1;
        moveSpeed = 10000;
        attackDamage = 10;
        attackSpeed = 4f;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Hit();
        }
    }

    protected override void Hit()
    {
        Debug.Log("아야");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

    }


}
