using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Category
{
    public string CatName;
    public Question[] AllQuestion;
}
[System.Serializable]
class Question
{
    public string QuestionName;
    public Option[] AllOption;
    public string Answer;
}
[System.Serializable]
class Option
{
    public string OptionsName;
}



