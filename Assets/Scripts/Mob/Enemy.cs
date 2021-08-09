using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour, AttackObjectInterface
{
    public int standardNumber = 1000;
    const string animBaseLayer = "Base Layer";

    public Collider2D enemyCollider;
    private Collider2D enemyFloor;
    public Player player;
    
    protected Rigidbody2D enemyRigid;
    protected Animator animator;

    //�ӽ�
    protected SpriteRenderer sr;

    public float timeSlowNumber = 0.2f;

    //���� �ڽ��� �������� �ٶ󺸰� �ִ°�?
    protected bool isRight = false;

    public int hp;
    //��Ÿ�
    public int range;
    //�̵��ӵ�
    public float moveSpeed = 1;
    public float moveCurrentSpeed = 1;
    //���ݼӵ�
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


    //��ݷ�
    public int damage = 0;
    public int Damage { get => Damage; set => Damage = value; }

    protected void Awake()
    {
        enemyRigid = transform.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<PolygonCollider2D>();

        if(GetComponent<BoxCollider2D>() != null) {
            enemyFloor = GetComponent<BoxCollider2D>();
        }
        sr = GetComponent<SpriteRenderer>();

    }

    protected void Start()
    {
        if(GameObject.Find("player") != null) {
        player = GameObject.Find("player").GetComponent<Player>();

        }

        if(enemyFloor != null)
        {
            PolygonCollider2D[] colliders = player.transform.GetChild(0).GetComponent<PlayerAttackEffect>().colliders;

            for (int i = 0; i < colliders.Length; i++)
            {
                Physics2D.IgnoreCollision(enemyFloor, colliders[i]);
            }

        }

        GameManager.Instance.enemies.Add(this);
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
            }
        }
        if (GameManager.Instance.timeStop) return;

        if (!canAttack)
        {
            if(GameManager.Instance.timeSlow){
                currentAttackDelay += Time.deltaTime / 2;
            }
            else {
                currentAttackDelay += Time.deltaTime;
            }

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
        enemyCollider.isTrigger = true;
        gameObject.layer = 9;
        enemyRigid.gravityScale = 0;
    }

    protected virtual void OffAttack()
    {
        gameObject.tag = "Enemy";
        gameObject.layer = 7;
        enemyCollider.isTrigger = false;
        enemyRigid.gravityScale = 1;
    }

    public void TimeStop()
    {
        if (GameManager.Instance.timeStop)
        {
            animator.speed = 0;
            // effectAnimator.speed = 0;
        }
        else
        {
            if (GameManager.Instance.timeSlow)
            {
                animator.speed = timeSlowNumber;
                // effectAnimator.speed = timeSlowNumber;
            }
            else
            {
                animator.speed = 1;
                // effectAnimator.speed = 1;
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
                // effectAnimator.speed = timeSlowNumber;
                moveCurrentSpeed = moveCurrentSpeed * timeSlowNumber;
                Debug.Log("test");
            }
            else
            {
                animator.speed = 1;
                // effectAnimator.speed = 1;
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
