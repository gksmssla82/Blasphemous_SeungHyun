using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ComboAttack1 : PlayerState
{
    public Player_ComboAttack1(PlayerController _Player) : base(_Player)
    {
        
    }

    public override void StateEnter()
    {
        m_Player.m_Damge = 1;
        m_Player.m_Trail.m_CreateTrail = true;
        m_Player.Get_Rigid().velocity = Vector2.zero;
        m_Player.m_isComboAttack1 = true;
        m_Player.Get_Anim().SetBool("Combo1", true);
        AudioManager.Instance.Play("Slash1");
    }

    public override void StateExit()
    {
        m_Player.m_Trail.m_CreateTrail = false;
        m_Player.m_isComboAttack1 = false;
        m_Player.m_AttackCollider.enabled = false;
        m_Player.Get_Anim().SetBool("Combo1", false);
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
       
    }

    public override void StateUpdate()
    {
        // Combo Attack
        if (m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isJump && !m_Player.m_CanCombo &&
            Input.GetKeyDown(KeyCode.K))
        {
            m_Player.ReleaseFreezeX();
            m_Player.m_CanCombo = true;
            m_Player.TransitonToState(PlayerStateType.Combo_Attack2);
        }

        // Dodge State
        if (m_Player.m_isGrounds && m_Player.m_CanDodge && Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_Player.m_isDodge = true;
            m_Player.m_CanCombo = false;
            m_Player.TransitonToState(PlayerStateType.Dodge);
        }


    }
}
