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
    public VideoPlayer m_FirstVidioPlayer; // ù���� ���� �÷��̾�
    public PlayableDirector m_PlayableDirector; // Ÿ�Ӷ���
    public VideoPlayer m_SecondVidioPlayer; // �ι������� �÷��̾�
    public RawImage m_VideoScreen; // ���� ǥ���� �̹���
    public RenderTexture m_VideoRender; // ���� �����ؽ���
    public Fade m_Fade; // ���̵� �̹���
    public GameObject m_DefaultUI; // �⺻UI

    public TextMeshProUGUI m_SubtitleText; // �ڸ� TextMeshPro

    public List<Subtitle> m_FirstVideoSubText = new List<Subtitle>
    {
        new Subtitle { m_StartTime = 10f, m_EndTime = 20f, m_Text = "�׶� ��������" },
        new Subtitle { m_StartTime = 30f, m_EndTime = 40f, m_Text = "�ʰ� �ִ���" }
    };
    public List<Subtitle> m_SecondVideoSubText; // �ι��� �������ڸ�

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
        Debug.Log("ù��° ������ �غ� �Ϸ�");
        StartCoroutine(PlayFistVidio());
    }

    
    void OnSecondVideoPrepared(VideoPlayer _Video)
    {
        Debug.Log("�ι�° ������ �غ� �Ϸ�");
        StartCoroutine(PlaySecondVidio());
    }

    void OnVideoErrorReceived(VideoPlayer _Video, string _Message)
    {
        Debug.LogError("���� ��� ���� :" + _Message);
    }


    IEnumerator PlayFistVidio()
    {
        m_DefaultUI.SetActive(false);
       
        m_FirstVidioPlayer.Play();
        m_Fade.Fade_Out();

        Debug.Log("ù���� ������ ���");

        m_SubtitleText.gameObject.SetActive(true);
        m_SubTitleCoroutine = StartCoroutine(ShowSubtitles(m_FirstVidioPlayer, m_FirstVideoSubText));

        while (m_FirstVidioPlayer.isPlaying)
        {
            yield return null;
        }

        m_Fade.Fade_In();
        Debug.Log("ù���� ������ ��");
        

        m_FirstVidioPlayer.gameObject.SetActive(false);
        m_VideoScreen.gameObject.SetActive(false);

        
        yield return StartCoroutine(PlayerCutScene());
    }

    IEnumerator PlayerCutScene()
    {
        m_PlayableDirector.gameObject.SetActive(true);
        m_PlayableDirector.Play();
        m_Fade.Fade_Out();

        Debug.Log("�ƽ� ���");

        while (m_PlayableDirector.state == PlayState.Playing)
        {
            yield return null;
        }

        
        m_PlayableDirector.gameObject.SetActive(false);

        Debug.Log("�ƽ� ��");

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
        
        Debug.Log("�ι��� ������ ���");

        while (m_SecondVidioPlayer.isPlaying)
        {
            if (m_SecondVidioPlayer.length - m_SecondVidioPlayer.time <= 1.5f)
            {
                m_Fade.FadeIn_Out();
            }
            yield return null;
        }
        
        Debug.Log("�ι��� ������ ��");
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
