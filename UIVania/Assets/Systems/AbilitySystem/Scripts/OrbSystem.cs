using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSystem
{
    public const int MAX_FRACTION_AMOUNT = 1;

    public event EventHandler OnUsed;
    public event EventHandler OnRecovered;
    public event EventHandler OnEmpty;


    private List<Orb> orbList;

    public OrbSystem(int orbAmount)
    {
        orbList = new List<Orb>();
        for (int i = 0; i < orbAmount; i++)
        {
            Orb orb = new Orb(MAX_FRACTION_AMOUNT);
            orbList.Add(orb);
        }

    }

    public List<Orb> GetOrbList()
    {
        return orbList;
    }

    public void Use(int useAmount)
    {
        //Cycle through orb
        for (int i = orbList.Count - 1; i >= 0; i--)
        {
            Orb orb = orbList[i];

            //Can orb take all use received
            if (useAmount > orb.GetFractionsAmount())
            {
                useAmount -= orb.GetFractionsAmount();
                orb.Use(orb.GetFractionsAmount());
            }
            else
            {
                orb.Use(useAmount);
                break;
            }
        }

        if (OnUsed != null) OnUsed(this, EventArgs.Empty);

        //check if out of ability
        if (IsEmpty())
        {
            if (OnEmpty != null) OnEmpty(this, EventArgs.Empty);
        }
    }

    public void Recover(int recoverAmount)
    {
        for (int i = 0; i < orbList.Count; i++)
        {
            Orb orb = orbList[i];
            int missingFractions = MAX_FRACTION_AMOUNT - orb.GetFractionsAmount();
            if (recoverAmount > missingFractions)
            {
                recoverAmount -= missingFractions;
                orb.Recover(missingFractions);
            }
            else
            {
                orb.Recover(recoverAmount);
                break;
            }
        }

        if (OnRecovered != null) OnRecovered(this, EventArgs.Empty);
    }

    public bool IsEmpty()
    {
        return orbList[0].GetFractionsAmount() == 0;
    }

    public class Orb
    {
        private int fractions;

        public Orb(int fractions)
        {
            this.fractions = fractions;
        }

        public int GetFractionsAmount()
        {
            return fractions;
        }

        public void SetFractionAmount(int fractions)
        {
            this.fractions = fractions;
        }

        public void Use(int useAmount)
        {
            if (useAmount >= fractions)
            {
                fractions = 0;
            }
            else
            {
                fractions = fractions - useAmount;
            }
        }

        public void Recover(int recoverAmount)
        {
            if (fractions + recoverAmount > MAX_FRACTION_AMOUNT)
            {
                fractions = MAX_FRACTION_AMOUNT;
            }
            else
            {
                fractions += recoverAmount;
            }
        }
    }
}