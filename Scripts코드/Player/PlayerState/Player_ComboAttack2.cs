using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ComboAttack2 : PlayerState

{
    public Player_ComboAttack2(PlayerController _Player) : base(_Player)
    {
    }

    // Start is called before the first frame update
    public override void StateEnter()
    {
        m_Player.m_Damge = 3;
        m_Player.m_Trail.m_CreateTrail = true;
        m_Player.m_isComboAttack2 = true;
        m_Player.Get_Anim().SetBool("Combo2", true);
        AudioManager.Instance.Play("Slash2");
    }

    public override void StateExit()
    {
        m_Player.m_Trail.m_CreateTrail = false;
        m_Player.m_isComboAttack2 = false;
        m_Player.m_AttackCollider.enabled = false;
        m_Player.Get_Anim().SetBool("Combo2", false);
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
       
    }

    public override void StateUpdate()
    {
        //Combo Attack
        if (m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isJump && !m_Player.m_CanCombo &&
            Input.GetKeyDown(KeyCode.K))
        {
            m_Player.ReleaseFreezeX();
            m_Player.m_CanCombo = true;
            m_Player.TransitonToState(PlayerStateType.Combo_Attack3);
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
