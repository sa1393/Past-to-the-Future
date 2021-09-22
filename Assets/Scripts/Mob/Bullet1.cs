using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour, AttackObjectInterface
{
    private float speed = 400f;
    private bool isHit = false;

    private Rigidbody2D rigid;
    private Animator animator;

    GameObject player;
    Vector2 vec;

    public int damage = 0;
    public int Damage { get => damage; set => damage = value; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update(){
        if(GameManager.Instance.timeStop){
            rigid.velocity = new Vector2(0, 0);
            return;
        }
        else {
            rigid.velocity = new Vector2(vec.normalized.x * speed, vec.normalized.y * speed);
        }

        if(GameManager.Instance.timeSlow){
            rigid.velocity = new Vector2(vec.normalized.x * speed / 2, vec.normalized.y * speed / 2);
        }
        else {
            rigid.velocity = new Vector2(vec.normalized.x * speed, vec.normalized.y * speed);
        }

    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        vec = player.transform.position - gameObject.transform.position;

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
        gameObject.transform.localScale *= 1.2f;
        animator.SetTrigger("destroy");

        yield return new WaitForSeconds(second);

        Destroy(gameObject);
    }

    IEnumerator SelfDie()
    {
        yield return new WaitForSeconds(6f);

        Destroy(gameObject);
    }
}
