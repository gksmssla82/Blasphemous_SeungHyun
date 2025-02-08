using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpCombo2 : PlayerState
{
    public Player_JumpCombo2(PlayerController _Player) : base(_Player)
    {
    }
    public override void StateEnter()
    {
        m_Player.m_Damge = 3;
        Debug.Log("점프콤보1");
        m_Player.m_isJumpCombo2 = true;
        m_Player.Get_Anim().SetBool("JumpCombo2", true);
        m_Player.Anim_SlashSound();
    }

    public override void StateExit()
    {
        m_Player.m_isJumpCombo2 = false;
        m_Player.m_AttackCollider.enabled = false;
        m_Player.Get_Anim().SetBool("JumpCombo2", false);
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
       
    }
}