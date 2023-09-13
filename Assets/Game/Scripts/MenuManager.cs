using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject firstPanel, SettingPanel;

    public void PlayBtnClicked()
    {
        SceneManager.LoadScene(1);
    }
    public void SettingBtnClicked()
    {
        SettingPanel.SetActive(true);
    }
    public void BackbtnSettingPanel()
    {
        SettingPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
