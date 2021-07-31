using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBat : Enemy
{
    public GameObject bullet;
    public Transform attackPosition;

    private Rigidbody2D rigid;
    private Animator animator;

    private float axis_X;
    private float axis_Y;
    private float origin_Y;
    private float movingDelay = 0.3f;
    private bool movingChainge = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(canAttack)
        {
            canAttack = false;
            StartCoroutine(CreateBullet());
        }
        if(movingChainge)
        {
            movingChainge = false;
            StartCoroutine(RandomMoving());
            rigid.velocity = new Vector2(axis_X * moveSpeed, axis_Y * moveSpeed);
        }
    }

    IEnumerator RandomMoving()
    {
        axis_X = Random.Range(-1f, 1f);
        axis_Y = Random.Range(-1f, 1f);

        if (axis_X == 0) axis_X = 1;
        if (axis_Y == 0) axis_Y = 1;

        if(origin_Y - 3 > gameObject.transform.position.y)
        {
            axis_Y = 1;
        } else if(origin_Y + 3 < gameObject.transform.position.y)
        {
            axis_Y = -1;
        }

        yield return new WaitForSeconds(movingDelay);

        movingChainge = true;
    }

    IEnumerator CreateBullet()
    {
        Instantiate(bullet);
        
        bullet.transform.position = attackPosition.transform.position;

        yield return new WaitForSeconds(attackSpeed);

        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {

            if (!canHit) return;
            Player player = collision.gameObject.transform.parent.GetComponent<Player>();
            PlayerAttackInfo tempPlayer = collision.gameObject.GetComponent<PlayerAttackInfo>();
            PlayerAttackEffect effect = collision.gameObject.GetComponent<PlayerAttackEffect>();

            Debug.Log("¸ÂÀ½");


            if (tempPlayer != null)
            {
                Hit(tempPlayer.attackDamage);
                canHit = false;
                player.SkillCoolDown();
            }
            else if (effect != null)
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

    protected override void Init()
    {
        hp = 3;
        moveSpeed = 100f;
        moveCurrentSpeed = moveSpeed;
        attackSpeed = 2.5f;
        origin_Y = gameObject.transform.position.y;
    }

    protected override void Hit(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            animator.SetTrigger("isDead");
            StartCoroutine(Die(0.5f));
        }
        else
        {
            sr.color = new Color(1, 1, 1, 0.4f);
        }
    }

    protected override IEnumerator Die(float second)
    {
        yield return new WaitForSeconds(second);

        Destroy(gameObject);
    }
}
