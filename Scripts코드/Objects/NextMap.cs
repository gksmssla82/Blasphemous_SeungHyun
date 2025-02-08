using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextMap : MonoBehaviour
{
    public string m_NextMapName; // �̵��� ���� �̸�

    public Transform m_Target; // �̵���ų Ÿ��
    public BoxCollider2D m_TargetBound; // ī�޶� �ٿ�� �ݶ��̴�

    private PlayerController m_Player; // �÷��̾�
    private CameraManager m_Camera; // ī�޶�

    public Image m_Panel; // ���̵� �̹���
    private float m_Time = 0f;  // ���̵� ���İ� �ð�
    private float m_FadeTime = 0.5f; // ���̵� Ÿ��

    [System.Obsolete]
    void Start()
    {
        m_Player = FindObjectOfType<PlayerController>();
        m_Camera = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name == "Player")
        {
            m_Player.m_CurrentMapName = m_NextMapName;
            StartCoroutine(FadeInOut());
        }

        if (m_Player.m_CurrentMapName == "Stage3")
        {
            m_Camera.m_Camera.orthographicSize = 170;
            m_Camera.m_AddCameraPosY = -100;
        }

        else if (m_Player.m_CurrentMapName != "Stage3")
        {
            m_Camera.m_Camera.orthographicSize = 190;
            m_Camera.m_AddCameraPosY = 50;
        }
    }


    IEnumerator FadeInOut()
    {
        m_Panel.gameObject.SetActive(true);
        m_Time = 0f;
        Color Alpha = m_Panel.color;

        while (Alpha.a < 1f)
        {
            m_Time += Time.deltaTime / m_FadeTime;
            Alpha.a = Mathf.Lerp(0, 1, m_Time);
            m_Panel.color = Alpha;
           
            yield return null;
        }

        m_Time = 0f;



        yield return new WaitForSeconds(0.8f);

        m_Camera.SetBound(m_TargetBound);
        m_Camera.transform.position = new Vector3(m_Target.transform.position.x, m_Target.transform.position.y, m_Camera.transform.position.z);
        m_Player.transform.position = new Vector3(m_Target.transform.position.x, m_Target.transform.position.y, 0);


        yield return new WaitForSeconds(0.8f);
       
        while (Alpha.a > 0f)
        {
            m_Time += Time.deltaTime / m_FadeTime;
            Alpha.a = Mathf.Lerp(1, 0, m_Time);
            m_Panel.color = Alpha;
            yield return null;
        }

        m_Panel.gameObject.SetActive(false);
        

       
        yield return null;
    }
}
