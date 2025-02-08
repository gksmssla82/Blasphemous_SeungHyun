using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thron : MonoBehaviour
{

    private Animator m_Anim;
    private Rigidbody2D m_Rigid;
    private BoxCollider2D m_BoxColider;
    private AudioManager m_Audio;

    


    public float m_Speed;

    public bool m_isGrounds;
    public bool m_isDestroy;

    public Trail m_Trail;
    void Start()
    {
        m_BoxColider = GetComponent<BoxCollider2D>();
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody2D>();
        m_isGrounds = false;
        m_isDestroy = false;

        if (m_Audio == null)
        {
            m_Audio = Object.FindAnyObjectByType<AudioManager>();
        }

    }

    private void Update()
    {
        AnimatorStateInfo StateInfo = m_Anim.GetCurrentAnimatorStateInfo(0);
        
        if (StateInfo.IsName("Idle"))
        {
            m_Trail.m_CreateTrail = true;
        }
        else
        {
            m_Trail.m_CreateTrail = false;
        }

       
        if (IsAnimationFinished("Thron_Grounds"))
        {

            m_isDestroy = true;
            m_BoxColider.isTrigger = true;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_isGrounds && collision.gameObject.CompareTag("PlayerAttack") || collision.gameObject.CompareTag("PlayerParry") 
            || collision.gameObject.CompareTag("PlayerHit"))
        {
            m_BoxColider.enabled = false;
            m_Anim.SetTrigger("Destroy");

            if (collision.gameObject.CompareTag("PlayerAttack"))
            {
                int Temp = Random.Range(1, 3);

                switch (Temp)
                {
                    case 1:
                        m_Audio.Play("Split_Hit1");
                        break;
                    case 2:
                        m_Audio.Play("Split_Hit2");
                        break;
                }
            }
            

            m_Rigid.velocity = Vector2.down * m_Speed;
            StartCoroutine(DestroyAfterAnimation());
        }

        else if (collision.gameObject.CompareTag("Grounds"))
        {

            m_Audio.Play("Split_Root");
            m_Rigid.velocity = Vector2.zero;
            m_BoxColider.isTrigger = false;
            m_Rigid.isKinematic = true;
            m_Anim.SetTrigger("Grounds");
            m_isGrounds = true;
           
        }

        if (m_isDestroy && collision.gameObject.CompareTag("PlayerAttack") || collision.gameObject.CompareTag("PlayerHit"))
        {
            

            if (collision.gameObject.CompareTag("PlayerAttack"))
            {
                int Temp = Random.Range(1, 3);

                switch (Temp)
                {
                    case 1:
                        m_Audio.Play("Split_Hit1");
                        break;
                    case 2:
                        m_Audio.Play("Split_Hit2");
                        break;
                }
            }

            m_Audio.Play("Split_Destroy");
            m_BoxColider.enabled = false;
            m_Anim.SetTrigger("GroundsDestroy");
            StartCoroutine(DestroyAfterAnimation());
        }

    }

    private bool IsAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = m_Anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 0.95f;
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(m_Anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
