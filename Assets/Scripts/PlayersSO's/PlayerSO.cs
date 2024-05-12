using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Scriptable Objects/PlayerStats", order = 1)]

public class PlayerSO : ScriptableObject
{
   public enum PArmor
    {
        None,
        Light,
        Medium,
        Heavy

    }

    [field: SerializeField] public float pHealth { get; private set; }
    [field: SerializeField] public int pMorale { get; private set; }
    [field: SerializeField, Range(0, 10)] public int pShootingSkill { get; private set; }
    [field: SerializeField] public PArmor  pArmor { get; private set; }
    [field: SerializeField] public float pArmorValue {  get; private set; }



}

