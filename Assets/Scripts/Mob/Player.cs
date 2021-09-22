using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CurrentSkill
{
    timeStop = 0,
    timeSlow = 1,
    timeFast = 2
}

public class Player : MonoBehaviour
{
    private Cinemachine.CinemachineCollisionImpulseSource MyInpulse;

    float moveSpeed = 1000f;
    float currentMoveSpeed = 1000f;


    float jumpForce = 2000f;
    float fastJumpForce = 3000f;

    bool canAttack = true;
    bool canHit = true;

    bool hitting = false;
    private bool attacking = false;


    //�������� ��Ÿ�� ���
    private int timeCool = 4;
    private int currentTimeCool = 4;


    //private int timeStopCool = 4;
    //private int currentTimeStopCool = 4;

    //private int timeSlowCool = 4;
    //private int currentTimeSlowCool = 4;

    //private int timeFastCool = 4;
    //private int currentTimeFastCool = 4;

    public bool timeFast = false;
    public float timeFastNumber = 1.5f;

    //�ɷ�ġ
    //ü��
    public int maxHp;
    public int hp;
    //���ݷ�
    public int attackDamage;

    //상호작용
    public bool interection = false;
    public bool isDead = false;


    float currentAttackDelay = 0;
    float attackDelay = 0.5f;
 

    private Rigidbody2D rigid;
    private Animator animator;
    public Collider2D playerCollider;
    public PlayerAttackEffect effect;

    CurrentSkill currentSkill;

    //������
    private bool isJump = false;
    //���� �÷��̾ �������� �ٶ󺸰� �ִ°�?
    private bool isRight = true;
    //�÷��̾ ���� ���� ��� �ִ°�?
    private bool isGround = true;
    public GameObject mapConfiner;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();

        currentSkill = CurrentSkill.timeStop;

