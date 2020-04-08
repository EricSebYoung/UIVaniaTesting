using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int amountDamage = 2;

    public int DoDamage()
    {
        return amountDamage;
    }
}
