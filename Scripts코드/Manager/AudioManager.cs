using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region SoundClass
[System.Serializable] // 커스텀 클래스를 유니티에서 강제로 값바꾸는걸 뛰움
public class Sound
{
    public string m_Name; // 사운드이름

    public AudioClip m_Clip; // 사운드 파일
    private AudioSource m_Source; // 사운드 플레이어



    public float m_Volum;
    public float m_Pitch;
    public bool m_Loop;

    public void SetSource(AudioSource _Source)
    {
        m_Source = _Source;
        m_Source.clip = m_Clip;
        m_Source.loop = m_Loop;
        m_Source.volume = m_Volum;
    }

    public void SetVolumn()
    {
        m_Source.volume = m_Volum;
    }


    public void Play()
    {
        m_Source.Play();
    }

    public void Stop()
    {
        m_Source.Stop();
    }

    public void SetLoop()
    {
        m_Source.loop = true;
    }

    public void SetLoopCancel()
    {
        m_Source.loop = false;
    }

    public void PlaySegment(float _StartTime)
    {
        m_Source.time = _StartTime;
        m_Source.Play();
    }

    public void StopAfterTime()
    {
        Stop();
    }
}
#endregion

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] m_Sounds;
    
    private void Start()
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            GameObject SoundObject = new GameObject("사운드 파일 이름 : " + i + " = " + m_Sounds[i].m_Name);
            m_Sounds[i].SetSource(SoundObject.AddComponent<AudioSource>());
            SoundObject.transform.SetParent(this.transform);
        }
    }

    public void Play(string _name)
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (_name == m_Sounds[i].m_Name)
            {
                m_Sounds[i].Play();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (_name == m_Sounds[i].m_Name)
            {
                m_Sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (_name == m_Sounds[i].m_Name)
            {
                m_Sounds[i].SetLoop();
                return;
            }
        }
    }

    public void PlaySegment(string _Name, float _StartTIme, float _StopTime)
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (_Name == m_Sounds[i].m_Name)
            {
                m_Sounds[i].PlaySegment(_StartTIme);
                StartCoroutine(StopSoundAfterTime(m_Sounds[i], _StopTime - _StartTIme));
                return;
            }
        }
    }

    private IEnumerator StopSoundAfterTime(Sound _sound, float _time)
    {
        yield return new WaitForSeconds(_time);
        _sound.Stop();
    }

    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (_name == m_Sounds[i].m_Name)
            {
                m_Sounds[i].SetLoopCancel();
                return;
            }
        }
    }


    public void SetVolumn(string _name, float _Volumn)
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (_name == m_Sounds[i].m_Name)
            {
                m_Sounds[i].SetVolumn();
                return;
            }
        }
    }
}

