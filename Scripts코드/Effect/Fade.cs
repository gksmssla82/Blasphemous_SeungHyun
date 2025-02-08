using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public CanvasGroup m_CanvasGroup;
    public Image m_Panel;
    float m_Time = 0f;
    public float m_FadeTime = 1f;
    
    public void Fade_Out()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    public void Fade_In()
    {
        m_Panel.gameObject.SetActive(true);
        StartCoroutine(FadeInCoroutine());
    }
    public void FadeIn_Out()
    {
        m_Panel.gameObject.SetActive(true);
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
       
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

        yield return new WaitForSeconds(1f);
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

    IEnumerator FadeOutCoroutine()
    {
        // Canvas Groups 컴포넌트 방법
        //m_Time = 0f;

        //while (m_Time < m_FadeTime)
        //{
        //    m_Time += Time.deltaTime;
        //    m_CanvasGroup.alpha = 1f - Mathf.Clamp01(m_Time / m_FadeTime);
        //    yield return null;
        //}

        //m_CanvasGroup.alpha = 0f;


        //이미지 Color 방법
        m_Panel.gameObject.SetActive(true);
        m_Time = 0f;
        Color Alpha = m_Panel.color;

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

    IEnumerator FadeInCoroutine()
    {
        //// Canvas Groups 컴포넌트 방법
        //m_CanvasGroup.gameObject.SetActive(true);
        //m_Time = 0f;

        //while (m_Time < m_FadeTime)
        //{
        //    m_Time += Time.deltaTime;
        //    m_CanvasGroup.alpha = Mathf.Clamp01(m_Time / m_FadeTime);
        //    yield return null;
        //}

        //m_CanvasGroup.alpha = 1f;


        //이미지 Color 방법
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

        yield return null;



    }
}
