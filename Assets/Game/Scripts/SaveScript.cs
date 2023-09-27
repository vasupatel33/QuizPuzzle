using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveScript : MonoBehaviour
{
    int LoadValue;
    int Value;

    void Start()
    {
        LoadValue = PlayerPrefs.GetInt("LoadValue");// 5
        Value = PlayerPrefs.GetInt("Score", 0);     // 5

        Debug.Log("Value = " + Value);     // 0 1 2 3  unlock val
        Debug.Log("Load Value = " + LoadValue); // 1 2 3 4  btn click val


    }

    public void BtnPresed()
    {
        Debug.Log("Value = " + Value);     // 0 1 2 3  unlock val 4
        Debug.Log("Load Value = " + LoadValue); // 1 2 3 4  btn click val 1

        if (LoadValue >= Value)// 1<0
        {
            //Value = LoadValue;
            Value++;
            PlayerPrefs.SetInt("Score", Value);
        }
        Debug.Log("After Value = " + Value);     // 0 1 2 3  unlock val 4
        Debug.Log(" After Load Value = " + LoadValue); // 1 2 3 4  btn click val 1

    }
    public void BackBtnClicked()
    {
        SceneManager.LoadScene(0);
    }
}
