using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public Button m_StartButton;
    public Button m_SettingButton;
    public Button m_QuitButton;
    public Image m_SelectImage;

    public AudioSource m_BackSoundSouce;
    public AudioSource m_ButtonSoundSouce;

    public AudioClip m_BackBusic;
    public AudioClip m_ButtonMoveSound;
    public AudioClip m_ButtonSelectSound;

    private Button[] m_Buttons;
    private Color m_DefaultColor = Color.white;
    private Color m_SelectedColor = Color.yellow;
    private GameObject m_LastSelectedButton;

    void Start()
    {
        m_Buttons = new Button[] { m_StartButton, m_SettingButton, m_QuitButton };
        EventSystem.current.SetSelectedGameObject(m_StartButton.gameObject);
        m_LastSelectedButton = m_StartButton.gameObject;
        Update_Button();

        m_StartButton.onClick.AddListener(Start_Game);
        m_QuitButton.onClick.AddListener(Quit_Game);

        m_BackSoundSouce.clip = m_BackBusic;
        m_BackSoundSouce.loop = true;
        m_BackSoundSouce.Play();

    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(m_LastSelectedButton);
        }

        else
        {
            m_LastSelectedButton = EventSystem.current.currentSelectedGameObject;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_BackSoundSouce.PlayOneShot(m_ButtonMoveSound);
            Select_NextButton();
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_BackSoundSouce.PlayOneShot(m_ButtonMoveSound);
            Select_PreviousButton();
        }

        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            m_BackSoundSouce.PlayOneShot(m_ButtonSelectSound);
            ClickSelect_Button();
        }

        Update_Button();
    }

    private void Select_NextButton()
    {
        GameObject Selected = EventSystem.current.currentSelectedGameObject;

        if (Selected == m_StartButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(m_SettingButton.gameObject);
        }

        else if (Selected == m_SettingButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(m_QuitButton.gameObject);
        }

        else if (Selected == m_QuitButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(m_StartButton.gameObject);
        }

        // 새로운 선택 항목이 포커스 받을떄까지 대기
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
    }

    private void Select_PreviousButton()
    {
        GameObject Selected = EventSystem.current.currentSelectedGameObject;

        if (Selected == m_StartButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(m_QuitButton.gameObject);
        }

        else if (Selected == m_SettingButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(m_StartButton.gameObject);
        }

        else if (Selected == m_QuitButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(m_SettingButton.gameObject);
        }

        // 새로운 선택 항목이 포커스 받을떄까지 대기
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
    }

    private void ClickSelect_Button()
    {
        GameObject Selected = EventSystem.current.currentSelectedGameObject;

        if (Selected == m_StartButton.gameObject)
        {
            m_StartButton.onClick.Invoke();
        }

        else if (Selected == m_StartButton.gameObject)
        {
            m_SettingButton.onClick.Invoke();
        }

        else if (Selected == m_QuitButton.gameObject)
        {
            m_QuitButton.onClick.Invoke();
        }
    }

    private void Update_Button()
    {
        GameObject Selected = EventSystem.current.currentSelectedGameObject;

        foreach (Button button in m_Buttons)
        {
            TextMeshProUGUI ButtonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (button.gameObject == Selected)
            {
                ButtonText.color = m_SelectedColor;

                RectTransform ButtonRect = button.GetComponent<RectTransform>();
                RectTransform IndicatorRect = m_SelectImage.GetComponent<RectTransform>();
                IndicatorRect.position = new Vector3(ButtonRect.position.x + ButtonRect.rect.width / 2 + IndicatorRect.rect.width / 2,
                    ButtonRect.position.y, ButtonRect.position.z);
            }

            else
            {
                ButtonText.color = m_DefaultColor;
            }
        }
    }

    private void Start_Game()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void Quit_Game()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
Application.Quit();

#endif

    }
}
