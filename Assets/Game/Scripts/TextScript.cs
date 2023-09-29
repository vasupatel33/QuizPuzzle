//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TextScript : MonoBehaviour
//{
//    [SerializeField] TextData[] AllData;
//    void Start()
//    {
//        var data = Resources.Load("Science");
//        Debug.Log(data);
//        string[] txtData = data.ToString().Split('\n');
//        foreach (var item in txtData)
//        {
//            Debug.Log("" + item);
//            //int IndexItem = (int)item;
//        }
//        //string textdata = data.ToString().Split('\n');
//    }
//    public void CatBtnClicked(int val)
//    {
//        for(int i = 0; i < AllData.Length; i++)
//        {
//            Debug.Log(AllData.Length);
//        }
//    }
//}
//[System.Serializable]
//class TextData
//{
//    public string QuestionTxt;
//    public string[] OptionTxt;
//    public string AnswerTxt;

//}




















using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the Unity UI namespace

public class TextScript : MonoBehaviour
{
    [SerializeField] TextData[] AllData;

    // Reference to your UI Text components
    public Text questionText;
    public Text[] optionTexts;
    public Text answerText;

    void Start()
    {
        var data = Resources.Load<TextAsset>("Science");

        if (data != null)
        {
            string[] txtData = data.text.Split('\n');

            // Assuming you have at least 6 lines in your text file
            if (txtData.Length >= 6)
            {
                // Assign lines to text fields
                questionText.text = txtData[0];
                for (int i = 0; i < optionTexts.Length; i++)
                {
                    optionTexts[i].text = txtData[i + 1];
                }
                answerText.text = txtData[5];
            }
            else
            {
                Debug.LogError("Not enough lines in the text file.");
            }
        }
        else
        {
            Debug.LogError("Failed to load the text file from Resources.");
        }
    }

    public void CatBtnClicked(int val)
    {
        for (int i = 0; i < AllData.Length; i++)
        {
            Debug.Log(AllData.Length);
        }
    }
}

[System.Serializable]
class TextData
{
    public string QuestionTxt;
    public string[] OptionTxt;
    public string AnswerTxt;
}
