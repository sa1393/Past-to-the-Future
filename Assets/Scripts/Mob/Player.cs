using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CurrentSkill
{
    timeStop = 0,
    timeSlow = 1,
    timeFast = 2
}


public class Player : LifeObject
{
    private Cinemachine.CinemachineCollisionImpulseSource MyInpulse;

    float moveSpeed = 1000f;
    float currentMoveSpeed = 1000f;
    float jumpForce = 2000f;


    bool canAttack = true;
    bool canHit = true;

    bool hitting = false;
    private bool attacking = false;


    //데미지로 쿨타임 계산
    private int timeStopCool = 4;
    private int currentTimeStopCool = 4;

    private int timeSlowCool = 4;
    private int currentTimeSlowCool = 4;

    private int timeFastCool = 4;
    private int currentTimeFastCool = 4;

    public bool timeFast = false;
    public float timeFastNumber = 1.5f;

    //능력치
    //체력
    public int maxHp;
    public int hp;
    //공격력
    public int attackDamage;


    float currentAttackDelay = 0;
    float attackDelay = 0.5f;
 

    private Rigidbody2D rigid;
    private Animator animator;
    public Collider2D playerCollider;
    public PlayerAttackEffect effect;

    CurrentSkill currentSkill;

    //점프중
    private bool isJump = false;
    //현재 플레이어가 오른쪽을 바라보고 있는가?
    private bool isRight = true;
    //플레이어가 현재 땅을 밟고 있는가?
    private bool isGround = true;

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
        Init();
        effect = transform.GetChild(0).GetComponent<PlayerAttackEffect>();
    }

    private void UIInit()
    {
        UIManager.Instance.hpText.text = hp.ToString();
        UIManager.Instance.SetPlayerHpImage(maxHp, hp);
    }

    private void Init() {
        maxHp = 100;
        hp = maxHp;
        attackDamage = 1;
    }

    void Update()
    {
        if (Input.GetButton("Jump") && !isJump && !attacking)
        {
            Jump();
        }

        //공격
        if (Input.GetMouseButton(0) && canAttack && !attacking && !hitting)
        {
            PlayerAttack();
        }

        //능력 교체
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeSkill();
        }

        //능력 사용
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            switch (currentSkill)
            {
                case CurrentSkill.timeStop:
                    if(currentTimeStopCool == timeStopCool)
                    {
                        currentTimeStopCool = 0;
                        StartCoroutine(TimeStop());
                        UIManager.Instance.SetPlayerSkillImage(timeStopCool, currentTimeStopCool);
                    }
                    break;

                case CurrentSkill.timeSlow:
                    if (currentTimeSlowCool == timeSlowCool)
                    {
                        currentTimeSlowCool = 0;
                        StartCoroutine(TimeSlow());
                        UIManager.Instance.SetPlayerSkillImage(timeSlowCool, currentTimeSlowCool);
                    }
                    break;

                case CurrentSkill.timeFast:
                    if (currentTimeFastCool == timeFastCool)
                    {
                        currentTimeFastCool = 0;
                        StartCoroutine(TimeFast());
                        UIManager.Instance.SetPlayerSkillImage(timeFastCool, currentTimeFastCool);
                    }
                    break;

            }
        }
    }

    private void FixedUpdate()
    {
        if (!canAttack)
        {
            currentAttackDelay += Time.deltaTime;

            if (currentAttackDelay >= attackDelay)
            {
                currentAttackDelay = 0;
                canAttack = true;
                UIManager.Instance.attackDelayText.text = "공격가능";
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
            Enemy enemy = collision.gameObject.transform.parent.GetChild(0).GetComponent<Enemy>();
            OnDamaged(enemy.attackDamage);
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
            isGround = true;
        } else
        {
            isGround = false;
        }
    }



    //플레이어 이동
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
    void OnDamaged(int damage)
    {
        MyInpulse.GenerateImpulse(new Vector3(100f, 100f));
        hp -= damage;

        UIManager.Instance.hpText.text = "hp : " + hp;
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

    //---플레이어 스킬
    private void SetSkillImage()
    {
        switch (currentSkill)
        {
            case CurrentSkill.timeStop:
                UIManager.Instance.SetPlayerSkillImage(timeStopCool, currentTimeStopCool);
                break;

            case CurrentSkill.timeSlow:
                UIManager.Instance.SetPlayerSkillImage(timeSlowCool, currentTimeSlowCool);
                break;

            case CurrentSkill.timeFast:
                UIManager.Instance.SetPlayerSkillImage(timeFastCool, currentTimeFastCool);
                break;

        }

    }


    //적을 때렸을때 스킬 쿨 관련
    public void SkillCoolDown()
    {
        switch (currentSkill)
        {
            case CurrentSkill.timeStop:
                currentTimeStopCool++;
                if (currentTimeStopCool > timeStopCool)
                    currentTimeStopCool = timeStopCool;

                break;

            case CurrentSkill.timeSlow:
                currentTimeSlowCool++;
                if (currentTimeSlowCool > timeSlowCool)
                    currentTimeSlowCool = timeSlowCool;
                break;

            case CurrentSkill.timeFast:
                currentTimeFastCool++;
                if (currentTimeFastCool > timeFastCool)
                    currentTimeFastCool = timeFastCool;
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

        yield return new WaitForSeconds(5f);

        currentMoveSpeed = currentMoveSpeed / timeFastNumber;
        animator.speed = animator.speed / timeFastNumber;
    }

    //---플레이어 스프라이트 전환
    void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator Die()
    {
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }


    //---애니메이션중 이벤트 함수들

    void PlayerAttackOff()
    {
        attacking = false;
        canAttack = false;
        UIManager.Instance.attackDelayText.text = "공격불가능";
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
