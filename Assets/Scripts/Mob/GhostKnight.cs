using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostKnight : Enemy
{
    public GameObject bullet;
    public GameObject bullet2;

    public GameObject dartBat;
    public Transform attackPosition;

    private bool canAttack = true;
    private bool isAttack = false;

    private bool canCreate = true;

    private void Awake()
    {
        base.Awake();

        enemyRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        base.Start();

        Init();
        Debug.Log(transform.position);
    }

    void Update()
    {
        base.Update();
        if (GameManager.Instance.timeStop)
        {
            enemyRigid.velocity = new Vector2(0, 0);
        }

        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(baseAttack());
        }
        if (canCreate && !isAttack)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) > 1500) return;
            canCreate = false;
            StartCoroutine(createAttack());
        }
    }

    IEnumerator baseAttack()
    {
        isAttack = true;
        animator.speed = 2f;
        StartCoroutine(CreateBullet(50));
        animator.SetTrigger("attack");
        isAttack = false;
        yield return new WaitForSeconds(3f);
        canAttack = true;


    }

    IEnumerator createAttack()
    {
        
        animator.speed = 1.5f;
        animator.SetTrigger("attack");
        GameObject temp = Instantiate(dartBat, transform);
        temp.transform.position = new Vector2(transform.position.x, transform.position.y + 50);
        temp.transform.localScale = new Vector2(5f, 5f);
        yield return new WaitForSeconds(12f);
        canCreate = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            if (!canHit) return;
            Player player = collision.gameObject.transform.parent.GetComponent<Player>();
            PlayerAttackInfo tempPlayer = collision.gameObject.GetComponent<PlayerAttackInfo>();
            PlayerAttackEffect effect = collision.gameObject.GetComponent<PlayerAttackEffect>();

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

    IEnumerator CreateBullet(float temp)
    {
        Debug.Log("����");
        Instantiate(bullet);

        bullet.transform.position = new Vector2(attackPosition.transform.position.x, attackPosition.transform.position.y );
        bullet.GetComponent<Bullet1>().damage = 10;

        yield return null;

    }

    protected override void Init()
    {
        hp = 10;
    }

    protected override void Hit(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            animator.SetTrigger("die");
            StartCoroutine(Die(0.5f));
            BGMManager.Instance.ClearBGM();
            SceneManager.LoadScene("gameclear");
            
        }
        else
        {
            // sr.color = new Color(1, 1, 1, 0.4f);
        }
    }

    protected override IEnumerator Die(float second)
    {
        yield return new WaitForSeconds(second);

        Destroy(gameObject);
    }
}
