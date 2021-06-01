using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveMaxSpeed = 20f;
    public float moveSpeed = 10f;
    public float jumpForce = 10f;

    private Rigidbody2D rigid;


    //점프중
    private bool isJump = false;
    //점프 횟수
    private int jumpCount = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump") && !isJump)
        {
            jump();

        }

    }

    private void FixedUpdate()
    {
        groundCheck();
        playerMove();
    }

  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    //땅 체크
    void groundCheck()
    {
        Debug.DrawRay(rigid.position, Vector3.down * 2.1f, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2.1f, LayerMask.GetMask("Floor"));

        if (rayHit.collider != null)
        {
            Debug.Log("땅");
            isJump = false;
        }
    }

    //플레이어 이동
    void playerMove()
    {
        float axis_X = Input.GetAxis("Horizontal");
        if(axis_X != 0)
        {
            rigid.velocity = new Vector2(axis_X * moveSpeed, rigid.velocity.y);
        }
        
    }

    //플레이어 점프
    void jump()
    {
        isJump = true;
        Debug.Log("실행");
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    //플레이어 피격 이벤트
    void OnDamaged(Vector2 targetPos)
    {
        //레이어 변경
        Debug.Log("gg");
        rigid.velocity = Vector2.zero;
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        Debug.Log(dirc);
        rigid.AddForce(new Vector2(dirc, 1)*20, ForceMode2D.Impulse);
    }


}
