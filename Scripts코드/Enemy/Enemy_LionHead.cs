using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LionHead : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        Next_Move();
    }


    protected override void Update()
    {
        base.Update();

        if (!m_Parry && !m_isAttacking)
        {
            Tracing();
        }
        if (m_isAttack && m_isCanAttack)
        {
            AudioManager.Instance.Play("Leon_WaitAttack");
            NormalAttack_Corutine();
        }
    }

    protected override void FixedUpdate()
    {
        Normal_Move();
    }
}
