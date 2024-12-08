using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Idle : PlayerState
{
    public Player_Idle(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        m_Player.Get_Rigid().velocity = Vector2.zero;
        m_Player.m_isIdle = true;
        m_Player.m_Damge = 1;
       

        
    }

    public override void StateExit()
    {
        m_Player.m_isIdle = false;
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
        ////언덕 PositionX축 고정
        //if (_Horizontal == 0 && m_Player.m_isIdle)
        //{
        //    m_Player.FreezeX();
        //}

        //State Run
        if (_Horizontal != 0 && m_Player.m_isIdle)
        {
            // X축 고정 해제
            //m_Player.ReleaseFreezeX();
            m_Player.TransitonToState(PlayerStateType.Run);
        }

        m_Player.Ladder(_Vertical);

       

       
    }

    public override void StateUpdate()
    {

       
        
        // 점프 True
        if (m_Player.m_isGrounds && !m_Player.m_isLadder && Input.GetKeyDown(KeyCode.Space))
        {
            m_Player.Create_Prefabs(m_Player.m_Jumpdust, 11f, -25f);
            m_Player.Get_Rigid().velocity = Vector2.up * m_Player.m_JumpPower;
        }

        // Ground가 아니고 Jump일경우 JumpState로
        if (!m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isLadder)
        {
            //m_Player.ReleaseFreezeX();
            m_Player.TransitonToState(PlayerStateType.Jump);
        }

        // Combo Attack
        if (m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isJump && !m_Player.m_CanCombo &&
             !m_Player.m_isLadder && Input.GetKeyDown(KeyCode.K))
        {
            //m_Player.ReleaseFreezeX();
            m_Player.m_CanCombo = true;
            m_Player.TransitonToState(PlayerStateType.Combo_Attack1);
        }

        // Crouch
        if (m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isJump && !m_Player.m_CanCombo &&
            !m_Player.m_isLadder && Input.GetKeyDown(KeyCode.S))
        {
            m_Player.TransitonToState(PlayerStateType.Crouch);
        }

        // Dodge State
        if (m_Player.m_isGrounds && m_Player.m_CanDodge && !m_Player.m_isLadder && Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_Player.m_isDodge = true;
            m_Player.TransitonToState(PlayerStateType.Dodge);
        }

       
        // Parry State
        if (m_Player.m_isGrounds && m_Player.m_CanParry && !m_Player.m_isLadder && Input.GetKeyDown(KeyCode.J))
        {
            m_Player.m_isParry = true;
            m_Player.TransitonToState(PlayerStateType.Parry);
        }

        // Potion

        if (m_Player.m_isGrounds && !m_Player.m_isLadder && m_Player.m_Potion != 0 && Input.GetKeyDown(KeyCode.F))
        {
            m_Player.Get_Anim().SetTrigger("Heal");
            m_Player.TransitonToState(PlayerStateType.Event);
        }
    }
}
