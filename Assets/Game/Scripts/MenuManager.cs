using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject firstPanel, SettingPanel, AboutPanel;
    [SerializeField] Animator settingPanelAnim, AboutPanelAnim;
    [SerializeField] Button MusicBtn, SoundBtn;
    [SerializeField] Sprite MusicOnImg, SoundOnImg,MusicOffImg,SoundOffImg;

    private void Start()
    {
        MusicSet();
        SoundSet();
    }
    public void PlayBtnClicked()
    {
        SceneManager.LoadScene(1);
    }
    public void SettingBtnClicked()
    {
        SettingPanel.SetActive(true);
        settingPanelAnim.Play("SettingPanel");
    }
    
    public void BackbtnSettingPanel()
    {
        settingPanelAnim.Play("SettingPanelClose");
        StartCoroutine(SettingPanelWait());
    }
    IEnumerator SettingPanelWait()
    {
        yield return new WaitForSeconds(0.5f);
        SettingPanel.SetActive(false);
    }
    public void AboutPanelOpen()
    {
        AboutPanel.SetActive(true);
        AboutPanelAnim.Play("settingPanelAnim");
    }
    public void BackbtnAboutPanel()
    {
        AboutPanelAnim.Play("AboutPanelClose");
        StartCoroutine(AboutPanelWait());
    }
    IEnumerator AboutPanelWait()
    {
        yield return new WaitForSeconds(0.5f);
        AboutPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void MusicManager()
    {
        if(Common.Instance.isMusicPlaying == false)
        {
            Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().mute = false;
            MusicBtn.GetComponent<Image>().sprite = MusicOnImg;
            Common.Instance.isMusicPlaying = true;
        }
        else 
        {
            Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().mute = true;
            MusicBtn.GetComponent<Image>().sprite = MusicOffImg;
            Common.Instance.isMusicPlaying = false;
        }
    }
    public void MusicSet()
    {
        if (Common.Instance.isMusicPlaying == false)
        {
            Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().mute = true;
            MusicBtn.GetComponent<Image>().sprite = MusicOffImg;
        }
        else
        {
            Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().mute = false;
            MusicBtn.GetComponent<Image>().sprite = MusicOnImg;

        }
    }
    public void SoundManager()
    {
        if (Common.Instance.isSoundPlaying == false)
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().mute = false;
            SoundBtn.GetComponent<Image>().sprite = SoundOnImg;
            Common.Instance.isSoundPlaying = true;
        }
        else
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().mute = true;
            SoundBtn.GetComponent<Image>().sprite = SoundOffImg;
            Common.Instance.isSoundPlaying = false;
        }
    }
    public void SoundSet()
    {
        if (Common.Instance.isSoundPlaying == false)
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().mute = true;
            SoundBtn.GetComponent<Image>().sprite = SoundOffImg;
        }
        else
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().mute = false;
            SoundBtn.GetComponent<Image>().sprite = SoundOnImg;

        }
    }
}
