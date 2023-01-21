using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitTypes
{
    ProximityUnit, // ���� ����
    LongrangeUnit // ���Ÿ� ����
}

[CreateAssetMenu(fileName = "UnitInfo", menuName = "Unity/Create UnitInfo")]
public class UnitInfo : MonoBehaviour
{
    public UnitTypes unit;
    public int hp; // ü��
    public int atk; // ���ݷ�
    public int MovePower; // �̵���
    public int atkRange; // ���� ����
    public int prodCost; // ���� ���
}
