using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_Colider : MonoBehaviour
{
    protected PlayerController m_Player;
    protected Boss_Pietat m_Boss;
    protected virtual void Start()
    {
        m_Player = GetComponentInParent<PlayerController>();

        if (m_Boss == null)
        {
            m_Boss = Object.FindAnyObjectByType<Boss_Pietat>();
        }
    }

    protected abstract void OnTriggerEnter2D(Collider2D collision);
    protected abstract void OnTriggerStay2D(Collider2D collision);
    protected abstract void OnTriggerExit2D(Collider2D collision);
}
