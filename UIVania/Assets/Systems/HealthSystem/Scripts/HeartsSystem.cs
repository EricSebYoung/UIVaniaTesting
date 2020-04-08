using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsSystem
{
    public const int MAX_FRACTION_AMOUNT = 4;

    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;


    private List<Heart> heartList;

    public HeartsSystem(int heartAmount)
    {
        heartList = new List<Heart>();
        for (int i = 0; i < heartAmount; i++)
        {
            Heart heart = new Heart(4);
            heartList.Add(heart);
        }

    }

    public List<Heart> GetHeartList()
    {
        return heartList;
    }

    public void Damage(int damageAmount)
    {
        //Cycle through hearts
        for (int i = heartList.Count - 1; i >=0; i--)
        {
            Heart heart = heartList[i];

            //Can heart take all damage received
            if (damageAmount > heart.GetFractionsAmount())
            {
                damageAmount -= heart.GetFractionsAmount();
                heart.Damage(heart.GetFractionsAmount());
            } else
            {
                heart.Damage(damageAmount);
                break;
            }
        }

        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);

        //check if out of hearts
        if (IsDead())
        {
            if (OnDead != null) OnDead(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount)
    {
        for (int i = 0; i < heartList.Count; i++)
        {
            Heart heart = heartList[i];
            int missingFractions = MAX_FRACTION_AMOUNT - heart.GetFractionsAmount();
            if (healAmount > missingFractions)
            {
                healAmount -= missingFractions;
                heart.Heal(missingFractions);
            } else
            {
                heart.Heal(healAmount);
                break;
            }
        }

        if (OnHealed != null) OnHealed(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return heartList[0].GetFractionsAmount() == 0;
    }

    public class Heart
    {
        private int fractions;

        public Heart(int fractions)
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

        public void Damage(int damageAmount)
        {
            if (damageAmount >= fractions)
            {
                fractions = 0;
            } else
            {
                fractions = fractions - damageAmount;
            }
        }

        public void Heal(int healAmount)
        {
            if (fractions + healAmount > MAX_FRACTION_AMOUNT)
            {
                fractions = MAX_FRACTION_AMOUNT;
            } else
            {
                fractions += healAmount;
            }
        }
    }
}
