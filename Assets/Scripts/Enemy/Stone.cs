using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Animator m_Anim;
    private Rigidbody2D m_Rigid;
    private BoxCollider2D m_BoxColider;
    public float m_Speed;
    void Start()
    {
        m_BoxColider = GetComponent<BoxCollider2D>();
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHit") ||  collision.gameObject.CompareTag("PlayerParry") || 
            collision.gameObject.CompareTag("Grounds"))
        {
            m_BoxColider.enabled = false;
            m_Anim.SetTrigger("Broken");

            //m_Rigid.velocity = Vector2.zero;
            //m_Rigid.isKinematic = true;

            m_Rigid.velocity = Vector2.down * m_Speed;
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(m_Anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }


}
