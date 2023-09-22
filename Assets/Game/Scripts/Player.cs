using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Button[] AllButons;
    int PLayerValue;
    void Start()
    {
        PLayerValue =PlayerPrefs.GetInt("Score",0);
        for(int i = 0 ; i<= PLayerValue ; i++)
        {
            AllButons[i].interactable = true;
            AllButons[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void BtnPressed()
    {
        int Value = PlayerPrefs.GetInt("Score",0);
        Value++;
        PlayerPrefs. SetInt("Score", Value);

        if (Value > PLayerValue)
        {
            for (int i = 0; i <= Value; i++)
            {
                AllButons[Value].interactable = true;
                AllButons[Value].gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

}