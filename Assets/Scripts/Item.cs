using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        //index of Text[] follow hierarchy order
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        //text display
        textLevel.text = "Lv. " + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.Rotate:
            case ItemData.ItemType.Melee:           
            case ItemData.ItemType.Lightning:
            case ItemData.ItemType.Floor:         
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.upgrade[level]);
                break;
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.upgrade[level] * 100);
                break;
            case ItemData.ItemType.Exp:          
                textDesc.text = string.Format(data.itemDesc, data.upgrade[level] * 100);
                break;
            case ItemData.ItemType.Hp:
                textDesc.text = string.Format(data.itemDesc, data.upgrade[level]);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
        
    }
    public void OnClick()
    {
        
        switch (data.itemType)
        {
            case ItemData.ItemType.Rotate:
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Lightning:
            case ItemData.ItemType.Floor:
            case ItemData.ItemType.Exp:
            case ItemData.ItemType.Hp:
                //if level 0: weapon register
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                //if level is not 0: weapon upgrade
                else
                {
                    float nextDamage = data.baseDamgae;
                    float nextUpgrade = 0;

                    nextDamage += data.baseDamgae * data.damages[level];
                    nextUpgrade += data.upgrade[level];

                    weapon.LevelUp(nextDamage, nextUpgrade);
                }
                break;

                default: break;
        }

        level++;

        //최대 레벨에 도달하면 버튼 비활성화
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
