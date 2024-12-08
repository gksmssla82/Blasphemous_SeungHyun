using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BlockColider : Enemy_Colider
{
    public GameObject m_BloodEffect1;
    public GameObject m_BloodEffect2;
    public GameObject m_BloodEffect3;
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
            

           

        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
           



        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
          



        }
    }
}
