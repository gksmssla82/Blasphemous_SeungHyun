using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ParryAttack : PlayerState
{
    public Player_ParryAttack(PlayerController _Player) : base(_Player)
    {
    }

    public override void StateEnter()
    {
        m_Player.m_HitCollider.enabled = false;
        m_Player.m_ParryColider.enabled = false;
        m_Player.m_isParryCounter = true;
        m_Player.Get_Anim().SetBool("ParryCounter", true);
    }

    public override void StateExit()
    {
        m_Player.Get_Anim().SetBool("ParryCounter", false);
        m_Player.m_isParryCounter = false;
        m_Player.m_HitCollider.enabled = true;
    }

    public override void StateFixedUpdate(float _Horizontal, float _Vertical)
    {
        
    }

    public override void StateUpdate()
    {
        
    }

 
}
