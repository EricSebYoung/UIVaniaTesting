using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesController : MonoBehaviour
{
    private enum Abilities { None, Dagger, Axe };
    private Abilities Equipped;

    [SerializeField] private Transform firePoint;

    [System.Serializable]
    public class abilityObjects
    {
        public GameObject prefab;
    }

    [SerializeField] private abilityObjects[] abilityObject;

    private void Start()
    {
        Equipped = Abilities.Dagger;
    }

    public void FireAbility()
    {
        int abilityNum = (int)Equipped - 1;
        if (Equipped != Abilities.None);
        {
            Instantiate(abilityObject[abilityNum].prefab, firePoint.position, firePoint.rotation);
        }
    }

    public void ChangeEquip(string abilityName)
    {
        try
        {
            Equipped = (Abilities) Enum.Parse(typeof(Abilities), abilityName);
        } catch (ArgumentException)
        {
            Console.WriteLine("Unrecognized Ability Name: {0}", abilityName);
        }
    }

    public void CycleEquip(bool next)
    {
        int eqNum = (int)Equipped;
        int numOfAbilities = abilityObject.Length;

        if (next)
        {
            eqNum++;
            if (eqNum > numOfAbilities)
            {
                eqNum = 1;
            }
        } else
        {
            eqNum--;
            if (eqNum < 1)
            {
                eqNum = numOfAbilities - 1;
            }
        }

        ChangeEquip(Enum.GetName(typeof(Abilities), eqNum));
    }
}
