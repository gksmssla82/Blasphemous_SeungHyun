using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ThrowColider : Enemy_Colider
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Enemy.m_Anim.SetTrigger("Rising");
            Enemy.m_isAttack = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Enemy.m_isAttack = false;
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Enemy.m_isAttack = true;
        }
    }

    
}
