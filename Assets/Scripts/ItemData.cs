using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ŀ���� �޴��� �����ϴ� �Ӽ�
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable object/ItemData")]

public class ItemData : ScriptableObject
{
    //�������� �������� enum���� ����
    public enum ItemType { Rotate, Range, Lightning, Floor, Melee, Exp, Hp }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemID;
    public string itemName;

    //�ν����Ϳ� �ؽ�Ʈ�� ������ ���� �� �ֵ��� ����
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    //�⺻ ����
    public float baseDamgae;
    public float baseUpgrade;

    //���׷��̵� ����
    public float[] damages;
    public float[] upgrade;

    [Header("# Weapon")]
    //����ü ������Ʈ
    public GameObject projectile;
}
