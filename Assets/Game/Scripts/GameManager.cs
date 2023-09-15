using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject firstPanel, secondPanel;
    public void BackBtnFirstPanel()
    {
        SceneManager.LoadScene(0);
    }
    public void BackBtnSecondPanel()
    {
        secondPanel.SetActive(false);
        firstPanel.SetActive(true);
    }
    int selectedField;
    public void SecondPanelOpen(int category)
    {
        selectedField = category;
        firstPanel.SetActive(false);
        secondPanel.SetActive(true);
    }
}
