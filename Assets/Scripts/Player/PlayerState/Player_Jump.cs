using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jump : PlayerState
{
    public Player_Jump(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        
        m_Player.m_isJump = true;
        m_Player.Get_Anim().SetBool("Jump", true);
        AudioManager.Instance.Play("Jump");
        
    }

    public override void StateExit()
    {
        m_Player.m_isJump = false;
        m_Player.Get_Anim().SetBool("Jump", false);
        
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
        // 점프State에도 좌우 움직일수있음
        m_Player.Get_Rigid().velocity = new Vector2(_Horizontal * m_Player.m_Speed, m_Player.Get_Rigid().velocity.y);
    }

    public override void StateUpdate()
    {
        // Ground일 경우 Idle
      if (m_Player.m_isGrounds && !m_Player.m_isIdle && !m_Player.m_isRun)
        {
            AudioManager.Instance.Play("Randing");
            m_Player.Create_Prefabs(m_Player.m_Landingust, 12f, -25f);
            m_Player.TransitonToState(PlayerStateType.Idle);
        }

        // Combo Attack
        if (!m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isIdle && !m_Player.m_CanJumpCombo &&
            Input.GetKeyDown(KeyCode.K))
        {
            m_Player.ReleaseFreezeX();
            m_Player.m_CanJumpCombo = true;
            m_Player.TransitonToState(PlayerStateType.Jump_Attack1);
        }

    }

 
}
