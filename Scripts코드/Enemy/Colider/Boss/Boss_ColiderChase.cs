using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_ColiderChase : Enemy_Colider
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Enemy.m_isTracing = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Enemy.m_isTracing = false;
            Enemy.m_isAttack = false;
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Enemy.Player_BehindCheck())
            {
                Enemy.m_Turn = true;
                Enemy.m_isAttack = false;
                Enemy.m_isTracing = false;
            }

            if (!Enemy.Player_BehindCheck() && Enemy.m_TagetDistance <= Enemy.m_AttackRange)
            {
                Enemy.m_isTracing = false;
                Enemy.m_isAttack = true;
            }

            else if (!Enemy.Player_BehindCheck() && Enemy.m_TagetDistance >= Enemy.m_AttackRange)
            {
                Enemy.m_isAttack = false;
                Enemy.m_isTracing = true;
            }
        }
    }

  
}
