using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thron_Tower : Enemy
{
    
    protected override void Awake()
    {
        base.Awake();
    }
  
  


    public void EventAnim_TowerColiderOn()
    {
        AudioManager.Instance.Play("Grow");
        m_BoxCollider.enabled = true;
    }

    public void EventAnim_TowerColiderOff()
    {
        m_BoxCollider.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHit"))
        {
            Debug.Log("Tower¿¡ ¸ÂÀ½");
        }
    }

}
