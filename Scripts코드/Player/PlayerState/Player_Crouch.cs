using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Crouch : PlayerState
{
    public Player_Crouch(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        m_Player.m_Damge = 1;
        m_Player.m_isCrouch = true;
        m_Player.Get_Anim().SetBool("Crouch", true);
        m_Player.Get_Rigid().velocity = Vector2.zero;
    }

    public override void StateExit()
    {
        m_Player.m_isCrouch = false;
        m_Player.Get_Anim().SetBool("Crouch", false);
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
        
    }

    public override void StateUpdate()
    {
        // Idle
        if (m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isJump && Input.GetKeyUp(KeyCode.S))
        {
            m_Player.TransitonToState(PlayerStateType.Idle);
        }



        // Crocuh Attack
        if (m_Player.m_isGrounds && !m_Player.m_isRun && !m_Player.m_isJump && Input.GetKeyDown(KeyCode.K))
        {
            m_Player.Anim_SlashSound();
            m_Player.Get_Anim().SetBool("Attack", true);
        }

        

        


    }
}
