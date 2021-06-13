using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Enemy : LifeObject
{
    const string animBaseLayer = "Base Layer";

    private GameObject target;
    private PolygonCollider2D enemyCollider;
    public Player player;
    protected Rigidbody2D enemyRigid;
    protected Animator animator;


    //임시
    protected SpriteRenderer sr;


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
    public bool isDead = false;

    public bool canHit = true;
    public float hitDelay = 1.0f;
    public float currentHitDelay = 0;

    public bool canAttack = true;
    public float attackDelay = 5.0f;
    public float currentAttackDelay = 0;

    public bool attacking = false;
    public bool hitting = false;

    protected void Awake()
    {
        enemyRigid = transform.parent.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<PolygonCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public virtual void SlimeAttackMove()
    {
        enemyRigid.AddForce(new Vector2((isRight ? 1 : -1) * standardNumber * 30f, 0));
    }

    protected void Update()
    {
        if (!canHit)
        {
            currentHitDelay += Time.deltaTime;

            if(currentHitDelay >= hitDelay)
            {
                currentHitDelay = 0;
                canHit = true;
                //임시
                sr.color = new Color(1, 1, 1, 1f);
            }
        }

        if (!canAttack)
        {
            currentAttackDelay += Time.deltaTime;

            if (currentAttackDelay >= attackDelay)
            {
                currentAttackDelay = 0;
                canAttack = true;
                //임시
                Debug.Log("2");
            }
        }
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
        if (canAttack && !hitting && !attacking)
        {
            float exitTime = 0.8f;
            attacking = true;
            yield return new WaitForSeconds(time);
            Attack();

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("slime_attack"))
            {
                //전환 중일 때 실행되는 부분
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
            {
                //애니메이션 재생 중 실행되는 부분
                yield return null;
            }

            canAttack = false;
            attacking = false;
            Debug.Log("1");
        }
    }

    //거리 이동
    protected void StartMove(float speed, float time)
    {
    }

    protected void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected abstract void Init();

    protected abstract void Hit(int damage);

    protected abstract IEnumerator Die(float second);
}
