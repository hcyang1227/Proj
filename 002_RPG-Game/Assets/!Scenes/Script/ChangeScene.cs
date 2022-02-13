using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;


public class ChangeScene : MonoBehaviour
{
public GameObject LoadGameFlag;
//舊的讀檔寫法：
//Application.LoadLevel("MainGameScene");

    void Start()
    {
        DontDestroyOnLoad(LoadGameFlag);
        LoadGameFlag.name = "沒讀檔";
    }
    public void BackToTitle()
    {
        EditorSceneManager.LoadScene("TitleScene");
        LoadGameFlag.name = "沒讀檔";
    }
    public void LoadDataToStage()
    {
        LoadGameFlag.name = "有讀檔";
        EditorSceneManager.LoadScene("MainGameScene");
    }
    public void LoadStageStart()
    {
        EditorSceneManager.LoadScene("MainGameScene");
        LoadGameFlag.name = "沒讀檔";
    }

}