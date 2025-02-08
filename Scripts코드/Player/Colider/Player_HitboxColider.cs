using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HitboxColider : Player_Colider
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack") || collision.CompareTag("Stone"))
        {
            Debug.Log("플레이어가 데미지를 입음");
            if (m_Player.m_CurrentHp > 0)
            {
                AudioManager.Instance.Play("Damage");
                m_Player.m_CurrentHp -= 1;
                m_Player.m_HpBar.fillAmount = (float)m_Player.m_CurrentHp / m_Player.m_Hp;
                m_Player.Create_Prefabs(m_Player.m_HitEffect, m_Player.m_AddX, m_Player.m_AddY);
                m_Player.Create_Prefabs(m_Player.m_PushBackdust, 20f, -25f);
                m_Player.Get_Anim().SetTrigger("PushBack");
                m_Player.TransitonToState(PlayerStateType.Event);
                m_Player.m_GameManager.SlowTime(0.5f, 0.2f);
             
                m_Player.OnDamaged(collision.transform.position);
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
     
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
       
    }

   
}
