using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priedieu : MonoBehaviour
{
    public GameObject m_Keycode;
    public GameManager m_GameManager;
    public BoxCollider2D m_TargetBound; // 카메라 바운드 콜라이더
    public PlayerController m_Player;
    private Animator m_Animator;

    private bool m_isActive = false;


    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_Player.SetActive_Priedieu(this);
            m_Keycode.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_Keycode.SetActive(false);
        }
    }
    public void Pray()
    {
        m_Animator.SetBool("On", true);
        m_GameManager.RespawnMonsters();
        m_Player.Recover();
        
        m_isActive = true;
    }

    public Vector3 Get_Position()
    {
        return transform.position;
    }

}
