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
    [SerializeField] GameObject firstPanel, secondPanel, settingPanel, gameOverPanel, BackgroundClickRemoveImage, QuestionCompletePanel;
    [SerializeField] private Animator BlackRoundAnim;
    [SerializeField] private Animator settingPanelAnim;
    [SerializeField] private Animator[] GameSecondPanel;
    [SerializeField] private Button[] CatButton;
    [SerializeField] Button MusicBtn, SoundBtn;
    [SerializeField] Sprite MusicOnImg, SoundOnImg, MusicOffImg, SoundOffImg;
    [SerializeField] Image SliderImage;
    [SerializeField] private Animator QuestionAnim;
    [SerializeField] AudioClip ClickSound, CorrectAnsSound, GameOverSound, WrongAnswerSound;
    [SerializeField] Category[] AllCat;
    [SerializeField] TextMeshProUGUI QuestionTxt;
    [SerializeField] TextMeshProUGUI[] AllOptText;
    [SerializeField] TextMeshProUGUI CatHeadingText;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI HighScoreText;
    [SerializeField] TextMeshProUGUI ScoreTextGameOverPanel;
            
    
    bool isSlider;
    //int questionNo;
    int ScoreValue=0,HighScoreValue;
    int scorePlayerPrefs;
    private void Start()
    {
        if (quesUnlock)
        {
            Debug.Log("If workss");
            GameObject.FindWithTag("Content").gameObject.transform.GetChild(selectedCategory - 1).gameObject.GetComponent<SpriteRenderer>().color = Color.black;

        }
        Debug.Log("Start called");
        int value = PlayerPrefs.GetInt("HighScore");
        HighScoreText.text = value.ToString();
        QuestionUnlock();
        
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
        MusicSet();
        SoundSet();
    }
    public void QuestionUnlock()
    {
        int CatPrefs = PlayerPrefs.GetInt("CatPref", 0);
        Debug.Log("Before  Category Prefabs = " + CatPrefs);
        for (int i = 0; i <= CatPrefs; i++)
        {
            Debug.Log("Selected CAt = " + selectedCategory);
            Debug.Log("Category Prefabs = " + CatPrefs);

            CatButton[i].interactable = true;
            PlayerPrefs.SetInt("CatPref", selectedCategory);
        }
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
    }
    public void QuestionSet()
    {
        int QuestionNo = PlayerPrefs.GetInt("Question"+selectedField,0);
        Debug.Log("Playerpreffffffffffff = " + QuestionNo);

        if (QuestionNo < AllCat[selectedField].AllQuestion.Length)
        {
            for (int j = 0; j < AllCat[selectedField].AllQuestion.Length; j++)
             {
                QuestionTxt.text = AllCat[selectedField].AllQuestion[QuestionNo].QuestionName;

                 Debug.Log(AllCat[selectedField].AllQuestion[QuestionNo].Answer);

                for (int k = 0; k < AllCat[selectedField].AllQuestion[QuestionNo].AllOption.Length; k++)
                {
                    AllOptText[k].text = AllCat[selectedField].AllQuestion[QuestionNo].AllOption[k].OptionsName;
                }
            }
        }
        else
        {
            selectedCategory++;
            quesUnlock = true;
            PlayerPrefs.SetInt("CatPref",selectedCategory);
            Debug.Log("All question overr");
            QuestionCompletePanel.SetActive(true);
            BackgroundClickRemoveImage.SetActive(true);
            Debug.Log("Category = "+selectedCategory);
        }
    }
    bool quesUnlock;
    public void CheckAnswer(TextMeshProUGUI OptText)
    {
        int QuestionNo = PlayerPrefs.GetInt("Question" + selectedField, 0);

        string ans = AllCat[selectedField].AllQuestion[QuestionNo].Answer;
        OptText.text = OptText.text.Replace(" ", "");
        ans = ans.Replace(" ", "");
        if (OptText.text == ans)
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(CorrectAnsSound);
            SliderImage.fillAmount = 0;
            QuestionAnim.SetTrigger("QuestionTrigger");
            
            ScoreValue+=50;
            ScoreText.text = ScoreValue.ToString();
            int.TryParse(ScoreText.text, out scorePlayerPrefs);
            PlayerPrefs.SetInt("Score",scorePlayerPrefs);
            QuestionNo++;
            PlayerPrefs.SetInt("Question"+selectedField, QuestionNo);
            ScoreTextGameOverPanel.text = ScoreText.text;

            HighScoreText.text = HighScoreValue.ToString();
            if (ScoreValue > HighScoreValue)
            {
                PlayerPrefs.SetInt("HighScore", ScoreValue);
                HighScoreText.text = ScoreValue.ToString();
            }

            QuestionSet();
        }
        else
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(WrongAnswerSound);
            StartCoroutine(LifeBarHealthClose());
            Debug.Log("Answer is wrong");
        }
    }
    int index=3;
    IEnumerator LifeBarHealthClose()
    {
        GameSecondPanel[index-1].SetTrigger("WrongAns");
        yield return new WaitForSeconds(0.5f);

        index--;
        if(index == 0)
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(GameOverSound);
            gameOverPanel.SetActive(true);
            BackgroundClickRemoveImage.SetActive(true);
            SliderImage.fillAmount = 0;
            isSlider = false;
            index = 3;
        }
    }
    public void UnlockLevelButtonClicked()
    {
        QuestionCompletePanel.SetActive(false);
        secondPanel.SetActive(false);
        firstPanel.SetActive(true);
        isSlider = false;
        BackgroundClickRemoveImage.SetActive(false);
        PlayerPrefs.GetInt("CatPref");
        QuestionUnlock();
        if (quesUnlock)
        {
            GameObject.FindWithTag("Content").gameObject.transform.GetChild(selectedCategory - 1).gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            //GameObject.FindWithTag("Content").gameObject.transform.GetChild(selectedCategory - 1).gameObject.SetActive(false);
        }
    }
    public void BackBtnGameOverPanel()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        gameOverPanel.SetActive(false);
        firstPanel.SetActive(true);
        secondPanel.SetActive(false);
        BackgroundClickRemoveImage.SetActive(false);
    }
    public void HomeBtnSettingPanel()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SceneManager.LoadScene(0);
    }
    public void PauseBtnFirstPanel()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        settingPanelAnim.Play("SettingPanel");
        BackgroundClickRemoveImage.SetActive(true);
        settingPanel.SetActive(true);
    }
    public void SettingPanelClose()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
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
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        PlayerPrefs.GetInt("CatPref");
        QuestionUnlock();
        
        Debug.Log("Category = "+selectedCategory);
        isSlider = false;
        BlackRoundAnim.SetTrigger("Start");
        StartCoroutine(SecondPanelWait());  
    }
    int selectedField;
    int selectedCategory;
    public void SecondPanelOpen(int category)
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        selectedField = category;
        selectedCategory = category;
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
            QuestionCompletePanel.SetActive(false);
            secondPanel.SetActive(false);
            firstPanel.SetActive(true);
            isSlider = false;
            if (quesUnlock)
            {
                GameObject.FindWithTag("Content").gameObject.transform.GetChild(selectedCategory - 1).gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                //GameObject.FindWithTag("Content").gameObject.transform.GetChild(selectedCategory - 1).gameObject.SetActive(false);
                PlayerPrefs.GetInt("CatPref",selectedField);
            }
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
