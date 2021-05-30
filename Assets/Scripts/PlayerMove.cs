using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 5f;

    private Rigidbody2D rigid;
    private bool jumpEnable = true;

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
        float axis_X = Input.GetAxis("Horizontal");

        rigid.velocity = new Vector2(axis_X * moveSpeed, rigid.velocity.y);

        if (jumpEnable && Input.GetKeyDown(KeyCode.Space))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 3 * jumpForce);
            Debug.Log("점프 함");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            jumpEnable = true;
            Debug.Log("땅에 닿음");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            jumpEnable = false;
            Debug.Log("공중에 떠있음");
        }
    }
}
