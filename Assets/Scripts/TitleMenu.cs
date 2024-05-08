using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void start_btn_clicked()
    {
        GameManager.instance.gameStart = true;
        GameManager.instance.Resume();
        SceneManager.LoadScene("Main");      
    }

}
