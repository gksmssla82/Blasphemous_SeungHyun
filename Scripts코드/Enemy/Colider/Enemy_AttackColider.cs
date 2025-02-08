using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackColider : Enemy_Colider
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerParry"))
        {
            Debug.Log("������ ������ �÷��̾� �и��� ����");
            Enemy.EvnetAnim_AttackColider_Off();
            Enemy.m_Parry = true;
            Enemy.m_Anim.SetBool("Parry", true);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
       
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    
}
