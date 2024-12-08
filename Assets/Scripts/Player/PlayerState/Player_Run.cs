using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Run : PlayerState
{

    private float m_EffectCreateTime = 0.3f;
    private float m_EffectDelayTime = 0.3f;
    public Player_Run(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        m_Player.m_Damge = 1;
      
        m_Player.m_isRun = true;
        m_Player.Get_Anim().SetBool("Run", true);

        if (m_Player.m_RunSoundCoroutine != null)
        {
            m_Player.Run_SoundStop();
        }

        m_Player.Run_Sound();
    }

    public override void StateExit()
    {
        
        m_Player.m_isRun = false;
        m_Player.Get_Anim().SetBool("Run", false);

        if (m_Player.m_RunSoundCoroutine != null)
        { 
            m_Player.Run_SoundStop();
        }

        AudioManager.Instance.Play("RunStop");
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
        // 이동
        if (m_Player.m_isGrounds)
        {
            m_Player.Get_Rigid().velocity = new Vector2(_Horizontal * m_Player.m_Speed, m_Player.Get_Rigid().velocity.y);
        }
        // Translate Idle
        if (m_Player.m_isGrounds && _Horizontal == 0)
        {
            m_Player.Create_Prefabs(m_Player.m_RunStopdust, 50f, -25f);
            m_Player.TransitonToState(PlayerStateType.Idle);
        }
    }

    public override void StateUpdate()
    {
        m_EffectDelayTime -= Time.deltaTime;


        // Prefabs
        Run_Prefabs();

        // 점프 True
        if (m_Player.m_isGrounds && !m_Player.m_isIdle && !m_Player.m_isJump && Input.GetKeyDown(KeyCode.Space))
        {
            m_Player.Create_Prefabs(m_Player.m_Jumpdust, 0f, -25f);
            m_Player.Get_Rigid().velocity = Vector2.up * m_Player.m_JumpPower;
        }

        // Ground가 아니고 Jump일경우 JumpState로
        if (!m_Player.m_isGrounds && !m_Player.m_isIdle && !m_Player.m_isJump)
        {
            m_Player.ReleaseFreezeX();
            m_Player.TransitonToState(PlayerStateType.Jump);
        }

        // Combo Attack
        if (m_Player.m_isGrounds && !m_Player.m_isIdle && !m_Player.m_isJump && !m_Player.m_CanCombo &&
            Input.GetKeyDown(KeyCode.K))
        {
            m_Player.ReleaseFreezeX();
            m_Player.m_CanCombo = true;
            m_Player.TransitonToState(PlayerStateType.Combo_Attack1);
        }

        // Dodge State
        if (m_Player.m_isGrounds && m_Player.m_CanDodge && Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_Player.m_isDodge = true;
            m_Player.TransitonToState(PlayerStateType.Dodge);
        }

        
        // Crouch
        if (m_Player.m_isGrounds && !m_Player.m_isIdle && !m_Player.m_isJump && Input.GetKeyDown(KeyCode.S))
        {
            m_Player.TransitonToState(PlayerStateType.Crouch);
        }

        // Parry State
        if (m_Player.m_isGrounds && m_Player.m_CanParry && Input.GetKeyDown(KeyCode.J))
        {
            m_Player.m_isParry = true;
            m_Player.TransitonToState(PlayerStateType.Parry);
        }
    }

    private void Run_Prefabs()
    {
        if (m_EffectDelayTime <= 0)
        {
            m_Player.Create_Prefabs(m_Player.m_Rundust, -20f, -30f);
            m_EffectDelayTime = m_EffectCreateTime;
        }
    }

}
