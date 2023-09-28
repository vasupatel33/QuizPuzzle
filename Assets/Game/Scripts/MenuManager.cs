using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject firstPanel, SettingPanel, AboutPanel, BackGroundClickRemoveImage;
    [SerializeField] Animator settingPanelAnim, AboutPanelAnim;
    [SerializeField] Button MusicBtn, SoundBtn;
    [SerializeField] Sprite MusicOnImg, SoundOnImg,MusicOffImg,SoundOffImg;
    [SerializeField] AudioClip ClickSound;
    private void Start()
    {
        MusicSet();
        SoundSet();
    }
    public void PlayBtnClicked()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SceneManager.LoadScene(1);
    }
    public void SettingBtnClicked()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SettingPanel.SetActive(true);
        BackGroundClickRemoveImage.SetActive(true);
        settingPanelAnim.Play("SettingPanel");
    }
    
    public void BackbtnSettingPanel()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        settingPanelAnim.Play("SettingPanelClose");
        BackGroundClickRemoveImage.SetActive(false);
        StartCoroutine(SettingPanelWait());
    }
    IEnumerator SettingPanelWait()
    {
        yield return new WaitForSeconds(0.5f);
        SettingPanel.SetActive(false);
    }
    public void AboutPanelOpen()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        AboutPanel.SetActive(true);
        BackGroundClickRemoveImage.SetActive(true);
        AboutPanelAnim.Play("settingPanelAnim");
    }
    public void BackbtnAboutPanel()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        AboutPanelAnim.Play("AboutPanelClose");
        BackGroundClickRemoveImage.SetActive(false);
        StartCoroutine(AboutPanelWait());
    }
    IEnumerator AboutPanelWait()
    {
        yield return new WaitForSeconds(0.5f);
        AboutPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
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
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
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
