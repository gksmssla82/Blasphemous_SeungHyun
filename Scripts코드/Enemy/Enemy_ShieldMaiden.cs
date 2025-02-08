using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ShieldMaiden : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        
        Next_Move();
    }


    protected override void Update()
    {
        base.Update();


        BlockCheck();

        if (!m_Parry && !m_isAttacking)
        {
            Tracing();
        }

        if (m_isAttack && m_isCanAttack && !m_Parry)
        {
            AudioManager.Instance.Play("ShieldMaiden_Attack");
            NormalAttack_Corutine();
        }
    }

    protected override void FixedUpdate()
    {
        Normal_Move();
    }


    private void BlockCheck()
    {
        if (!m_Parry && !m_Dead)
        {
            if (m_Taget.transform.localScale == this.transform.localScale)
            {
                m_HitBoxColider.enabled = true;
                m_BlockColider.enabled = false;
            }

            else
            {
                m_HitBoxColider.enabled = false;
                m_BlockColider.enabled = true;
            }
        }

        else if (m_Parry && !m_Dead)
        {
            m_BlockColider.enabled = false;
            m_HitBoxColider.enabled = true;
        }
    }
}
