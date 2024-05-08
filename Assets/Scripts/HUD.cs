using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    //ui로 다루게 될 데이터를 열거형(enum)으로 저장하고 선언
    public enum InfoType { Exp, Level, Kill, Time, Hp }
    public InfoType type;

    //text, slider 변수 선언
    Text myText;
    Slider mySlider;

    //변수 초기화
    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type) 
        {
            //exp 정보를 가져와 슬라이더 ui에 적용
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;
            //레벨 정보를 가져와 string으로 변환해 text로 출력
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                //남은 시간 구하기, 소수점은 버리기
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Hp:
                float curHp = GameManager.instance.hp;
                float maxHp = GameManager.instance.initialHp;
                mySlider.value = curHp / maxHp;
                break;
        }
    }
}
