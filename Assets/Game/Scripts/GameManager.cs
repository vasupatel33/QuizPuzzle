using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject firstPanel, secondPanel, settingPanel, gameOverPanel, BackgroundClickRemoveImage;
    [SerializeField] private Animator BlackRoundAnim;
    [SerializeField] private Animator settingPanelAnim;
    [SerializeField] private Animator[] GameSecondPanel;
    [SerializeField] Button MusicBtn, SoundBtn;
    [SerializeField] Sprite MusicOnImg, SoundOnImg, MusicOffImg, SoundOffImg;
    [SerializeField] Image SliderImage;
    [SerializeField] private Animator QuestionAnim;
    [SerializeField] AudioClip ClickSound, CorrectAnsSound, GameOverSound;
    [SerializeField] Category[] AllCat;
    [SerializeField] TextMeshProUGUI QuestionTxt;
    [SerializeField] TextMeshProUGUI[] AllOptText;
    [SerializeField] TextMeshProUGUI CatHeadingText;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI HighScoreText;
    [SerializeField] TextMeshProUGUI ScoreTextGameOverPanel;

    
    bool isSlider;
    int questionNo;
    int ScoreValue=50,HighScoreValue;
    int scorePlayerPrefs;
    private void Start()
    {

        MusicSet();
        SoundSet();
        //for(int i = 0; i < AllCat.Length; i++)
        //{
        //    Debug.Log(AllCat[i].CatName);
        //    for(int j = 0; j < AllCat[i].AllQuestion.Length; j++)
        //    {
        //        Debug.Log(AllCat[i].AllQuestion[j].QuestionName);
        //        Debug.Log(AllCat[i].AllQuestion[j].Answer);
        //        for(int k = 0; k < AllCat[i].AllQuestion[j].AllOption.Length; k++)
        //        {
        //            Debug.Log(AllCat[i].AllQuestion[j].AllOption[k].OptionsName);
        //        }
        //    }

        //}
        HighScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    private void Update()
    {
        if (isSlider)
        {
            if (SliderImage.fillAmount < 1)
            {
                SliderImage.fillAmount += 0.05f * Time.deltaTime;
            }
            else
            {
                gameOverPanel.SetActive(true);
                BackgroundClickRemoveImage.SetActive(true);
            }
        }
        //ScoreText.text = ScoreVar.ToString();
        HighScoreText.text = HighScoreValue.ToString();
        if(ScoreValue > HighScoreValue)
        {
            PlayerPrefs.SetInt("HighScore",);
            HighScoreText.text = ScoreValue.ToString();
        }
    }
    public void QuestionSet()
    {
        for (int j = 0; j < AllCat[selectedField].AllQuestion.Length; j++)
        {
            QuestionTxt.text = AllCat[selectedField].AllQuestion[questionNo].QuestionName;

            //Debug.Log(AllCat[selectedField].AllQuestion[questionNo].QuestionName);
            //Debug.Log(AllCat[selectedField].AllQuestion[questionNo].Answer);

            for (int k = 0; k < AllCat[selectedField].AllQuestion[questionNo].AllOption.Length; k++)
            {
                
                AllOptText[k].text = AllCat[selectedField].AllQuestion[questionNo].AllOption[k].OptionsName;
                Debug.Log(AllCat[selectedField].AllQuestion[selectedField].AllOption[k].OptionsName);
            }
        }
    }
    public void CheckAnswer(TextMeshProUGUI OptText)
    {
        string ans = AllCat[selectedField].AllQuestion[questionNo].Answer;
        OptText.text = OptText.text.Replace(" ", "");
        ans = ans.Replace(" ", "");
        if (OptText.text == ans)
        {
            SliderImage.fillAmount = 0;
            QuestionAnim.SetTrigger("QuestionTrigger");
            Debug.Log("Option text = "+OptText.text);
            Debug.Log("Answer text = "+ans);
            ScoreText.text = ScoreValue.ToString();
            ScoreValue+=50;
            int.TryParse(ScoreText.text, out scorePlayerPrefs);
            PlayerPrefs.SetInt("Score",scorePlayerPrefs);
            questionNo++;
            ScoreTextGameOverPanel.text = ScoreText.text;
            QuestionSet();
        }
        else
        {
            StartCoroutine(LifeBarHealthClose());
            SliderImage.fillAmount = 0;
            Debug.Log("Answer is wrong");
        }
    }
    int i=3;
    IEnumerator LifeBarHealthClose()
    {
        GameSecondPanel[i-1].SetTrigger("WrongAns");
        yield return new WaitForSeconds(1.1f);
        //GameObject.FindWithTag("LifeBar").gameObject.transform.GetChild(i).gameObject.SetActive(false);
        i--;
        if(i== 0)
        {
            gameOverPanel.SetActive(true);
            isSlider = false;
            i = 3;
        }
    }
    public void BackBtnGameOverPanel()
    {
        gameOverPanel.SetActive(false);
        BackgroundClickRemoveImage.SetActive(false);
    }
    public void HomeBtnSettingPanel()
    {
        SceneManager.LoadScene(0);
    }
    public void PauseBtnFirstPanel()
    {
        settingPanelAnim.Play("SettingPanel");
        BackgroundClickRemoveImage.SetActive(true);
        settingPanel.SetActive(true);
    }
    public void SettingPanelClose()
    {
        settingPanelAnim.Play("SettingPanelClose");
        StartCoroutine(SettingPanelWait());
    }
    IEnumerator SettingPanelWait()
    {
        yield return new WaitForSeconds(0.5f);
        BackgroundClickRemoveImage.SetActive(false);
        settingPanel.SetActive(false);
    }
    public void BackBtnSecondPanel()
    {
        isSlider = false;
        BlackRoundAnim.SetTrigger("Start");
        StartCoroutine(SecondPanelWait());  
    }
    int selectedField;
    public void SecondPanelOpen(int category)
    {
        selectedField = category;
        BlackRoundAnim.SetTrigger("Start");
        StartCoroutine(SecondPanelWait());
        CatHeadingText.text = AllCat[selectedField].CatName;
        QuestionSet();
    }
    IEnumerator SecondPanelWait()
    {
        SliderImage.fillAmount = 0;
        yield return new WaitForSeconds(1f);
        isSlider = true;
        QuestionAnim.SetTrigger("QuestionTrigger");
        if (firstPanel.activeInHierarchy)
        {
            secondPanel.SetActive(true);
            firstPanel.SetActive(false);
        }
        else
        {
            secondPanel.SetActive(false);
            firstPanel.SetActive(true);
        }
        
    }
    public void MusicManager()
    {
        if (Common.Instance.isMusicPlaying == false)
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
        if (Common.Instance?.isMusicPlaying == false)
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
