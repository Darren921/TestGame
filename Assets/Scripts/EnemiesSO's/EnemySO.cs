using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : ScriptableObject
{
    enum EArmor
    {
        None,
        Light,
        Medium,
        Heavy
    }
    [field: SerializeField] public float eHealth { get; private set; }
    [field: SerializeField] public int eMorale { get; private set; }
    [field: SerializeField, Range(0, 10)] public int eShootingSkill { get; private set; }
    [field: SerializeField] public float eArmorValue { get; private set; }

}
