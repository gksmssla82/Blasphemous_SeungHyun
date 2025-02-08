using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





public class Boss_Pietat: Enemy
{
    private enum State
    {
        INTRO,
        IDLE,
        WALK,
        TURNAROUND,
        SLASH,
        STOMP,
        SMASH,
        SPLIT,
        DEATH
    }
    private State m_CurrentState;

    public bool m_isTurnAround;
    public bool m_isSlash;
    public bool m_isStomp;
    public bool m_isSmash;
    public bool m_isSplit;

    public GameObject m_WalkDust;
    public GameObject m_ThronTower;
    public GameObject m_Thron;
    public Transform m_ThronFire;

    private bool m_ThronThrow1 = false;
    private bool m_ThronThrow2 = false;
    private bool m_ThronThrow3 = false;

    public Collider2D m_PlayerColider;
    public Collider2D m_PlatformColider;

    public int[] m_ThronSpeedValues = { 50, 100, 150, 200, 250};

    public Image m_Hpbar;
    public Image m_Hpbar_linear;
    private Coroutine m_hpCoroutine;

    public float m_BossHp;
    public float m_BossMaxHP;


    public float m_XEffect;
    public float m_YEffect;

    public BoxCollider2D m_SlashColider;
    public BoxCollider2D m_StompColider;
    public BoxCollider2D m_SmashColider;

    public GameObject m_BossUI;
    public GameObject m_BossObject;



    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void Start()
    {
        base.Start();
        m_BossHp = m_BossMaxHP;

        TransitionToState(State.INTRO);
    }
    
    protected override void Update()
    {
        m_TagetDistance = Vector3.Distance(this.transform.position, m_Taget.transform.position);

        Dead_Check();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            //TransitionToState(State.SPLIT);
            m_Anim.SetTrigger("IntroTrriger");
        }


