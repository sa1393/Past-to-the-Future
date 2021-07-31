using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour, AttackObjectInterface
{
    private float speed = 400f;
    private bool isHit = false;

    private Rigidbody2D rigid;
    private Animator animator;

    public int damage = 0;
    public int Damage { get => damage; set => damage = value; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 vec = player.transform.position - gameObject.transform.position;

        if(!isHit)
        {
            rigid.velocity = new Vector2(vec.normalized.x * speed, vec.normalized.y * speed);
        }

        StartCoroutine(SelfDie());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "floor")
        {
            isHit = true;
            StartCoroutine(Die(0.2f));
        }
    }


    IEnumerator Die(float second)
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
