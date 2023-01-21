using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitTypes
{
    ProximityUnit, // 근접 유닛
    LongrangeUnit // 원거리 유닛
}

[CreateAssetMenu(fileName = "UnitInfo", menuName = "Unity/Create UnitInfo")]
public class UnitInfo : MonoBehaviour
{
    public UnitTypes unit;
    public int hp; // 체력
    public int atk; // 공격력
    public int MovePower; // 이동력
    public int atkRange; // 공격 범위
    public int prodCost; // 생산 비용
}
