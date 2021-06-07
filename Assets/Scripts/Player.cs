using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveMaxSpeed = 30000f;
    public float moveSpeed = 45000f;
    public float jumpForce = 2000f;

    //체력
    public int hp;
    //공격력
    public int attackDamage;

    private Rigidbody2D rigid;
    private Animator animator;
    public Collider2D playerCollider;

    //점프중
    private bool isJump = false;
    //점프 횟수
    private int jumpCount = 0;
    //현재 플레이어가 오른쪽을 바라보고 있는가?
    private bool isRight = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Init() {
        hp = 100;
        attackDamage = 10;
    }

    void Update()
    {
        if (Input.GetButton("Jump") && !isJump)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        PlayerMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }

    //땅 체크
    void GroundCheck()
    {
        Debug.DrawRay(rigid.position, Vector3.down * 110f, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 110f, LayerMask.GetMask("Floor"));

        if (rayHit.collider != null)
        {
            isJump = false;
        }
    }

    //플레이어 이동
    void PlayerMove()
    {
        float axis_X = Input.GetAxis("Horizontal");

        if(axis_X != 0)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            rigid.AddForce(Vector2.right * axis_X * moveSpeed);
        }

        if(axis_X < 0 && isRight || axis_X > 0 && !isRight)
        {
            Flip();
        }

        //속도 제한
        if (rigid.velocity.x >= moveMaxSpeed)
        {
            rigid.velocity = new Vector2(2.5f, rigid.velocity.y);
        }
        else if (rigid.velocity.x <= moveMaxSpeed * -1)
        {
            rigid.velocity = new Vector2(-2.5f, rigid.velocity.y);
        }
        //애니메이션 파라미터 설정
        animator.SetFloat("axis_X", Mathf.Abs(axis_X));
    }

    //플레이어 점프
    void Jump()
    {
        isJump = true;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        //애니메이션 파라미터 설정
        animator.SetTrigger("isJump");
    }

    //플레이어 피격 이벤트
    void OnDamaged(Vector2 targetPos)
    {
        //레이어 변경
        //Debug.Log("gg");
        //rigid.velocity = Vector2.zero;
        //int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        //Debug.Log(dirc);
        //rigid.AddForce(new Vector2(dirc, 1)*20, ForceMode2D.Impulse);

        //애니메이션 파라미터 설정
        animator.SetTrigger("isHit");
    }

    //플레이어 스프라이트 전환
    void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
