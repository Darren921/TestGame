using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats",menuName = "Scriptable Objects/PlayerStats",order = 1)]

public class PlayerSO : ScriptableObject
{
    [field:SerializeField] public int pHealth {  get; private set; }
    [field: SerializeField] public int pMorale { get; private set; }
    [field: SerializeField, Range(0,10)] public int pShootingSkill { get; private set; }

}

