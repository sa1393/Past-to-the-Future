using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : Enemy
{
    private float speed = 400f;
    private bool isHit = false;

    private Rigidbody2D rigid;
    private Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 vec = player.transform.position - gameObject.transform.position;

        if(!isHit)
        {
            rigid.velocity = new Vector2(vec.normalized.x * speed, vec.normalized.y * speed);
        }

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "floor")
        {
            isHit = true;
            StartCoroutine(Die(0.2f));
        }
    }

    protected override void Init()
    {
        attackDamage = 10;

        StartCoroutine(SelfDie());
    }

    protected override void Hit(int damage)
    {
        
    }

    protected override IEnumerator Die(float second)
    {
        gameObject.transform.localScale *= 1.4f;
        animator.SetTrigger("destroy");

        yield return new WaitForSeconds(second);

        Destroy(gameObject);
    }

    IEnumerator SelfDie()
    {
        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }
}
