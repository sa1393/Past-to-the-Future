using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : LifeObject
{
    private GameObject target;
    private BoxCollider2D enemyFloor;
    public Player player;
    protected Rigidbody2D enemyRigid;

    public int hp;
    //공격력
    public int attackDamage;
    //사거리
    public int range;
    //이동속도
    public float moveSpeed = 1;
    //공격속도
    public float attackSpeed;

    protected void Awake()
    {
        enemyFloor = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(enemyFloor, player.playerCollider);
        enemyRigid = GetComponent<Rigidbody2D>();
    }

    //일정한 거리 이동
    protected void Move()
    {
        enemyRigid.velocity = new Vector2(100 * -moveSpeed, enemyRigid.velocity.y);
    }

    protected abstract void Init();

    protected abstract void Hit(int damage);
    protected abstract void Die();
}
