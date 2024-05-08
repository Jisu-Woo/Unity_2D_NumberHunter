using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject optionMenu;
    public GameObject exitCheck;

    public void setting_btn_clicked()
    {
        GameManager.instance.Stop();
        optionMenu.SetActive(true);
    }

    public void back_btn_clicked()
    {
        optionMenu.SetActive(false);
        GameManager.instance.Resume();
    }

    public void back_MainMenu_clicked()
    {
        optionMenu.SetActive(false);
        SceneManager.LoadScene("TitleMainMenu");
    }

    //undefined
    public void Undefined_btn_clicked()
    {
        //later edit
    }

    public void Exit_btn_clicked()
    {
        optionMenu.SetActive(false);
        exitCheck.SetActive(true);     
    }

    public void Exit_Yes_btn_clicked()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void Exit_No_btn_clicked()
    {
        exitCheck.SetActive(false);
        optionMenu.SetActive(true);
    }

}
