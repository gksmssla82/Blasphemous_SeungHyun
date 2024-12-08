using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ComboAttack3 : PlayerState
{
    public Player_ComboAttack3(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        m_Player.m_Damge = 5;
        m_Player.m_Trail.m_CreateTrail = true;
        m_Player.m_isComboAttack3 = true;
        m_Player.Get_Anim().SetBool("Combo3", true);
        AudioManager.Instance.Play("Slash_Final");
    }

    public override void StateExit()
    {
        m_Player.m_Trail.m_CreateTrail = false;
        m_Player.m_isComboAttack3 = false;
        m_Player.m_AttackCollider.enabled = false;
        m_Player.Get_Anim().SetBool("Combo3", false);
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
        
    }

    public override void StateUpdate()
    {
        // Dodge State
        if (m_Player.m_isGrounds && m_Player.m_CanDodge && Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_Player.m_isDodge = true;
            m_Player.m_CanCombo = false;
            m_Player.TransitonToState(PlayerStateType.Dodge);
        }
    }
}
