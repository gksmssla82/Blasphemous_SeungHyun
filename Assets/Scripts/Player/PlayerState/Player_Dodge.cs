using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dodge : PlayerState
{
    public Player_Dodge(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        AudioManager.Instance.Play("Dash");
        m_Player.Create_Prefabs(m_Player.m_DodgeDust, 0f, -22f);
        m_Player.m_Trail.m_CreateTrail = true;
        m_Player.Get_Rigid().constraints = RigidbodyConstraints2D.FreezeRotation;
        m_Player.DodgeCorutine();
        m_Player.Get_Anim().SetBool("Dodge", true);
    }

    public override void StateExit()
    {
        m_Player.m_Trail.m_CreateTrail = false;
        m_Player.m_isDodge = false;
        m_Player.Get_Anim().SetBool("Dodge", false);
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
        
    }

    public override void StateUpdate()
    {


        if (!m_Player.m_isDodgeing)
        {
            m_Player.TransitonToState(PlayerStateType.Idle);
        }

        // 점프 True
        if (m_Player.m_isGrounds && Input.GetKeyDown(KeyCode.Space))
        {
            m_Player.Get_Rigid().velocity = Vector2.up * m_Player.m_JumpPower;
        }

        // Ground가 아니고 Jump일경우 JumpState로
        if (!m_Player.m_isGrounds && !m_Player.m_isRun)
        {
           // m_Player.ReleaseFreezeX();
            m_Player.TransitonToState(PlayerStateType.Jump);
        }

        // Crouch
        if (m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isJump && Input.GetKeyDown(KeyCode.S))
        {
            m_Player.TransitonToState(PlayerStateType.Crouch);
        }

        // Combo Attack
        if (m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isJump && !m_Player.m_CanCombo &&
            Input.GetKeyDown(KeyCode.K))
        {
           // m_Player.ReleaseFreezeX();
            m_Player.m_CanCombo = true;
            m_Player.TransitonToState(PlayerStateType.Combo_Attack1);
        }
    }

   
}
