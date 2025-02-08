using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntroTrigger : MonoBehaviour
{
    private Boss_Pietat m_Boss;
    private PlayerController m_Player;
    private CameraManager m_Camera;
    void Start()
    {
        if (m_Player == null)
        {
            m_Player = Object.FindAnyObjectByType<PlayerController>();
        }

        if (m_Boss == null)
        {
            m_Boss = Object.FindAnyObjectByType<Boss_Pietat>();
        }

        if (m_Camera == null)
        {
            m_Camera = Object.FindAnyObjectByType<CameraManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_Camera.m_PlayerChase = false;
            m_Camera.TargetMove_Enemy();
            m_Player.TransitonToState(PlayerStateType.Event);
            m_Boss.m_Anim.SetTrigger("IntroTrriger");
            this.gameObject.SetActive(false);
        }
    }
}
