using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_CrossWhler : Enemy
{

    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        
        if (!m_Parry && !m_isAttacking && !m_Turn)
        {
            Tracing();
        }

        if (m_isAttack && m_isCanAttack && !m_Parry && !m_Turn)
        {
            NormalAttack_Corutine();
        }
    }

 



}
