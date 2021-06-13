using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    CapsuleCollider2D rangeCollider;

    //공격력
    //사거리
    //이동속도
    //공격속도

    private void Awake()
    {
        base.Awake();

        Init();
        rangeCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        base.Update();
        StartMove(1f * standardNumber, 3f);
    }

    private void FixedUpdate()
    {
        //Move();
        SlimeMove();
    }

    protected override void Init()
    {
        hp = 4;
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
                Hit(tempPlayer.attackDamage);
                canHit = false;
            }
        }
    }

    private void SlimeMove()
    {
        if(!attacking && !hitting)
        {
            bool right = false;
            right = player.transform.position.x > transform.position.x ? true : false;

            if (right != isRight)
                Flip();

            enemyRigid.velocity = new Vector2(0.2f * standardNumber * (right ? 1 : -1) , enemyRigid.velocity.y);
        }
    }


    protected override void Hit(int damage)
    {
        hp -= damage;
        if(hp < 0)
        {
            animator.SetTrigger("isDead");
            StartCoroutine("Die", 1f);
        }
        else
        {
            sr.color = new Color(1, 1, 1, 0.4f);
        }
        

    }

    protected override IEnumerator Die(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(transform.parent.gameObject);

    }
}