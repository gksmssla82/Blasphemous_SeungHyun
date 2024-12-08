using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public int PlayMusicTrack;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BgmManager.Instance.Play(PlayMusicTrack);
            this.gameObject.SetActive(false);
        }
    }
}
