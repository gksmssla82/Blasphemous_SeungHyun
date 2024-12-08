using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Objects
    public CameraManager m_Camera;
    public GameManager m_GameManager;
    private Priedieu m_Priedieu;
    // Effect
    public GameObject m_HitEffect;
    public GameObject m_Rundust;
    public GameObject m_RunStopdust;
    public GameObject m_Jumpdust;
    public GameObject m_Landingust;
    public GameObject m_PushBackdust;
    public GameObject m_DodgeDust;
    public GameObject m_HealEffect;
    public GameObject m_InputKey;
    // UI
    public Image m_HpBar;
    public Image m_Potion1;
    public Image m_Potion2;
    public Image m_Potion3;

    public Fade m_FadeImage;
    public Fade m_DeadthUI;
    public Fade m_AnyKeyUI;

    // Audio
    [SerializeField]
    public Coroutine m_RunSoundCoroutine;

    // Trail
    public Trail m_Trail;

    // MapName
    public string m_CurrentMapName;

    // Colider
    public BoxCollider2D m_AttackCollider;
    public BoxCollider2D m_HitCollider;
    public BoxCollider2D m_ParryColider;

    // Component
    private Rigidbody2D m_Rigid;
    private Animator m_Animator;
    private CapsuleCollider2D m_Colider;
    private PlatformEffector2D m_PlatformEffector;
    private SpriteRenderer m_SpriteRenderer;

    // Member
    private PlayerState m_CurrentState;
    public float m_Speed;
    public float m_JumpPower;
    public bool m_isLadder;
    public float m_Hp = 10;
    public float m_CurrentHp;
    public float m_KnockBackPower;
    public float m_AddX;
    public float m_AddY;
    public int m_MaxPotion = 3;
    public int m_Potion = 3;

    // State Switch
    //[HideInInspector]
    public bool m_isIdle;
    //[HideInInspector]
    public bool m_isRun;
   // [HideInInspector]
    public bool m_isJump;
   // [HideInInspector]
    public bool m_isCrouch;
    //[HideInInspector]
    public bool m_isDodge;
   // [HideInInspector]
    public bool m_isParry;
    public bool m_isParryCounter;
    public bool m_isDead;
    public bool m_isSpine;
    public bool m_isIntro;
    // [HideInInspector]
    public bool m_isComboAttack1;
   // [HideInInspector]
    public bool m_isComboAttack2;
    //[HideInInspector]
    public bool m_isComboAttack3;
   // [HideInInspector]
    public bool m_isJumpCombo1;
   // [HideInInspector]
    public bool m_isJumpCombo2;
    public bool m_isEvent;

    public float m_Damge;
    

    // Parry
    public bool m_CanParry = true;
    public bool m_isParrying;
    public float m_ParryingTime;
    public float m_ParryCooldown;


    // Dodge
    [HideInInspector]
    public bool m_CanDodge = true;
    [HideInInspector]
    public bool m_isDodgeing;
    [HideInInspector]
    public float m_DodgeingPower = 500f;
    //[HideInInspector]
    public float m_DodgeingTime = 1.2f;
    [HideInInspector]
    public float m_DodgeingCooldown = 1.5f;

    // ComboAttack
    public bool m_CanCombo;
    public bool m_CanJumpCombo;
    

    // Grounds Check
    public Transform m_CheckGroundsPos;
    public float m_CheckGroundsRad;
    public LayerMask m_isGroundsLayer;
    public bool m_isGrounds;

    void Start()
    {

        // GetComponent
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigid = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Colider = GetComponent<CapsuleCollider2D>();
        m_PlatformEffector = GetComponent<PlatformEffector2D>();
        
        if (m_GameManager == null)
        {
            m_GameManager = Object.FindAnyObjectByType<GameManager>();
        }

       

        // 초기 시작 상태
        m_CurrentHp = m_Hp;
        m_Potion = m_MaxPotion;
        m_HpBar.fillAmount = 1f;
        TransitonToState(PlayerStateType.Event);
        m_isIntro = true;
        m_Animator.SetTrigger("Intro");
    }

    private void Update()
    {
        if (m_isIntro && Input.GetKeyDown(KeyCode.E))
        {
            m_Animator.SetTrigger("WakeUp");
        }
        // GroundCheck
        GroundCheck();
        
        // State Update
        if (m_CurrentState != null)
        {
            m_CurrentState.StateUpdate();
        }

        Death_Check();
      
    }


    private void FixedUpdate()
    {
        // FixedState Update
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");

        if (m_CurrentState != null)
        {
            if (!m_isDodge && !m_isParrying && !m_isEvent)
            {
                InversionLeftOrRight(Horizontal);
            }
            m_CurrentState.StateFixedUpdate(Horizontal, Vertical);
        }

       if (!m_isLadder)
        {
            m_Rigid.gravityScale = 200;
        }
    }

    #region Anim_Event
    public void Parry_Collider_On()
    {
        m_HitCollider.enabled = false;
        m_ParryColider.enabled = true;
    }

    public void Parry_Collider_Off()
    {
        m_ParryColider.enabled = false;
        m_HitCollider.enabled = true;
    }



    public void Attack_Collider_On()
    {

        m_AttackCollider.enabled = true;
    }

    public void Attack_Collider_Off()
    {
        m_AttackCollider.enabled = false;
    }


    public void Start_Combo()
    {
        m_CanCombo = false;
    }

    public void Finish_Combo()
    {
        m_CanCombo = false;
        TransitonToState(PlayerStateType.Idle);
    }

    public void Start_JumpCombo()
    {
        m_CanJumpCombo = false;

    }

    public void SpriteRenderer_On()
    {
        m_SpriteRenderer.enabled = true;
    }

    public void EventState_Off()
    {
        TransitonToState(PlayerStateType.Idle);
    }
    public void Finish_JumpCombo()
    {
        m_CanJumpCombo = false;
        TransitonToState(PlayerStateType.Jump);
    }

    public void Finish_CrocuhAttack()
    {
        m_Animator.SetBool("Attack", false);
    }

    public void CameraShake_Horizontal()
    {
        StartCoroutine(m_Camera.Shake(0.2f, 5f, new Vector2(1, 0)));
    }

    public void CameraShake_Vertical()
    {
        StartCoroutine(m_Camera.Shake(0.2f, 10f, new Vector2(0, 1)));
    }

    public void Pray()
    {
        m_Priedieu.Pray();
    }

    public void Anim_KeyInputOn()
    {
        m_InputKey.gameObject.SetActive(true);
    }

    public void Anim_KeyInputOff()
    {
        m_InputKey.gameObject.SetActive(false);
    }

    public void Anim_SlashSound()
    {
        int Temp = Random.Range(1, 5);

        switch (Temp)
        {
            case 1:
                AudioManager.Instance.Play("Slash1");
                break;
            case 2:
                AudioManager.Instance.Play("Slash2");
                break;
            case 3:
                AudioManager.Instance.Play("Slash3");
                break;
            case 4:
                AudioManager.Instance.Play("Slash4");
                break;
        }
    }

    public void Anim_Dead_Sound()
    {
        AudioManager.Instance.Play("Death");
    }

    public void Anim_Respawn_Sound()
    {
        AudioManager.Instance.Play("Respawn");
    }


    #endregion

    #region Corutine
    private IEnumerator Dodge()
    {
        m_CanDodge = false;
        m_isDodgeing = true;
        m_HitCollider.enabled = false;
        m_Rigid.velocity = new Vector2(transform.localScale.x * m_DodgeingPower, m_Rigid.velocity.y);
        yield return new WaitForSeconds(m_DodgeingTime);
        m_HitCollider.enabled = true;
        m_isDodgeing = false;
        yield return new WaitForSeconds(m_DodgeingCooldown);
        m_CanDodge = true;
    }

    public void DodgeCorutine()
    {
        StartCoroutine(Dodge());
    }

    private IEnumerator Parry()
    {
        m_CanParry = false;
        m_isParrying = true;
        yield return new WaitForSeconds(m_ParryingTime);
        m_isParrying = false;
        yield return new WaitForSeconds(m_ParryCooldown);
        m_CanParry = true;
    }

    public void ParryCorutine()
    {
        StartCoroutine(Parry());
    }

    public void DeathCorutine()
    {
        StartCoroutine(Death());
    }
    private IEnumerator Death()
    {
       
        m_FadeImage.Fade_In();
        m_DeadthUI.Fade_In();

        AudioManager.Instance.Play("UI_Death");
        yield return new WaitForSeconds(m_DeadthUI.m_FadeTime * 2);

        m_AnyKeyUI.Fade_In();

        yield return new WaitForSeconds(m_AnyKeyUI.m_FadeTime);

        yield return new WaitUntil(() => Input.anyKeyDown);

        m_AnyKeyUI.Fade_Out();
        m_DeadthUI.Fade_Out();

        Player_Respawn();

        yield return new WaitForSeconds(m_FadeImage.m_FadeTime * 2);
        m_SpriteRenderer.enabled = false;
        m_FadeImage.Fade_Out();

        yield return new WaitForSeconds(m_FadeImage.m_FadeTime);
        m_Animator.SetTrigger("Respawn");
        
        
    }

    #endregion
    public void Death_Check()
    {
        if(m_CurrentHp <= 0 && !m_isSpine)
        {
            m_isDead = true;
            m_Animator.SetBool("Death", true);
            
            TransitonToState(PlayerStateType.Event);
        }
    }

    public void Player_Respawn()
    {
        if (m_Priedieu != null)
        {
            if (m_isDead)
            {
                m_isDead = false;
                m_Animator.SetBool("Death", false);
            }

            if (m_isSpine)
            {
                m_isSpine = false;
                m_Animator.SetBool("Spine", false);
            }
            m_Priedieu.Pray();
            m_Camera.SetBound(m_Priedieu.m_TargetBound);
            m_Camera.transform.position = new Vector3(m_Priedieu.Get_Position().x, m_Priedieu.Get_Position().y, m_Camera.transform.position.z);
            this.transform.position = m_Priedieu.Get_Position();
        }

        else
        {
            Debug.Log("Priedieu를 찾을수없습니다");
        }
        
    }
    public void Heal()
    {
        Create_Prefabs(m_HealEffect, 8f, -40f);

        if (m_Potion == 3)
        {
            m_Potion1.enabled = false;
        }

        else if (m_Potion == 2)
        {
            m_Potion2.enabled = false;
        }

        else if (m_Potion == 1)
        {
            m_Potion3.enabled = false;
        }

        m_Potion -= 1;
        AudioManager.Instance.Play("Glass");
        AudioManager.Instance.Play("Heal");
        float Healing  = (m_Hp * 0.2f);
        m_CurrentHp += Healing;
        Debug.Log(Healing);

        if (m_CurrentHp > m_Hp)
        {
            m_CurrentHp = m_Hp;
        }
        m_HpBar.fillAmount = (float)m_CurrentHp / (float)m_Hp;
    }

    

    public void Run_Sound()
    {
        if (m_RunSoundCoroutine == null)
        {
            m_RunSoundCoroutine = StartCoroutine(RunSound_Coroutine());
        }
    }

    public void Run_SoundStop()
    {
        if (m_RunSoundCoroutine != null)
        {
            StopCoroutine(RunSound_Coroutine());
            m_RunSoundCoroutine = null;
            Debug.Log("런 사운드 정지");
        }
    }

    IEnumerator RunSound_Coroutine()
    {
        while(m_isRun && !m_isIdle)
        {
            int Temp = Random.Range(1, 9);

            switch (Temp)
            {
                case 1:
                    AudioManager.Instance.Play("Run1");
                    break;
                case 2:
                    AudioManager.Instance.Play("Run2");
                    break;
                case 3:
                    AudioManager.Instance.Play("Run3");
                    break;
                case 4:
                    AudioManager.Instance.Play("Run4");
                    break;
                case 5:
                    AudioManager.Instance.Play("Run5");
                    break;
                case 6:
                    AudioManager.Instance.Play("Run6");
                    break;
                case 7:
                    AudioManager.Instance.Play("Run7");
                    break;
                case 8:
                    AudioManager.Instance.Play("Run8");
                    break;

            }
           

            yield return new WaitForSeconds(0.4f);

        }
    }

    
    public void Recover()
    {
        m_CurrentHp = m_Hp;
        m_HpBar.fillAmount = (float)m_CurrentHp / (float)m_Hp;
        m_Potion = m_MaxPotion;
        m_Potion1.enabled = true;
        m_Potion2.enabled = true;
        m_Potion3.enabled = true;

        Debug.Log("회복완료");
        
    }

    public void SetActive_Priedieu(Priedieu _Priedieu)
    {
        m_Priedieu = _Priedieu;
    }


    public void TransitonToState(PlayerStateType _StateType)
    {
        // 이전 상태 종료

        if (m_CurrentState != null)
        {
            m_CurrentState.StateExit();
        }

        // 새로운 상태 설정

        switch (_StateType)
        {
            case PlayerStateType.Idle:
                m_CurrentState = new Player_Idle(this);
                break;
            case PlayerStateType.Run:
                m_CurrentState = new Player_Run(this);
                break;
            case PlayerStateType.Jump:
                m_CurrentState = new Player_Jump(this);
                break;
            case PlayerStateType.Crouch:
                m_CurrentState = new Player_Crouch(this);
                break;
            case PlayerStateType.Dodge:
                m_CurrentState = new Player_Dodge(this);
                break;
            case PlayerStateType.Combo_Attack1:
                m_CurrentState = new Player_ComboAttack1(this);
                break;
            case PlayerStateType.Combo_Attack2:
                m_CurrentState = new Player_ComboAttack2(this);
                break;
            case PlayerStateType.Combo_Attack3:
                m_CurrentState = new Player_ComboAttack3(this);
                break;
            case PlayerStateType.Jump_Attack1:
                m_CurrentState = new Player_JumpCombo1(this);
                break;
            case PlayerStateType.Jump_Attack2:
                m_CurrentState = new Player_JumpCombo2(this);
                break;
            case PlayerStateType.Parry:
                m_CurrentState = new Player_Parry(this);
                break;
            case PlayerStateType.ParryCounter:
                m_CurrentState = new Player_ParryAttack(this);
                break;
            case PlayerStateType.Event:
                m_CurrentState = new Player_Event(this);
                break;
        }

        // 새로운 상태 진입
        m_CurrentState.StateEnter();
    }


    public Rigidbody2D Get_Rigid()
    {
        return m_Rigid;
    }

    public Animator Get_Anim()
    {
        return m_Animator;
    }

    public void InversionLeftOrRight(float _Horizontal)
    {
        if (_Horizontal < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }

        else if (_Horizontal > 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void GroundCheck()
    {
        // Grounds Check
        m_isGrounds = Physics2D.OverlapCircle(m_CheckGroundsPos.position, m_CheckGroundsRad, m_isGroundsLayer);

        if (m_isGrounds)
        {
            m_Animator.SetBool("IsGround", true);
        }
        else if (!m_isGrounds)
        {
            m_Animator.SetBool("IsGround", false);
        }
    }

    public void FreezeX()
    { 
        m_Rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void ReleaseFreezeX()
    {
        m_Rigid.constraints = RigidbodyConstraints2D.FreezeRotation; 
    }

    public void Create_Prefabs(GameObject _Prefabs, float _x, float _y)
    {
        _Prefabs.transform.localScale = this.transform.localScale;

        if(_Prefabs.transform.localScale == new Vector3(-1,1,1))
        {
            _x *= -1;
        }

        else
        {
            _x *= 1;
        }

        Instantiate(_Prefabs, new Vector3(this.transform.position.x + _x, this.transform.position.y + _y, this.transform.position.z), Quaternion.identity);
    }

    
    

    public void Ladder(float _Vertical)
    {


        if (m_isLadder)
        {
            m_Rigid.gravityScale = 0;
            m_Rigid.velocity = new Vector2(m_Rigid.velocity.x, _Vertical * m_Speed);

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                m_Animator.SetFloat("LadderSpeed", 1f);
            }

            else
            {
                m_Animator.SetFloat("LadderSpeed", 0f);
            }

            if (_Vertical > 0)
            {
                m_Animator.SetBool("LadderDown", false);
                m_Animator.SetBool("LadderUp", true);
            }

            else if (_Vertical < 0)
            {
                m_Animator.SetBool("LadderUp", false);
                m_Animator.SetBool("LadderDown", true);
            }

            
        }

        else if (!m_isLadder)
        {
            m_Animator.SetBool("LadderDown", false);
            m_Animator.SetBool("LadderUp", false);
           
        }
       

        
       

    }

    public void OnDamaged(Vector3 _TargetPos)
    {
       int Dir = this.transform.position.x - _TargetPos.x > 0 ? 1 : -1;
       m_Rigid.AddForce(new Vector2(Dir, 1) * m_KnockBackPower, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spine")
        {
            m_isSpine = true;
            TransitonToState(PlayerStateType.Event);
            m_Animator.SetTrigger("Spine");
            m_CurrentHp = 0;
            m_HpBar.fillAmount = (float)m_CurrentHp / (float)m_Hp;
            AudioManager.Instance.Play("Death_Spine");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                m_isLadder = true;
                Debug.Log("Ladder 트루");
            }

        }


        if (collision.CompareTag("Priedieu"))
        {
           if (Input.GetKeyDown(KeyCode.E))
            {
                m_Animator.SetTrigger("Pray");
                AudioManager.Instance.Play("Pray");
                TransitonToState(PlayerStateType.Event);

            }
        }
        
        


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            m_isLadder = false;
        }
    }
}
