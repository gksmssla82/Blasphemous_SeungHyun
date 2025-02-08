using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpCombo1 : PlayerState
{
    public Player_JumpCombo1(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        m_Player.m_Damge = 1;
        Debug.Log("점프콤보");
        m_Player.m_isJumpCombo1 = true;
        m_Player.Get_Anim().SetBool("JumpCombo1", true);
        m_Player.Anim_SlashSound();
    }

    public override void StateExit()
    {
        m_Player.m_isJumpCombo1 = false;
        m_Player.Get_Anim().SetBool("JumpCombo1", false);
        m_Player.m_AttackCollider.enabled = false;
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
        // Ground일 경우 Idle
        if (m_Player.m_isGrounds && !m_Player.m_isIdle && !m_Player.m_isRun)
        {
            m_Player.m_CanJumpCombo = false;
            m_Player.TransitonToState(PlayerStateType.Idle);
        }
       
        
    }

    public override void StateUpdate()
    {
        // Combo Attack
        if (!m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isJump && !m_Player.m_CanJumpCombo &&
            Input.GetKeyDown(KeyCode.K))
        {
            m_Player.ReleaseFreezeX();
            m_Player.m_CanJumpCombo = true;
            m_Player.TransitonToState(PlayerStateType.Jump_Attack2);
        }
    }

  
}
