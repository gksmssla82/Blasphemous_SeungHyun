using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ParryColider : Player_Colider
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("EnemyAttack"))
       {
            m_Player.TransitonToState(PlayerStateType.ParryCounter);
            AudioManager.Instance.Play("Parry_Attack");
       }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
       
    }

    
}
