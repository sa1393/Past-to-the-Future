using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    CapsuleCollider2D rangeCollider;

    private void Awake()
    {
        base.Awake();

        Init();
        rangeCollider = GetComponent<CapsuleCollider2D>();
    }
    private void Update()
    {
        base.Update();
        if (GameManager.Instance.timeStop) return;

    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.timeStop) return;
        if (canHit)
        {
            SlimeMove();
        }
    }

    private void Start()
    {
        base.Start();
    }

    protected override void Init()
    {
        hp = 4;
        moveSpeed = 0.4f;
        moveCurrentSpeed = moveSpeed;
        attackDamage = 10;
        attackSpeed = 4f;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {

            if (!canHit) return;
            Player player = collision.gameObject.transform.parent.GetComponent<Player>();
            PlayerAttackInfo tempPlayer = collision.gameObject.GetComponent<PlayerAttackInfo>();
            PlayerAttackEffect effect = collision.gameObject.GetComponent<PlayerAttackEffect>();

            Debug.Log("맞음");


            if (tempPlayer != null)
            {
                Hit(tempPlayer.attackDamage);
                canHit = false;
                player.SkillCoolDown();
            }
            else if(effect != null)
            {
                Hit(effect.damage);
                canHit = false;
                player.SkillCoolDown();
            }
            else
            {
               
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void SlimeMove()
    {
        if(!attacking && !hitting)
        {
            bool right = false; 
            if(player != null)
                right = player.transform.position.x > transform.position.x ? true : false;

            if (right != isRight)
                Flip();

            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 200)
            {
                StartCoroutine("SlimeBiteAttack", 0.5);
                return;

            }

            enemyRigid.velocity = new Vector2(moveCurrentSpeed * standardNumber * (right ? 1 : -1) , enemyRigid.velocity.y);
        }
    }

    //공격 시작
    IEnumerator SlimeBiteAttack(float time)
    {
        if (!hitting && !attacking)
        {
            float exitTime = 0.8f;
            attacking = true;
            yield return new WaitForSeconds(time);
            SlimeBite();

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("slime_attack2"))
            {
                //전환 중일 때 실행되는 부분
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
            {
                yield return null;
            }

            attacking = false;  
        }
    }

    public virtual void SlimeAttackMove()
    {
        enemyRigid.AddForce(new Vector2((isRight ? 1 : -1) * standardNumber * 60f, 0));
        enemyCollider.isTrigger = true;
    }

    private void SlimeBite()
    {
        animator.SetTrigger("isAttack2");
    }

    private void BiteAttackOn()
    {
        effect.tag = "EnemyAttack";
        effect.layer = 9;
        
        effectAnimator.SetTrigger("isEffect1");
    }
    private void BiteAttackOff()
    {
        effect.tag = "Untagged";
        effect.layer = 0;
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