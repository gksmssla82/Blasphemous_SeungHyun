
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int m_Score;
    public int m_MaxHp;
    public int m_Hp;
    public float m_Speed;
    public bool m_isTracing;
    public int m_NextMove;
    public float m_DotProduct;

    public bool m_Dead;
    public float m_DeadTime;

    public bool m_Parry;
    public bool m_Turn;
   
    public int m_XDir;

    public PlayerController m_Taget;
    public GameManager m_GameManager;
    public float m_TagetDistance;
    public float m_AttackRange = 60f;

    public float m_TurnTime;


   


    // Stone
    public GameObject m_StonePrefab;
    public Transform m_ThrowPoint; // 돌 발사 포지션
    public float m_ThrowSpeed = 10f; // 돌 스피드
    public float m_ThrowAngle = 45f; // 돌 던지는 각도
    public float m_ThorowHeight; // 돌 높이
    public float m_Gravity;

    // Attack
    public bool m_isAttack;
    public bool m_isCanAttack;
    public bool m_isAttacking;
    public float m_AttckTime;
    public float m_AttackCoolTime;

    protected Rigidbody2D m_Rigid;
    protected BoxCollider2D m_BoxCollider;

    // Colider
    public BoxCollider2D m_TraceColider;
    public BoxCollider2D m_HitBoxColider;
    public BoxCollider2D m_AttackColider;
    public BoxCollider2D m_BlockColider;
    public CircleCollider2D m_ThrowColider;

    public Animator m_Anim;
    public CameraManager m_Camera;
    protected LayerMask m_Layer;
    protected virtual void Awake()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
        m_BoxCollider = GetComponent<BoxCollider2D>();
        m_Anim = GetComponent<Animator>();
        if (m_Taget == null)
        {
            m_Taget = Object.FindAnyObjectByType<PlayerController>();
        }
        if (m_GameManager == null)
        {
            m_GameManager = Object.FindAnyObjectByType<GameManager>();
        }

       

    }

    protected virtual void Start()
    {
        m_Hp = m_MaxHp;
        m_XDir = -1;
      
    }



    // Update is called once per frame
    protected virtual void Update()
    {
        m_TagetDistance = Vector3.Distance(this.transform.position, m_Taget.transform.position);
        this.transform.localScale = new Vector3(m_XDir, 1, 1);

        DeadCheck();
    }

    protected virtual void FixedUpdate()
    {
        
    }


    #region Corutine
    public void NormalAttack_Corutine()
    {
        StartCoroutine(NormalAttack());
    }
    private IEnumerator NormalAttack()
    {
        m_isCanAttack = false;
        m_isAttacking = true;
        m_Anim.SetBool("Attack", true);
        yield return new WaitForSeconds(m_AttckTime);
        m_isAttacking = false;
        m_Anim.SetBool("Attack", false);
        LookAtTaget();
        yield return new WaitForSeconds(m_AttackCoolTime);
        m_isCanAttack = true;
    }

    
    #endregion
    #region Animation_Event

    public void EnventAnim_AttackColider_On()
    {
        m_AttackColider.enabled = true;
    }

    public void EvnetAnim_AttackColider_Off()
    {
        m_AttackColider.enabled = false;
    }

    public void EventAnim_ThorwColider_On()
    {
        m_ThrowColider.enabled = true;
    }

    public void EventAnim_ThorwColider_Off()
    {
        m_ThrowColider.enabled = false;
    }



    public void EventAnim_ParryFalse()
    {
        m_Anim.SetBool("Parry", false);
        m_Parry = false;
    }

    public void EventAnim_CanAttackTrue()
    {
        m_isCanAttack = true;
    }

    public void EventAnim_CanAttackFalse()
    {
        m_isCanAttack = false;
    }

    public void EvnentAnim_Dead()
    {
        Score(m_Score);
  
        if (m_GameManager != null)
        {
            m_GameManager.DeactiavateEnemy(this.gameObject);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        m_Hp = m_MaxHp;
        m_isCanAttack = true;
    }

    public void EventAnim_Turn()
    {
        m_Anim.SetBool("Turn", false);
        m_Turn = false;
        m_XDir *= -1;
    }

    public void EventAnim_StonerDead()
    {
        AudioManager.Instance.Play("Stoner_Death");
    }

    public void EventAnim_StonerRising()
    {
        AudioManager.Instance.Play("Stoner_Rising");
    }

    public void EventAnim_ShiledMaidenDead()
    {
        AudioManager.Instance.Play("ShieldMaiden_Death");
    }

    public void EventAnim_LeonDead()
    {
        AudioManager.Instance.Play("LEON_Dead");
    }

    public void EventAnim_LeonAttack()
    {
        AudioManager.Instance.Play("Leon_Hit");
    }
    #endregion

   
    protected void SetAllColider_Enable(bool _Value)
    {
        if (m_TraceColider != null)
        {
            m_TraceColider.enabled = _Value;
        }

        if (m_HitBoxColider != null)
        {
            m_HitBoxColider.enabled = _Value;
        }

        if (m_BlockColider != null)
        {
            m_BlockColider.enabled = _Value;
        }

        if (m_ThrowColider != null)
        {
            m_ThrowColider.enabled = _Value;
        }

        
    }
    protected void DeadCheck()
    {
        if (m_Hp <= 0)
        {
            m_Dead = true;

            if (m_AttackColider != null)
            {
                m_AttackColider.enabled = false;
            }
            m_Anim.SetTrigger("Dead");

            SetAllColider_Enable(false);

        }

        else if (m_Hp != 0)
        {
            m_Dead = false;
            SetAllColider_Enable(true);
        }
    }
    protected void LookAtTaget()
    {
        if (!m_Turn)
        {
            if (m_Taget.transform.position.x < this.transform.position.x)
            {
                m_XDir = -1;
            }

            else if (m_Taget.transform.position.x > this.transform.position.x)
            {
                m_XDir = 1;
            }
        }
    }

    protected void Normal_Move()
    {
        if(!m_isTracing && !m_isAttacking && !m_isAttack && !m_Dead)
        {
            if (m_NextMove == 0)
            {
                m_Anim.SetBool("Walk", false);
            }
            else
            {
                m_Anim.SetBool("Walk", true);
            }

            m_Rigid.velocity = new Vector2(m_NextMove * m_Speed, m_Rigid.velocity.y);

            if (m_NextMove == 1)
            {
                m_XDir = 1;
            }

            else if (m_NextMove == -1)
            {
                m_XDir = -1;
            }

            Vector2 FrontVec = new Vector2(m_Rigid.position.x + m_NextMove, m_Rigid.position.y);
            Debug.DrawRay(FrontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D RayHit = Physics2D.Raycast(FrontVec, Vector3.down, 1, LayerMask.GetMask("Grounds"));

            if (RayHit.collider == null)
            {
                m_NextMove *= -1;
                CancelInvoke();
                Invoke("Next_Move", 3);
            }
        }
    }
    protected void Next_Move()
    {
        m_NextMove = Random.Range(-1, 2);
        int Duration = Random.Range(0, 6);
        Invoke("Next_Move", Duration);
    }
   
    protected void Tracing()
    {
        if (m_isTracing && !m_isAttacking)
        {
            LookAtTaget();

            m_Rigid.velocity = new Vector2(m_XDir * m_Speed, m_Rigid.velocity.y);
        }
         
    }

    public bool Player_BehindCheck()
    {
        
            // 몬스터 전방백터
            Vector3 Foward = this.transform.right;

            if (this.transform.localScale.x < 0)
            {
                Foward = -this.transform.right;
            }

            // 몬스터와 타겟 사이의 백터
            Vector3 ToPlayer = m_Taget.transform.position - this.transform.position;

            // 백터의 내적을 사용해 플레이어가 몬스터의 뒤에 있는지 판단

            float m_DotProduct = Vector3.Dot(Foward, ToPlayer);

            return m_DotProduct < 0;
        
    }


    public void ThrowStone()
    {
        AudioManager.Instance.Play("Stoner_ThrowRock");
        // 돌 프리팹 생성
        GameObject Stone = Instantiate(m_StonePrefab, m_ThrowPoint.position, Quaternion.identity);

        // 목표 위치 계산
        Vector3 targetPosition = m_Taget.transform.position;
        Vector3 direction = StoneThrow_Direction(targetPosition, m_ThrowPoint.position, m_ThrowSpeed, m_ThrowAngle, m_Gravity);

        // 돌의 초기 속도
        Rigidbody2D rigid = Stone.GetComponent<Rigidbody2D>();
        rigid.velocity = direction;
        rigid.gravityScale = m_Gravity / Physics2D.gravity.y;

    }
    public Vector3 StoneThrow_Direction(Vector3 _Target, Vector3 _Origin, float _ThrowSpeed, float _ThrowAngle, float _Gravity)
    {
        //float radianAngle = _ThrowAngle * Mathf.Deg2Rad;

        //// 목표 위치와 시작 위치 사이의 거리 계산
        //float distanceX = _Target.x - _Origin.x;
        //float distanceY = _Target.y - _Origin.y;

        //// 던질 방향 계산
        //bool isTargetOnLeft = distanceX < 0;

        //// 초기 속도 벡터 계산
        //float initialVelocityX = _ThrowSpeed * Mathf.Cos(radianAngle);
        //float initialVelocityY = _ThrowSpeed * Mathf.Sin(radianAngle);

        //// 중력 값
        //float gravity = Mathf.Abs(Physics2D.gravity.y);

        //// 속도 벡터 계산
        //float time = distanceX / initialVelocityX;

        //// 목표 지점에 도달할 시간을 계산하여 y축 속도에 중력 보정
        //initialVelocityY += (gravity * time) / 2;

        //// 목표 위치가 왼쪽에 있을 경우 x 방향 속도 반전
        //if (isTargetOnLeft)
        //{
        //    initialVelocityX = -initialVelocityX;
        //}

        //return new Vector3(initialVelocityX, initialVelocityY, 0);


        float gravity = m_Gravity;

        // 거리 계산
        float distanceX = _Target.x - _Origin.x;
        float distanceY = _Target.y - _Origin.y;

        // 던질 각도 계산
        float angle = Mathf.Atan2(distanceY, distanceX);

        // 초기 속도 계산
        float velocityX = _ThrowSpeed * Mathf.Cos(angle);
        float velocityY = _ThrowSpeed * Mathf.Sin(angle);

        // 중력 보정
        float time = distanceX / velocityX;
        velocityY += 0.5f * gravity * time;

        //// 목표 위치가 왼쪽에 있을 경우 x 방향 속도 반전
        //if (distanceX < 0)
        //{
        //    velocityX = -velocityX;
        //}

        return new Vector2(velocityX, velocityY);
    }

    


    protected void Score(int _Score)
    {
        m_GameManager.AddScore(_Score);
    }

    protected void Create_Prefabs(GameObject _Prefabs, float _x, float _y)
    {
        _Prefabs.transform.localScale = this.transform.localScale;

        if (_Prefabs.transform.localScale == new Vector3(-1, 1, 1))
        {
            _x *= -1;
        }

        else
        {
            _x *= 1;
        }

        Instantiate(_Prefabs, new Vector3(this.transform.position.x + _x, this.transform.position.y + _y, this.transform.position.z), Quaternion.identity);
    }
}


    



