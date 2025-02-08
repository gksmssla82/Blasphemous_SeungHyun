using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stoner : Enemy
{
    public float m_Time;
    protected override void Awake()
    {
        base.Awake();
        
        
        
    }

    protected override void Start()
    {
        base.Start();
        m_Anim.SetTrigger("Spawn");
    }

    protected override void Update()
    {
        base.Update();

        if (!m_Dead)
        {
            LookAtTaget();
        }
       

        if (m_isAttack && m_isCanAttack)
        {
            AudioManager.Instance.Play("Stoner_PassRock");
            NormalAttack_Corutine();
        }
        
    }

}