        switch (m_CurrentState)
        {
            case State.IDLE:
                Update_Idle();
                break;
            case State.WALK:
                Update_Walk();
                break;
            case State.TURNAROUND:
                Update_TurnAround();
                break;
            case State.SLASH:
                Update_Slash();
                break;
            case State.STOMP:
                Update_Stomp();
                break;
            case State.SMASH:
                Update_Smash();
                break;
            case State.SPLIT:
                Update_Split();
                break;
            case State.INTRO:
                Update_Intro();
                break;

        }
    }

    private void TransitionToState(State _newState)
    {
        m_CurrentState = _newState;

        switch (_newState)
        {
            case State.IDLE:
                Enter_Idle();
                break;
            case State.WALK:
                Enter_Walk();
                break;
            case State.TURNAROUND:
                Enter_TurnAround();
                break;
            case State.SLASH:
                Enter_Slash();
                break;
            case State.STOMP:
                Enter_Stomp();
                break;
            case State.SMASH:
                Enter_Smash();
                break;
            case State.SPLIT:
                Enter_Split();
                break;
            case State.INTRO:
                Enter_Intro();
                break;

        }
    }

    #region STATE
    private void Enter_Idle()
    {
        Debug.Log("보스 Idle 상태 진입");
        
    }

    private void Update_Idle()
    {
        if (Player_BehindCheck() && m_Turn)
        {
            Debug.Log("플레이어가 보스의 뒤에있습니다");
            m_Turn = false;
            TransitionToState(State.TURNAROUND);
        }


        if (m_isTracing)
        {
            TransitionToState(State.WALK);
        }

        else if (m_isAttack)
        {
            int Temp = Random.Range(1, 5);

            switch (Temp)
            {
                case 1:
                    TransitionToState(State.SLASH);
                    break;
                case 2:
                    TransitionToState(State.STOMP);
                    break;
                case 3:
                    TransitionToState(State.SMASH);
                    break;
                case 4:
                    TransitionToState(State.SPLIT);
                    break;
            }
        }
    }

    private void Enter_Walk()
    {
        Debug.Log("보스 Walk 상태 진입");
        RandomSound_Temp2("Pietat_WalkVoice1", "Pietat_WalkVoice2");
    }

    private void Update_Walk()
    {
        if (m_isTracing)
        {
            Look_Player();
            m_Anim.SetBool("Walk", true);
            m_Rigid.velocity = new Vector2(m_XDir * m_Speed, m_Rigid.velocity.y);
        }

        else
        {
            m_Anim.SetBool("Walk", false);
            AudioManager.Instance.Play("Pietat_StopVoice");
            AudioManager.Instance.Play("Pietat_Stop");
            TransitionToState(State.IDLE);
        }
    }

    private void Enter_TurnAround()
    {
        Debug.Log("보스 TurnAround 상태 진입");
        m_Anim.SetTrigger("Turn");
        
    }

    private void Update_TurnAround()
    {
        if (IsAnimationFinished("TurnAround"))
        {
            TransitionToState(State.IDLE);
        }
    }

    private void Enter_Slash()
    {
        Debug.Log("보스 Slash 상태 진입");
        m_Anim.SetTrigger("Slash");
        EnableCollision(false);
        
    }

    private void Update_Slash()
    {
       
        if (IsAnimationFinished("Slash"))
        {
            EnableCollision(true);
            TransitionToState(State.IDLE);
            
        }


    }

    private void Enter_Stomp()
    {
        Debug.Log("보스 Stomp 상태 진입");
        m_Anim.SetTrigger("Stomp");
        EnableCollision(false);
        
    }

    private void Update_Stomp()
    {
        
        if (IsAnimationFinished("Stomp"))
        {
            EnableCollision(true);
            TransitionToState(State.IDLE);
           
        }

       
    }

    private void Enter_Smash()
    {
        Debug.Log("보스 Smash 상태 진입");
        m_Anim.SetTrigger("Smash");
        EnableCollision(false);
        AudioManager.Instance.Play("Pietat_SmashVoice");
    }

    private void Update_Smash()
    {
        


        if (IsAnimationFinished("Smash_to_IdleRight"))
        {
            EnableCollision(true);
            m_Anim.SetBool("Smash_To_Idle_Reverse", false);
            TransitionToState(State.IDLE);
           
        }

        else if (IsAnimationFinished("Smash_To_IdleReverse"))
        {
            EnableCollision(true);
            m_Anim.SetBool("Smash_To_Idle_Reverse", false);
            TransitionToState(State.IDLE);
        }
    }

    private void Enter_Split()
    {
        Debug.Log("보스 Split 상태 진입");


        int Temp = Random.Range(1, 4);

        switch (Temp)
        {
            case 1:
                m_Anim.SetTrigger("Split");
                m_ThronThrow1 = true;
                break;
            case 2:
                m_Anim.SetTrigger("Split");
                m_ThronThrow2 = true;
                m_Anim.SetBool("Split2", true);
                break;
            case 3:
                m_Anim.SetTrigger("Split");
                m_ThronThrow3 = true;
                m_Anim.SetBool("Split2", true);
                m_Anim.SetBool("Split3", true);
                break;
        }
    }

    private void Update_Split()
    {
        if (IsAnimationFinished("Split_End"))
        {
            m_Anim.SetBool("Split2", false);
            m_Anim.SetBool("Split3", false);
            m_ThronThrow1 = false;
            m_ThronThrow2 = false;
            m_ThronThrow3 = false;
            TransitionToState(State.IDLE);
        }
    }

    private void Enter_Intro()
    {
        Debug.Log("보스 Intro 상태 진입");
        m_Anim.SetTrigger("Intro");
    }

    private void Update_Intro()
    {
        
    }
    #endregion


    private void Chase()
    {
        if (m_isTracing && !m_isAttacking)
        {
            LookAtTaget();

            m_Rigid.velocity = new Vector2(m_XDir * m_Speed, m_Rigid.velocity.y);
        }
    }

    private void Look_Player()
    {
        if (!m_Turn)
        {
            if (m_Taget.transform.position.x < this.transform.position.x)
            {
                m_XDir = -1;
                this.transform.localScale = new Vector3(m_XDir, 1, 1);
            }

            else if (m_Taget.transform.position.x > this.transform.position.x)
            {
                m_XDir = 1;
                this.transform.localScale = new Vector3(m_XDir, 1, 1);
            }
        }
    }

    private bool IsAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = m_Anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 0.95f;
    }

    private bool IsAnimationFinished2(string animationName)
    {
        AnimatorStateInfo stateInfo = m_Anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 0.8f;
    }


    private void EnableCollision(bool enable)
    {
        Physics2D.IgnoreCollision(m_BoxCollider, m_PlayerColider, !enable);
    }

    #region Event_Anim
    public void EventAnim_WalkDust()
    {
        RandomSound_Temp2("Pietat_Step1", "Pietat_Step2");
        Create_Prefabs(m_WalkDust, 37f, 0);
        StartCoroutine(m_Camera.Shake(0.2f, 5f, new Vector2(0, 1)));
    }

    public void EventAnim_Stomp()
    {
        RandomSound_Temp2("Pietat_Step1", "Pietat_Step2");
        Create_Prefabs(m_WalkDust, 68f, 0f);
        m_StompColider.enabled = true;
        StartCoroutine(m_Camera.Shake(0.2f, 10f, new Vector2(0, 1)));
    }

    public void EventAnim_ThornTower()
    {
        m_StompColider.enabled = false;
        Instantiate(m_ThronTower, new Vector3(m_Taget.transform.position.x, m_PlatformColider.transform.position.y,
                this.transform.position.z), Quaternion.identity);
    }

    public void EventAnim_Smash1()
    {
        m_SmashColider.enabled = true;
        StartCoroutine(m_Camera.Shake(0.2f, 10f, new Vector2(0, 1)));
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x + 80, m_PlatformColider.transform.position.y,
                this.transform.position.z), Quaternion.identity);
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x - 80, m_PlatformColider.transform.position.y,
                this.transform.position.z), Quaternion.identity);
    }

    public void EventAnim_Smash2()
    {
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x + 120, m_PlatformColider.transform.position.y,
               this.transform.position.z), Quaternion.identity);
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x - 120, m_PlatformColider.transform.position.y,
                this.transform.position.z), Quaternion.identity);
    }

    public void EventAnim_Smash3()
    {
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x + 160, m_PlatformColider.transform.position.y,
              this.transform.position.z), Quaternion.identity);
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x - 160, m_PlatformColider.transform.position.y,
                this.transform.position.z), Quaternion.identity);
    }

    public void EventAnim_Smash4()
    {
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x + 200, m_PlatformColider.transform.position.y,
              this.transform.position.z), Quaternion.identity);
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x - 200, m_PlatformColider.transform.position.y,
                this.transform.position.z), Quaternion.identity);
    }

    public void EventAnim_Smash5()
    {
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x + 240, m_PlatformColider.transform.position.y,
              this.transform.position.z), Quaternion.identity);
        Instantiate(m_ThronTower, new Vector3(this.transform.position.x - 240, m_PlatformColider.transform.position.y,
                this.transform.position.z), Quaternion.identity);
    }

    public void EventAnim_SmashScream()
    {
        AudioManager.Instance.Play("Pietat_SmashScream");
    }

    public void EventAnim_SmashWave()
    {
        AudioManager.Instance.Play("Pietat_SmashWave");
    }

    public void EventAnim_SmashGetUp()
    {
        AudioManager.Instance.Play("Pietat_SmashGetup");
    }

    

    public void Anim_Turn()
    {
        
        m_Turn = false;
        m_XDir *= -1;
        this.transform.localScale = new Vector3(m_XDir, 1, 1);
    }

    public void Anim_SmashCheck()
    {
        m_SmashColider.enabled = false;
        if (Player_BehindCheck())
        {
            m_Anim.SetBool("Smash_To_Idle_Reverse", true);
        }
    }

    public void Anim_StompSound()
    {
        AudioManager.Instance.Play("Pietat_StompVoice");
        AudioManager.Instance.Play("Pietat_Stomp");
    }

    public void Anim_TurnSound()
    {
        AudioManager.Instance.Play("Pietat_TurnVoice");
        AudioManager.Instance.Play("Pietat_Turn");
    }

    public void Anim_SlashSound1()
    {
        AudioManager.Instance.Play("Pietat_SlashVoice");
        
    }

    public void Anim_SlashSound2()
    {
        AudioManager.Instance.Play("Pietat_Slash");
    }

    public void Anim_Walk()
    {
        RandomSound_Temp2("Pietat_Walk1", "Pietat_Walk2");
    }

    public void Anim_SplitSound()
    {
        if (m_ThronThrow1)
        {
            AudioManager.Instance.PlaySegment("Pietat_Split", 0f, 1.5f);
        }

        else if (m_ThronThrow2)
        {
            AudioManager.Instance.PlaySegment("Pietat_Split", 0f, 2.0f);
        }

        else if (m_ThronThrow3)
        {
            AudioManager.Instance.Play("Pietat_Split");
        }
        
        else
        {
            Debug.Log("ThoronThrow가 전부 Null");
        }
    }

    public void Anim_SlashColider_On()
    {
        m_SlashColider.enabled = true;
    }

    public void Anim_SlashColider_Off()
    {
        m_SlashColider.enabled = false;
    }

    public void Anim_Pietat_Dead()
    {
        AudioManager.Instance.Play("Pietat_DeadVoice");
        AudioManager.Instance.Play("Pietat_Dead");
        StartCoroutine(BossUI_Corutine());
    }

    public void Anim_Intro()
    {
        BgmManager.Instance.Stop();
        AudioManager.Instance.Play("Pietat_WakeUp");
    }

    public void Anim_IntroFinish()
    {
        m_BossObject.SetActive(true);
        m_Camera.m_PlayerChase = true;
        BgmManager.Instance.Play(2);
        m_Taget.TransitonToState(PlayerStateType.Idle);
        TransitionToState(State.IDLE);
        m_BossUI.SetActive(true);
    }
    

    public void Throw_Thron()
    {



        // 돌 프리펩 생성
        GameObject Stone = Instantiate(m_StonePrefab, m_ThrowPoint.position, Quaternion.identity);

        // 목표 위치 계산

        Vector3 TargetPosition = m_Taget.transform.position;
        TargetPosition.y += m_ThorowHeight;

        int RandomValue = RandomArryValue();

        Vector3 Direction = StoneThrow_Direction(TargetPosition, m_ThrowPoint.position, RandomValue);

        // 돌의 초기 속도
        Rigidbody2D Rigid = Stone.GetComponent<Rigidbody2D>();
        Rigid.velocity = Direction;
    }

    public Vector3 StoneThrow_Direction(Vector3 _Target, Vector3 _Origin, int _Speed)
    {
        // 방향 계산
        Vector3 direction = _Target - _Origin;
        float distance = direction.magnitude;

        // 던질 각도를 라디안으로 변환
        float radians = Mathf.Deg2Rad * 45f; // 던질 각도를 45도로 설정 (원하는 각도로 변경 가능)

        // 중력 계산
        float gravity = Mathf.Abs(Physics2D.gravity.y);

        // 초기 속도 계산
        float initialVelocityX = _Speed * Mathf.Cos(radians);
        float initialVelocityY = _Speed * Mathf.Sin(radians);

        // 속도 벡터 계산
        Vector3 velocity = new Vector3(
            initialVelocityX * Mathf.Sign(direction.x), // x 방향 속도
            initialVelocityY, // y 방향 속도
            0);

        return velocity;
    }

    #endregion

    private IEnumerator BossUI_Corutine()
    {
        yield return new WaitForSeconds(2f);
        m_BossUI.SetActive(false);
        Score(1000);
    }

    private int RandomArryValue()
    {
        int RandomArry = Random.Range(0, m_ThronSpeedValues.Length);

        return m_ThronSpeedValues[RandomArry];
    }

    private IEnumerator LerpHealthBar(float _StartValue, float _EndValue, float _Duration)
    {
        float elapsd = 0f;

        while (elapsd < _Duration)
        {
            elapsd += Time.deltaTime;
            float LerpValue = Mathf.Lerp(_StartValue, _EndValue, elapsd / _Duration);
            m_Hpbar_linear.fillAmount = LerpValue / m_BossMaxHP; 
            yield return null;
            
        }

        m_Hpbar_linear.fillAmount = _EndValue / m_BossMaxHP;
    }


    private void UpdateHpBar()
    {
        m_Hpbar.fillAmount = m_BossHp / m_BossMaxHP;
        
    }

  

    public void TakeDamage(float _Damage)
    {
        m_BossHp -= _Damage;
        UpdateHpBar();

        if (m_hpCoroutine != null)
        {
            StopCoroutine(m_hpCoroutine);
        }

        m_hpCoroutine = StartCoroutine(LerpHealthBar(m_BossHp + _Damage, m_BossHp, 0.5f));
    }


    private void Dead_Check()
    {
        if (m_BossHp <= 0f)
        {
            m_Anim.SetTrigger("Death");
            
            
        }
    }

    private void RandomSound_Temp2(string _PlayName1, string _PlayName2)
    {
        int Temp = Random.Range(1, 3);

        switch (Temp)
        {
            case 1:
                AudioManager.Instance.Play(_PlayName1);
                break;
            case 2:
                AudioManager.Instance.Play(_PlayName2);
                break;
        }
    }
}
