using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TraceingAttack : Enemy_Colider
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Enemy.m_isTracing = true;
            Enemy.m_Anim.SetBool("Walk", true);
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Enemy.m_TagetDistance <= Enemy.m_AttackRange)
            {
                Enemy.m_isTracing = false;
                Enemy.m_Anim.SetBool("Walk", false);
                Enemy.m_isAttack = true;
            }

            else if (Enemy.m_TagetDistance >= Enemy.m_AttackRange)
            {
                Enemy.m_isAttack = false;
                Enemy.m_isTracing = true;
                Enemy.m_Anim.SetBool("Walk", true);
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Enemy.m_isAttack = false;
            Enemy.m_isTracing = false;
            Enemy.m_Anim.SetBool("Walk", false);
        }
    }
}
