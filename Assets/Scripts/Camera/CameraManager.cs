using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager m_Instance;

    public GameObject m_Target;
    public GameObject m_Enemy;
    private Vector3 m_TargetPos;
    private Vector3 m_EnemyPos;
    public float m_MoveSpeed;
    public float m_AddCameraPosY;
    public float m_AddCameraPosX;

    public BoxCollider2D m_Bound;
    private Vector3 m_MinBound;
    private Vector3 m_MaxBound;

    private float m_HalfWidth;
    private float m_HalfHeight;

    public Camera m_Camera;
    public bool m_Shake;
    public bool m_PlayerChase;

    // Shake
    public float m_Force = 0f;
    public Vector3 m_Offset = Vector3.zero;
    public Quaternion m_OriginRot;

    private void Awake()
    {
        //if (m_Instance != null)
        //{
        //    Destroy(this.gameObject);
        //}
        //else if (m_Instance == null)
        //{
        //    DontDestroyOnLoad(this.gameObject);
        //    m_Instance = this;
        //}
    }
    void Start()
    {

        m_Camera = GetComponent<Camera>();
        m_MinBound = m_Bound.bounds.min;
        m_MaxBound = m_Bound.bounds.max;

        m_HalfHeight = m_Camera.orthographicSize;
        m_HalfWidth = m_HalfHeight * Screen.width / Screen.height;
        m_OriginRot = this.transform.rotation;
    }


    void Update()
    {
        if (m_PlayerChase && !m_Shake)
        {
            TargetMove();
        }

        if (!m_PlayerChase)
        {
            TargetMove_Enemy();
        }
    }

    public void TargetMove_Enemy()
    {
        if (m_Enemy.gameObject != null)
        {

            m_EnemyPos.Set(m_Enemy.transform.position.x + m_AddCameraPosX, m_Enemy.transform.position.y + m_AddCameraPosY, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, m_EnemyPos, m_MoveSpeed * Time.deltaTime);
            BoundCamera();
        }
    }

    private void TargetMove()
    {
        if (m_Target.gameObject != null)
        {
            
            m_TargetPos.Set(m_Target.transform.position.x + m_AddCameraPosX, m_Target.transform.position.y + m_AddCameraPosY, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, m_TargetPos, m_MoveSpeed * Time.deltaTime);
            BoundCamera();
        }
    }

    private void BoundCamera()
    {
        // 최솟값엔 반너비를 더해주고 최댓값엔 반너비를 뺴줌
        float ClampedX = Mathf.Clamp(this.transform.position.x, m_MinBound.x + m_HalfWidth, m_MaxBound.x - m_HalfWidth);
        float ClampedY = Mathf.Clamp(this.transform.position.y, m_MinBound.y + m_HalfHeight, m_MaxBound.y - m_HalfHeight);
        this.transform.position = new Vector3(ClampedX, ClampedY, this.transform.position.z);
    }

    public void SetBound(BoxCollider2D _NewBound)
    {
        m_Bound = _NewBound;
        m_MinBound = m_Bound.bounds.min;
        m_MaxBound = m_Bound.bounds.max;
    }

    //IEnumerator ShakeCoroutine()
    //{
    //    // 카메라 오일러값 초기지정
    //    Vector3 OriginEuler = transform.eulerAngles;

    //    while (true)
    //    {
    //        // 벡터 축마다 랜덤값부여
    //        float RotX = Random.Range(-m_Offset.x, m_Offset.x);
    //        float RotY = Random.Range(-m_Offset.y, m_Offset.y);
    //        float RotZ = Random.Range(-m_Offset.z, m_Offset.z);

    //        // 흔들림값 = 초기값 + 랜덤값
    //        Vector3 RandomRot = OriginEuler + new Vector3(RotX, RotY, RotZ);
    //        Quaternion TransQuat = Quaternion.Euler(RandomRot);

    //        while(Quaternion.Angle(transform.rotation, TransQuat) > 0.1f)
    //        {

    //        }
    //    }
    //}


    public IEnumerator Shake(float duration, float magnitude, Vector2 direction)
    {
        m_Shake = true;
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude * direction.x;
            float offsetY = Random.Range(-1f, 1f) * magnitude * direction.y;

            transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsed += Time.deltaTime;

            m_Shake = false;
            yield return null;
        }

        transform.localPosition = originalPosition;
        
    }
}
