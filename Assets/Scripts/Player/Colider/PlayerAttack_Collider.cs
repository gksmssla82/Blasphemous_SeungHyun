using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack_Collider : Player_Colider
{
    public GameObject m_AttackEffect1;
    public GameObject m_AttackEffect2;
    public GameObject m_AttackEffect3;
    public GameObject m_Block;


    private void RandomEffect()
    {
        int RandomIndex = Random.Range(1, 4);

        switch (RandomIndex)
        {
            case 1:
                Instantiate(m_AttackEffect1, transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(m_AttackEffect2, transform.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(m_AttackEffect3, transform.position, Quaternion.identity);
                break;


        }
    }

    private void RandomSound()
    {
        int Temp = Random.Range(1, 5);

        switch (Temp)
        {
            case 1:
                AudioManager.Instance.Play("Enemy_Hit1");
                break;
            case 2:
                AudioManager.Instance.Play("Enemy_Hit2");
                break;
            case 3:
                AudioManager.Instance.Play("Enemy_Hit3");
                break;
            case 4:
                AudioManager.Instance.Play("Enemy_Hit4");
                break;
        }
    }

    private void ShiledSound()
    {
        int Temp = Random.Range(1, 4);

        switch (Temp)
        {
            case 1:
                AudioManager.Instance.Play("Shield_Hit1");
                break;
            case 2:
                AudioManager.Instance.Play("Shield_Hit2");
                break;
            case 3:
                AudioManager.Instance.Play("Shield_Hit3");
                break;
        }
    }

 
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHit"))
        {
            Debug.Log("몬스터 공격!");
            if (!m_Player.m_isParryCounter)
            {
                RandomSound();
            }
            else
            {
                AudioManager.Instance.Play("Enemy_HeavyHit");
            }
            RandomEffect();
           // m_Player.m_GameManager.SlowTime(0.3f, 0.1f);

        }
        else if (collision.CompareTag("EnemyBlock"))
        {
            Instantiate(m_Block, transform.position, Quaternion.identity);
            ShiledSound();
        }


        else if (collision.CompareTag("BossHit"))
        {
            m_Boss.TakeDamage(m_Player.m_Damge);
            RandomSound();
           
        
            RandomEffect();
            // m_Player.m_GameManager.SlowTime(0.3f, 0.1f);

        }

       
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
       
    }
}
