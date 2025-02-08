using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle,
    Run,
    Jump,
    Fall,
    Crouch,
    Dodge,
    Combo_Attack1,
    Combo_Attack2,
    Combo_Attack3,
    Dodge_Attack,
    Crouch_Attack,
    UpWard_Attack,
    Jump_Attack1,
    Jump_Attack2,
    Jump_UpWard_Attack,
    Parry,
    ParryCounter,
    Event,
}


public abstract class PlayerState 
{
    protected PlayerController m_Player;

    public PlayerState(PlayerController _Player)
    {
        m_Player = _Player;
    }

    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate(float _Horizontal, float _Vertical);
    public abstract void StateExit();


   
   
}
