using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    //현재 자신이 오른쪽을 바라보고 있는가?
    private bool isRight = false;

    private Rigidbody2D rigid;
    private Animator animator;

    //공격력
    //사거리
    //이동속도
    //공격속도

    private void Awake()
    {
        base.Awake();

        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }

    protected override void Init()
    {
        hp = 100;
        range = 1;
        moveSpeed = 10000;
        attackDamage = 10;
        attackSpeed = 4f;
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

    void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}