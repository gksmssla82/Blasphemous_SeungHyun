using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    public float m_DeltaTime;
    public bool m_ChaseTaget;
    public PlayerController m_Player;
    public float m_Spped;
    private Vector3 m_TargetPos;
    

    private void Start()
    {
        m_Player = Object.FindAnyObjectByType<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        m_DeltaTime -= Time.deltaTime;

        
            if (m_ChaseTaget)
            {
                m_TargetPos.Set(m_Player.transform.position.x, m_Player.transform.position.y, this.transform.position.z);
                this.transform.position = Vector3.Lerp(this.transform.position, m_TargetPos, m_Spped * Time.deltaTime);
            }

            if (m_DeltaTime <= 0)
            {
                Destroy(this.gameObject);
            }
        
    }
}
