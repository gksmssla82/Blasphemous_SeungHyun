using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public float m_TrailDelay; // Ʈ���� ���� Delay
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
                // ������ ���� �÷��̾��� ������,�÷��̾��� �����̼�
                GameObject CurrentTrail = Instantiate(m_Trail, transform.position, transform.rotation);
                // ��������Ʈ �ִϸ��̼� �޾ƿ���
                Sprite CurretnSprite = GetComponent<SpriteRenderer>().sprite;
                // �ø������� ���ý����� �÷��̾�� ����
                CurrentTrail.transform.localScale = this.transform.localScale;
                // ��������Ʈ �ִϸ��̼��� ������ Ʈ������ ����
                CurrentTrail.GetComponent<SpriteRenderer>().sprite = CurretnSprite;
                m_TrailDelayTime = m_TrailDelay;
                // 1�������� ����
                Destroy(CurrentTrail, 1f);
            }
        }
    }
}
