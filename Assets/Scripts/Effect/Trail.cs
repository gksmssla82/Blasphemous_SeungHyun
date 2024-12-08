using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public float m_TrailDelay; // 트레일 생성 Delay
    private float m_TrailDelayTime; 
    public GameObject m_Trail;
    public bool m_CreateTrail = false;
    void Start()
    {
        m_TrailDelayTime = m_TrailDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CreateTrail)
        {
            if (m_TrailDelayTime > 0)
            {
                m_TrailDelayTime -= Time.deltaTime;
            }

            else
            {
                // 프리펩 생성 플레이어의 포지션,플레이어의 로테이션
                GameObject CurrentTrail = Instantiate(m_Trail, transform.position, transform.rotation);
                // 스프라이트 애니메이션 받아오기
                Sprite CurretnSprite = GetComponent<SpriteRenderer>().sprite;
                // 플립을위해 로컬스케일 플레이어로 고정
                CurrentTrail.transform.localScale = this.transform.localScale;
                // 스프라이트 애니메이션을 프리펩 트레일의 저장
                CurrentTrail.GetComponent<SpriteRenderer>().sprite = CurretnSprite;
                m_TrailDelayTime = m_TrailDelay;
                // 1초지나면 삭제
                Destroy(CurrentTrail, 1f);
            }
        }
    }
}
