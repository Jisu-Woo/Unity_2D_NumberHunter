using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//커스템 메뉴를 생성하는 속성
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable object/ItemData")]

public class ItemData : ScriptableObject
{
    //아이템의 종류들을 enum으로 선언
    public enum ItemType { Rotate, Range, Lightning, Floor, Melee, Exp, Hp }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemID;
    public string itemName;

    //인스펙터에 텍스트를 여러줄 넣을 수 있도록 선언
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    //기본 스택
    public float baseDamgae;
    public float baseUpgrade;

    //업그레이드 스택
    public float[] damages;
    public float[] upgrade;

    [Header("# Weapon")]
    //투사체 오브젝트
    public GameObject projectile;
}
