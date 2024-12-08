using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnemyInfo
{
    public GameObject m_Prefabs;
    public Transform m_EnemyTransform;
    [HideInInspector]
    public GameObject m_Instance;
}
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI m_ScroeText;
    public int m_Score = 0;
    public int m_DisplayedScore = 0;
    public Fade m_FadeImage;
    public Fade m_DeadthUI;
    public Fade m_AnyKeyUI;
    public List<EnemyInfo> m_Enemys = new List<EnemyInfo>();
    public Enemy m_Enemy;
    
    public int m_PlayMusicTrack;

    private void Awake()
    {

        
        m_Enemy = FindAnyObjectByType<Enemy>();
    }
    private void Start()
    {
        UpdateScoreText();
        InitializeEnemys();

        BgmManager.Instance.Play(m_PlayMusicTrack);
        
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.P))
        {
            Death_Ui();
        }

       if (Input.GetKeyDown(KeyCode.O))
        {
            m_FadeImage.Fade_Out();
            m_DeadthUI.Fade_Out();
        }

       
    }

    public void Set_MusicTrack(int _int)
    {
        m_PlayMusicTrack = _int;
    }

    private void InitializeEnemys()
    {
        foreach (var enemyInfo in m_Enemys)
        {
            enemyInfo.m_Instance = Instantiate(enemyInfo.m_Prefabs, enemyInfo.m_EnemyTransform.position, enemyInfo.m_EnemyTransform.rotation);
            enemyInfo.m_Instance.SetActive(false);
        }
    }
    

    public void RespawnMonsters()
    {
        foreach (var enemyInfo in m_Enemys)
        {
            
            if (enemyInfo.m_Prefabs.activeSelf == false)
            {
                enemyInfo.m_Instance.transform.position = enemyInfo.m_EnemyTransform.position;
                enemyInfo.m_Instance.transform.rotation = enemyInfo.m_EnemyTransform.rotation;
                enemyInfo.m_Instance.SetActive(true);
            }

         
        }
    }

    public void DeactiavateEnemy(GameObject _Enemy)
    {
        _Enemy.SetActive(false);
    }

    public void Death_Ui()
    {
        StartCoroutine(Death_Coroutine());
    }
    private IEnumerator Death_Coroutine()
    {
        m_FadeImage.Fade_In();
        m_DeadthUI.Fade_In();

        yield return new WaitForSeconds(m_DeadthUI.m_FadeTime);

        m_AnyKeyUI.Fade_In();

        yield return new WaitForSeconds(m_AnyKeyUI.m_FadeTime);

        yield return new WaitUntil(() => Input.anyKeyDown);

        m_AnyKeyUI.Fade_Out();
        m_DeadthUI.Fade_Out();

    }

    public void SlowTime(float _Duration, float _SlowAmount)
    {
        StartCoroutine(SlowTime_Coroutine(_Duration, _SlowAmount));
    }

    public void AddScore(int _Point)
    {
        int TargetScore = m_Score + _Point;
        StartCoroutine(AnimScore_Coroutine(TargetScore, 1.0f));
        m_Score = TargetScore;
    }

    private IEnumerator SlowTime_Coroutine(float _Duration, float _SlowAmount)
    {
        // 시간 느리게설정
        Time.timeScale = _SlowAmount;
        // 물리엔진을 0.02초마다 업데이트하니 0.02곱해줌
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        // Duration 동안 대기 (실제시간)
        yield return new WaitForSecondsRealtime(_Duration);

        // 정상시간으로 복귀
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private IEnumerator AnimScore_Coroutine(int _TargetScore, float _Duration)
    {
        float StartTime = Time.time;
        int StartScore = m_DisplayedScore;

        while (Time.time  < StartTime + _Duration)
        {
            float t = (Time.time - StartTime) / _Duration;
            m_DisplayedScore = Mathf.RoundToInt(Mathf.Lerp(StartScore, _TargetScore, t));
            UpdateScoreText();
            yield return null;
        }

        m_DisplayedScore = _TargetScore;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        m_ScroeText.text = m_DisplayedScore.ToString();
    }


}
