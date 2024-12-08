using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class Subtitle
{
    public float m_StartTime;
    public float m_EndTime;
    public string m_Text;
}
public class GameSceneManager : MonoBehaviour
{
    public VideoPlayer m_FirstVidioPlayer; // 첫번쨰 비디오 플레이어
    public PlayableDirector m_PlayableDirector; // 타임라인
    public VideoPlayer m_SecondVidioPlayer; // 두번쨰비디오 플레이어
    public RawImage m_VideoScreen; // 비디오 표시할 이미지
    public RenderTexture m_VideoRender; // 비디오 렌더텍스쳐
    public Fade m_Fade; // 페이드 이미지
    public GameObject m_DefaultUI; // 기본UI

    public TextMeshProUGUI m_SubtitleText; // 자막 TextMeshPro

    public List<Subtitle> m_FirstVideoSubText = new List<Subtitle>
    {
        new Subtitle { m_StartTime = 10f, m_EndTime = 20f, m_Text = "그땐 몰랐었다" },
        new Subtitle { m_StartTime = 30f, m_EndTime = 40f, m_Text = "너가 있는지" }
    };
    public List<Subtitle> m_SecondVideoSubText; // 두번쨰 동영상자막

    private Coroutine m_SubTitleCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        m_VideoScreen.texture = m_VideoRender;

        m_VideoScreen.gameObject.SetActive(true);
        m_FirstVidioPlayer.gameObject.SetActive(true);

        m_FirstVidioPlayer.prepareCompleted += OnFirstVideoPrepared;
        m_FirstVidioPlayer.errorReceived += OnVideoErrorReceived;

        
 
        m_FirstVidioPlayer.Prepare();
     
    }

    void OnFirstVideoPrepared(VideoPlayer _Video)
    {
        Debug.Log("첫번째 동영상 준비 완료");
        StartCoroutine(PlayFistVidio());
    }

    
    void OnSecondVideoPrepared(VideoPlayer _Video)
    {
        Debug.Log("두번째 동영상 준비 완료");
        StartCoroutine(PlaySecondVidio());
    }

    void OnVideoErrorReceived(VideoPlayer _Video, string _Message)
    {
        Debug.LogError("비디오 재생 오류 :" + _Message);
    }


    IEnumerator PlayFistVidio()
    {
        m_DefaultUI.SetActive(false);
       
        m_FirstVidioPlayer.Play();
        m_Fade.Fade_Out();

        Debug.Log("첫번쨰 동영상 재생");

        m_SubtitleText.gameObject.SetActive(true);
        m_SubTitleCoroutine = StartCoroutine(ShowSubtitles(m_FirstVidioPlayer, m_FirstVideoSubText));

        while (m_FirstVidioPlayer.isPlaying)
        {
            yield return null;
        }

        m_Fade.Fade_In();
        Debug.Log("첫번쨰 동영상 끝");
        

        m_FirstVidioPlayer.gameObject.SetActive(false);
        m_VideoScreen.gameObject.SetActive(false);

        
        yield return StartCoroutine(PlayerCutScene());
    }

    IEnumerator PlayerCutScene()
    {
        m_PlayableDirector.gameObject.SetActive(true);
        m_PlayableDirector.Play();
        m_Fade.Fade_Out();

        Debug.Log("컷신 재생");

        while (m_PlayableDirector.state == PlayState.Playing)
        {
            yield return null;
        }

        
        m_PlayableDirector.gameObject.SetActive(false);

        Debug.Log("컷신 끝");

        m_Fade.FadeIn_Out();
        
        m_VideoScreen.gameObject.SetActive(true);
        m_SecondVidioPlayer.gameObject.SetActive(true);

        m_SecondVidioPlayer.prepareCompleted += OnSecondVideoPrepared;
        m_SecondVidioPlayer.errorReceived += OnVideoErrorReceived;

        m_SecondVidioPlayer.Prepare();
    }

    IEnumerator PlaySecondVidio()
    {
        
        m_SecondVidioPlayer.Play();
        
        Debug.Log("두번쨰 동영상 재생");

        while (m_SecondVidioPlayer.isPlaying)
        {
            if (m_SecondVidioPlayer.length - m_SecondVidioPlayer.time <= 1.5f)
            {
                m_Fade.FadeIn_Out();
            }
            yield return null;
        }
        
        Debug.Log("두번쨰 동영상 끝");
        m_DefaultUI.SetActive(true);
        m_SecondVidioPlayer.gameObject.SetActive(false);
        m_VideoScreen.gameObject.SetActive(false);
        yield return null;
    }

    IEnumerator ShowSubtitles(VideoPlayer _VideoPlayer, List<Subtitle> _SubTitles)
    {
        int SubtitleIndex = 0;
        m_SubtitleText.text = "";

        while (_VideoPlayer.isPlaying && SubtitleIndex < _SubTitles.Count)
        {
            float currentTime = (float)_VideoPlayer.time;
           

            if (currentTime >= _SubTitles[SubtitleIndex].m_StartTime && currentTime <= _SubTitles[SubtitleIndex].m_EndTime)
            {
                m_SubtitleText.text = _SubTitles[SubtitleIndex].m_Text;
            }
            else if (currentTime > _SubTitles[SubtitleIndex].m_EndTime)
            {
                SubtitleIndex++;
                if (SubtitleIndex < _SubTitles.Count)
                {
                    m_SubtitleText.text = "";
                }
            }

            yield return null;
        }

        m_SubtitleText.text = "";
    }

}
