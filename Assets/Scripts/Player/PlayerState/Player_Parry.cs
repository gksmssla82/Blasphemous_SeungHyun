using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Parry : PlayerState
{
    public Player_Parry(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        AudioManager.Instance.Play("Parry");
        m_Player.Get_Rigid().velocity = Vector2.zero;
        m_Player.Get_Anim().SetBool("Parry", true);
        m_Player.ParryCorutine();
    }

    public override void StateExit()
    {
        m_Player.Get_Anim().SetBool("Parry", false);
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
        
    }

    public override void StateUpdate()
    {
        if (!m_Player.m_isParrying)
        {
            m_Player.TransitonToState(PlayerStateType.Idle);
        }

        
    }
}
