using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{

    //공격력
    //사거리
    //이동속도
    //공격속도

    private void Awake()
    {
        base.Awake();

        Init();
    }

    private void Start()
    {
    }

    private void Update()
    {
        StartMove(1f * standardNumber, 3f);
    }

    private void FixedUpdate()
    {
        //Move();
    }

    protected override void Init()
    {
        hp = 100000;
        range = 1;
        moveSpeed = 5f;
        attackDamage = 10;
        attackSpeed = 4f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {

            PlayerAttackInfo tempPlayer = collision.gameObject.GetComponent<PlayerAttackInfo>();
            
            if (tempPlayer != null)
            {
                Debug.Log(tempPlayer.attackDamage);
                Hit(tempPlayer.attackDamage);
            }
        }
    }

    protected override void Attack()
    {
        base.Attack();
        
    }

    public void SlimeAttackMove()
    {
        enemyRigid.AddForce(new Vector2((isRight ? 1 : -1) * standardNumber * 30f, 0));
    }

    protected override void Hit(int damage)
    {
        hp -= damage;
        if(hp < 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("isHit");
        }
        

    }



    protected override void Die()
    {
        Destroy(this.gameObject);
    }
}