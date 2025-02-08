using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DotTraceAttackColider : Enemy_Colider
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Enemy.Player_BehindCheck())
            {
                Enemy.m_Turn = true;
                Enemy.m_Anim.SetBool("Turn", true);
            }

            else
            {
                Enemy.m_isTracing = true;
                Enemy.m_Anim.SetBool("Walk", true); 
            }
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Enemy.Player_BehindCheck())
            {
                Enemy.m_Turn = true;
                Enemy.m_Anim.SetBool("Turn", true);
            }

            else
            {
                if (Enemy.m_TagetDistance <= Enemy.m_AttackRange && !Enemy.m_Turn)
                {
                    Enemy.m_isTracing = false;
                    Enemy.m_Anim.SetBool("Walk", false);
                    Enemy.m_isAttack = true;
                }

                else if (Enemy.m_TagetDistance >= Enemy.m_AttackRange && !Enemy.m_Turn)
                {
                    Enemy.m_isAttack = false;
                    Enemy.m_isTracing = true;
                    Enemy.m_Anim.SetBool("Walk", true);
                }
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