        MyInpulse = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineCollisionImpulseSource>();
    }



    private void Start()
    {
        Debug.Log("test");
        GameManager.Instance.player = GetComponent<Player>();
        LevelManager.Instance.tempPlayer = gameObject;
        
        Init();
        effect = transform.GetChild(0).GetComponent<PlayerAttackEffect>();
    }

    private void UIInit()
    {
        UIManager.Instance.hpText.text = hp.ToString();
        UIManager.Instance.SetPlayerHpImage(maxHp, hp);
    }

    private void Init() {
        Debug.Log("스탯 초기화");
        maxHp = 100;
        hp = maxHp;
        Debug.Log(hp);
        attackDamage = 10;
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetButton("Jump") && !isJump && !attacking)
        {
            Jump();
        }

        //����
        if (canAttack && !attacking && !hitting)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PlayerAttack();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PlayerAttack();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PlayerAttack();

            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PlayerAttack();
            }

        }

        //�ɷ� ��ü
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeSkill();
        }

        //상호장용
        if (Input.GetKeyDown(KeyCode.F))
        {
        }

        //�ɷ� ���
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            switch (currentSkill)
            {
                case CurrentSkill.timeStop:
                    if(currentTimeCool == timeCool)
                    {
                        currentTimeCool = 0;
                        StartCoroutine(TimeStop());
                        UIManager.Instance.SetPlayerSkillImage(timeCool, currentTimeCool);
                    }
                    break;

                case CurrentSkill.timeSlow:
                    if (currentTimeCool == timeCool)
                    {
                        currentTimeCool = 0;
                        StartCoroutine(TimeSlow());
                        UIManager.Instance.SetPlayerSkillImage(timeCool, currentTimeCool);
                    }
                    break;

                case CurrentSkill.timeFast:
                    if (currentTimeCool == timeCool)
                    {
                        currentTimeCool = 0;
                        StartCoroutine(TimeFast());
                        UIManager.Instance.SetPlayerSkillImage(timeCool, currentTimeCool);
                    }
                    break;

            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (!canAttack)
        {
            currentAttackDelay += Time.deltaTime;

            if (currentAttackDelay >= attackDelay)
            {
                currentAttackDelay = 0;
                canAttack = true;
                UIManager.Instance.attackDelayText.text = "���ݰ���";
            }
        }

        animator.SetBool("isGround", isGround);
        GroundCheck();
        PlayerMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" && !attacking)
        {
            AttackObjectInterface ao = collision.gameObject.GetComponent<AttackObjectInterface>();
            if(ao != null)
            {
                OnDamaged(ao.Damage);
            }
            else
            {
                AttackObjectInterface ao2 = collision.gameObject.transform.GetChild(0).GetComponent<AttackObjectInterface>();
                if(ao2 != null)
                {
                    OnDamaged(ao2.Damage);
                }
                else
                {
                    Debug.Log("��¥ ����");
                }
            }
        }

        if(collision.gameObject.tag == "Door") {
            Door door = collision.gameObject.GetComponent<Door>();
            door.Move();
        }

        if (collision.gameObject.CompareTag("BossPortal"))
        {
            Debug.Log("포탈");
            BossMove();
        }
        
    }

    private void BossMove()
    {
        mapConfiner.transform.position = new Vector2(26110f, -28256f);
        transform.position = new Vector2(26110f, -28256f);
    
    }


    //�� üũ
    void GroundCheck()
    {
        Debug.DrawRay(rigid.position, Vector3.down * 110f, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 110f, LayerMask.GetMask("Floor"));

        if (rayHit.collider != null)
        {
            isJump = false;
            isGround = true;
        } else
        {
            isGround = false;
        }
    }



    //�÷��̾� �̵�
    void PlayerMove()
    {
        float axis_X = Input.GetAxisRaw("Horizontal");

        rigid.velocity = new Vector2(axis_X * currentMoveSpeed, rigid.velocity.y);

        if(!attacking )
        {
            if (isRight && axis_X == -1)
            {
                Flip();
            }

            if (!isRight && axis_X == 1)
            {
                Flip();
            }

        }

        //�ִϸ��̼� �Ķ���� ����
        animator.SetFloat("axis_X", Mathf.Abs(axis_X));
    }

    //�÷��̾� ����
    void Jump()
    {
        isJump = true;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        //�ִϸ��̼� �Ķ���� ����
        animator.SetTrigger("isJump");
    }

    //�÷��̾� �ǰ� �̺�Ʈ
    void OnDamaged(int damage)
    {
        MyInpulse.GenerateImpulse(new Vector3(100f, 100f));
        hp -= damage;

        UIManager.Instance.SetPlayerHpImage(maxHp, hp);

        if (hp <= 0)
        {
            StartCoroutine("Die");
        }else
        {
            animator.SetTrigger("isHit");
        }
    }

    void PlayerAttack()
    {
        if (!canAttack) return;
        attacking = true;
        animator.SetTrigger("isAttack");
    }

    IEnumerator InterectionSwitch(){
        interection = true;
        yield return new WaitForSeconds(0.5f);
        interection = false;
    }

    //---�÷��̾� ��ų
    private void SetSkillImage()
    {
        switch (currentSkill)
        {
            case CurrentSkill.timeStop:
                UIManager.Instance.SetPlayerSkillImage(timeCool, currentTimeCool);
                break;

            case CurrentSkill.timeSlow:
                UIManager.Instance.SetPlayerSkillImage(timeCool, currentTimeCool);
                break;

            case CurrentSkill.timeFast:
                UIManager.Instance.SetPlayerSkillImage(timeCool, currentTimeCool);
                break;

        }

    }


    //���� �������� ��ų �� ����
    public void SkillCoolDown()
    {
        switch (currentSkill)
        {
            case CurrentSkill.timeStop:
                currentTimeCool++;
                if (currentTimeCool > timeCool)
                    currentTimeCool = timeCool;

                break;

            case CurrentSkill.timeSlow:
                currentTimeCool++;
                if (currentTimeCool > timeCool)
                    currentTimeCool = timeCool;
                break;

            case CurrentSkill.timeFast:
                currentTimeCool++;
                if (currentTimeCool > timeCool)
                    currentTimeCool = timeCool;
                break;

        }
        SetSkillImage();
    }

    private void ChangeSkill()
    {
        if (currentSkill == CurrentSkill.timeFast)
        {
            currentSkill = CurrentSkill.timeStop;
        }
        else
        {
            currentSkill++;
        }

        UIManager.Instance.ChangePlayerSkillImage(currentSkill);
        SetSkillImage();
    }

    IEnumerator TimeStop()
    {
        GameManager.Instance.TimeStop();

        yield return new WaitForSeconds(3f);

        GameManager.Instance.TimeStop();
    }

    IEnumerator TimeSlow()
    {
        GameManager.Instance.TimeSlow();

        yield return new WaitForSeconds(5f);

        GameManager.Instance.TimeSlow();
    }

    IEnumerator TimeFast()
    {
        currentMoveSpeed = currentMoveSpeed * timeFastNumber;
        animator.speed = animator.speed * timeFastNumber;
        rigid.gravityScale = 2f;
        jumpForce = 3000f;

        yield return new WaitForSeconds(5f);

        currentMoveSpeed = currentMoveSpeed / timeFastNumber;
        animator.speed = animator.speed / timeFastNumber;
        rigid.gravityScale = 1f;
        jumpForce = 2000f;
    }

    //---�÷��̾� ��������Ʈ ��ȯ
    void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator Die()
    {
        isDead = true;

        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(1f);

        //Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
    }


    //---�ִϸ��̼��� �̺�Ʈ �Լ���

    void PlayerAttackOff()
    {
        attacking = false;
        canAttack = false;
    }

    public void PlayerAttackEffect()
    {
        effect.damage = attackDamage;
        effect.animator.SetTrigger("isAttack");
    }


    void PlayerHitOn()
    {
        hitting = true;
    }

    void PlayerhitOff()
    {
        hitting = false;
    }

}
