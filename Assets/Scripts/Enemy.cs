using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Enemy : LifeObject
{
    private GameObject target;
    private PolygonCollider2D enemyCollider;
    public Player player;
    protected Rigidbody2D enemyRigid;
    protected Animator animator;

    //현재 자신이 오른쪽을 바라보고 있는가?
    protected bool isRight = false;


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
        enemyRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<PolygonCollider2D>();
        
    }
    public virtual void SlimeAttackMove()
    {
        enemyRigid.AddForce(new Vector2((isRight ? 1 : -1) * standardNumber * 30f, 0));
    }

    protected virtual void Attack()
    {
        animator.SetTrigger("isAttack");
        gameObject.tag = "EnemyAttack";
        enemyRigid.gravityScale = 0;
        enemyCollider.isTrigger = true;
        gameObject.layer = 9;

    }

    protected virtual void OffAttack()
    {
        gameObject.tag = "Enemy";
        enemyCollider.isTrigger = false;
        enemyRigid.gravityScale = 1;
        gameObject.layer = 7;
    }
    //공격 시작
    IEnumerator StartAttack(float time)
    {
        yield return new WaitForSeconds(time);
        Attack();
    }

    //거리 이동
    protected void StartMove(float speed, float time)
    {
        transform.DOMoveX(speed, 2f);
    }

    void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected abstract void Init();

    protected abstract void Hit(int damage);
    protected abstract void Die();
}
