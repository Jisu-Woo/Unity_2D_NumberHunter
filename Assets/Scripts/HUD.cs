using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    //ui�� �ٷ�� �� �����͸� ������(enum)���� �����ϰ� ����
    public enum InfoType { Exp, Level, Kill, Time, Hp }
    public InfoType type;

    //text, slider ���� ����
    Text myText;
    Slider mySlider;

    //���� �ʱ�ȭ
    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type) 
        {
            //exp ������ ������ �����̴� ui�� ����
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;
            //���� ������ ������ string���� ��ȯ�� text�� ���
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                //���� �ð� ���ϱ�, �Ҽ����� ������
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
