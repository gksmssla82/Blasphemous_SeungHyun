using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Event : PlayerState
{
    public Player_Event(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        m_Player.m_Damge = 5;
        m_Player.m_isEvent = true;
        m_Player.m_HitCollider.enabled = false;
    }

    public override void StateExit()
    {
        m_Player.m_CanCombo = false;
        m_Player.m_isEvent = false;
        m_Player.m_HitCollider.enabled = true;
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
      
    }

    public override void StateUpdate()
    {
        
    }

   
}
