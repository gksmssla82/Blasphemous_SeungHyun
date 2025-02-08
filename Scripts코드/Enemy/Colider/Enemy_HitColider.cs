using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HitColider : Enemy_Colider
{
    public GameObject m_BloodEffect1;
    public GameObject m_BloodEffect2;
    public GameObject m_BloodEffect3;
    public GameObject m_Enemy;

    public float m_AddX;
    public float m_AddY;


    public void Create_Prefabs(GameObject _Prefabs, float _x, float _y)
    {
        _Prefabs.transform.localScale = Enemy.transform.localScale;

        if (_Prefabs.transform.localScale == new Vector3(-1, 1, 1))
        {
            _Prefabs.transform.localScale = new Vector3(1, 1, 1);
            _x *= -1;
        }

        else
        {
            _Prefabs.transform.localScale = new Vector3(-1, 1, 1);
            _x *= 1;
        }

        Instantiate(_Prefabs, new Vector3(Enemy.transform.position.x + _x, Enemy.transform.position.y + _y, Enemy.transform.position.z), Quaternion.identity);
    }

    private void RandomEffect()
    {
        int RandomIndex = Random.Range(1, 4);

        switch (RandomIndex)
        {
            case 1:
                Create_Prefabs(m_BloodEffect1, m_AddX, m_AddY);
                break;
            case 2:
                Create_Prefabs(m_BloodEffect2, m_AddX, m_AddY);
                break;
            case 3:
                Create_Prefabs(m_BloodEffect3, m_AddX, m_AddY);
                break;


        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            if (Enemy.m_Hp > 0)
            {
                RandomEffect();
                Enemy.m_Hp -= 1;
                Enemy_Hit();
            }

        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        
    }


    protected override void OnTriggerStay2D(Collider2D collision)
    {
       
    }

    public void Enemy_Hit()
    {
        
        if (m_Enemy != null && m_Enemy.GetComponentInParent<Enemy_Stoner>() != null)
        {
            Enemy.m_Anim.SetTrigger("Hurt");
            Enemy.m_isCanAttack = false;

            int Temp = Random.Range(1, 3);

            switch (Temp)
            {
                case 1:
                    AudioManager.Instance.Play("Stoner_Hit1");
                    break;
                case 2:
                    AudioManager.Instance.Play("Stoner_Hit2");
                    break;
            }
        }

       

        
    }
}
