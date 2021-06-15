using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : LifeObject
{
    const string animBaseLayer = "Base Layer";

    private GameObject target;
    private PolygonCollider2D enemyCollider;
    public Player player;
    protected GameObject effect;
    protected Rigidbody2D enemyRigid;
    protected Animator animator;
    protected Animator effectAnimator;

    //임시
    protected SpriteRenderer sr;

    public float timeSlowNumber = 0.2f;

    //현재 자신이 오른쪽을 바라보고 있는가?
    protected bool isRight = false;

    public int hp;
    //공격력
    public int attackDamage;
    //사거리
    public int range;
    //이동속도
    public float moveSpeed = 1;
    public float moveCurrentSpeed = 1;
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


        effect = transform.parent.GetChild(2).gameObject;
        effectAnimator = effect.GetComponent<Animator>();

        GameManager.Instance.enemies.Add(this);
        
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

            if (currentHitDelay >= hitDelay)
            {
                currentHitDelay = 0;
                canHit = true;
                //임시
                sr.color = new Color(1, 1, 1, 1f);
            }
        }
        if (GameManager.Instance.timeStop) return;

        if (!canAttack)
        {
            currentAttackDelay += Time.deltaTime;

            if (currentAttackDelay >= attackDelay)
            {
                currentAttackDelay = 0;
                canAttack = true;
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
    IEnumerator StartBaseAttack(float time)
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
        }
    }

    public void TimeStop()
    {
        if (GameManager.Instance.timeStop)
        {
            animator.speed = 0;
            effectAnimator.speed = 0;
        }
        else
        {
            if (GameManager.Instance.timeSlow)
            {
                animator.speed = timeSlowNumber;
                effectAnimator.speed = timeSlowNumber;
            }
            else
            {
                animator.speed = 1;
                effectAnimator.speed = 1;
            }


        }
    }

    public void TimeSlow()
    {
        if (GameManager.Instance.timeStop)
        {
            if (GameManager.Instance.timeSlow)
            {
                moveCurrentSpeed = moveCurrentSpeed * timeSlowNumber;
            }
            else
            {
                moveCurrentSpeed = moveCurrentSpeed / timeSlowNumber;
            }
        }
        else
        {
            if (GameManager.Instance.timeSlow)
            {
                animator.speed = timeSlowNumber;
                effectAnimator.speed = timeSlowNumber;
                moveCurrentSpeed = moveCurrentSpeed * timeSlowNumber;
                Debug.Log("test");
            }
            else
            {
                animator.speed = 1;
                effectAnimator.speed = 1;
                moveCurrentSpeed = moveCurrentSpeed / timeSlowNumber;
            }
        }
        

    }


    protected void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.parent.localScale;
        scale.x *= -1;
        transform.parent.localScale = scale;
    }

    protected abstract void Init();

    protected abstract void Hit(int damage);

    protected abstract IEnumerator Die(float second);
}
