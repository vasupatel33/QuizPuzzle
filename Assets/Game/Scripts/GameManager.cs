using System.Collections;
using System.Collections.Generic;
using Game.Utility;
using GoogleMobileAds.Samples;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject firstPanel, secondPanel, settingPanel, BackgroundClickRemoveImage, QuestionCompletePanel;
    public GameObject gameOverPanel;
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

    public static GameManager instance;
    bool isSlider;
    int ScoreValue=0,HighScoreValue;
    int scorePlayerPrefs;
    private void Start()
    {
        QuestionUnlock();
        GoogleMobileAdsController.Instance.ShowBannerAdd();
        if (quesUnlock)
        {
            Debug.Log("If workss");
            GameObject.FindWithTag("Content").gameObject.transform.GetChild(selectedCategory - 1).gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }
        int value = PlayerPrefs.GetInt("HighScore");
        HighScoreText.text = value.ToString();
        
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
            CatButton[i].interactable = true;
            Debug.Log("Button Unlocked = " + CatButton[i]);
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
    public void GrantReward()
    {
        SceneManager.LoadScene(1);
        gameOverPanel.SetActive(false);
        secondPanel.SetActive(false);
        firstPanel.SetActive(true);
        PlayerPrefs.SetInt("Score", scorePlayerPrefs);
        QuestionUnlock();
    }
    public void QuestionSet()
    {
        PlayerPrefs.SetInt("HighScore", ScoreValue);
        HighScoreText.text = ScoreValue.ToString();

        int QuestionNo = PlayerPrefs.GetInt("Question"+selectedField,0);

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
            Debug.Log("After Set = "+selectedCategory);
            QuestionCompletePanel.SetActive(true);
            BackgroundClickRemoveImage.SetActive(true);
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
            HighScoreValue = PlayerPrefs.GetInt("HighScore", HighScoreValue);
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(CorrectAnsSound);
            SliderImage.fillAmount = 0;
            QuestionAnim.SetTrigger("QuestionTrigger");

            ScoreValue += 50;
            ScoreText.text = ScoreValue.ToString();
            int.TryParse(ScoreText.text, out scorePlayerPrefs);
            PlayerPrefs.SetInt("Score", scorePlayerPrefs);
            QuestionNo++;
            PlayerPrefs.SetInt("Question" + selectedField, QuestionNo);
            ScoreTextGameOverPanel.text = ScoreText.text;

            QuestionSet();
        }
        else
        {
            StartCoroutine(LifeBarHealthClose());
        }
    }
    int index=3;
    int vall = 1;
    IEnumerator LifeBarHealthClose()
    {
        int LifeCount = PlayerPrefs.GetInt("LifePref", 3);
        
        GameSecondPanel[index-1].SetTrigger("WrongAns");
        index--;
        if (vall < 3)
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(WrongAnswerSound);
            vall++;
        }
        else
        {
            vall = 1;
        }
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Index = "+index);
        if (index == 0)
        {
            Debug.Log("Befor val = " + ScoreValue);
            if (ScoreValue > HighScoreValue)
            {
                HighScoreValue = ScoreValue;
                Debug.Log("high Score val = " + HighScoreValue);
                PlayerPrefs.SetInt("HighScore", HighScoreValue);
                HighScoreText.text = HighScoreValue.ToString();
            }
            else
            {
                HighScoreText.text = HighScoreValue.ToString();
            }
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(GameOverSound);
            Debug.Log("GameOVer SOund Playssss");
            gameOverPanel.SetActive(true);
            BackgroundClickRemoveImage.SetActive(true);
            SliderImage.fillAmount = 0;
            isSlider = false;
        }
    }
    public void RewardMethodForGiveLife()
    {
        GoogleMobileAdsController.Instance.ShowRewardAdd();
    }
    public void UnlockLevelButtonClicked()
    {
        QuestionCompletePanel.SetActive(false);
        secondPanel.SetActive(false);
        firstPanel.SetActive(true);
        isSlider = false;
        BackgroundClickRemoveImage.SetActive(false);
        QuestionUnlock();
        GoogleMobileAdsController.Instance.ShowRewardAdd();
    }
    public void BackBtnGameOverPanel()
    {
        PlayerPrefs.SetInt("Score", scorePlayerPrefs);
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
        HighScoreValue = PlayerPrefs.GetInt("HighScore");
        Debug.Log("Start High Score = "+HighScoreValue);
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
