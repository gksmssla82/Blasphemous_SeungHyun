using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_YellowCartWheel : Enemy
{
    
    protected override void Awake()
    {
        base.Awake();
       
    }

    
    protected override void Update()
    {
        base.Update();

        

        if (!m_Parry)
        {
            Tracing();
        }

        if (m_isAttack && m_isCanAttack && !m_Parry)
        {
            NormalAttack_Corutine();
        }
    }



    

   
}
