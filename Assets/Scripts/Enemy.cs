using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LifeObject
{
    private GameObject target;
    private BoxCollider2D enemyFloor;

    public int hp;
    //공격력
    public int attackDamage;
    //사거리
    public int range;
    //이동속도
    public float moveSpeed;
    //공격속도
    public float attackSpeed;

    private void Awake()
    {
        enemyFloor = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

}
