using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class HeartSystemTests : MonoBehaviour
{
    //Editor Tests
    private int numberOfHearts;
    private int maxFractions;
    private int expectedMax;
    private HeartsSystem heartsSystem;
    private List<HeartsSystem.Heart> heartList;

    [SetUp]
    public void Setup()
    {
        numberOfHearts = 3;
        maxFractions = 4;
        expectedMax = numberOfHearts * maxFractions;
        heartsSystem = new HeartsSystem(numberOfHearts);
        heartList = heartsSystem.GetHeartList();
    }

    [Test, Order(1)]
    public void HeartMaximum_Test()
    {
        //ACTIONS
        int totalFractions = 0;

        for (int i=0; i < heartList.Count; i++)
        {
            HeartsSystem.Heart heart = heartList[i];
            totalFractions += heart.GetFractionsAmount();
        }

        //ASSERT
        Assert.That(totalFractions, Is.EqualTo(expectedMax));
    }

    [Test, Order(2)]
    public void OnlyExpectedFractions()
    {
        //ACTIONS
        bool expectedMax = true;

        for (int i = 0; i < heartList.Count && expectedMax; i++)
        {
            HeartsSystem.Heart heart = heartList[i];
            if (heart.GetFractionsAmount() != maxFractions)
            {
                expectedMax = false;
            }
        }

        //ASSERT
        Assert.True(expectedMax);
    }

    [Test, Order(3)]
    public void NoOverflow()
    {
        //ACTIONS
        bool noOverflow = true;
        heartsSystem.Heal(maxFractions);

        for (int i = 0; i < heartList.Count && noOverflow; i++)
        {
            HeartsSystem.Heart heart = heartList[i];
            if (heart.GetFractionsAmount() > maxFractions)
            {
                noOverflow = false;
            }
        }

        //ASSERT
        Assert.True(noOverflow);
    }

    [Test, Order(4)]
    public void NoNegatives()
    {
        //ACTIONS
        bool noNegatives = true;
        heartsSystem.Damage(maxFractions + 1);

        for (int i = 0; i < heartList.Count && noNegatives; i++)
        {
            HeartsSystem.Heart heart = heartList[i];
            if (heart.GetFractionsAmount() < 0)
            {
                noNegatives = false;
            }
        }

        //ASSERT
        Assert.True(noNegatives);
    }

    [Test, Order(5)]
    public void LastTakesDamageFirst()
    {
        //make sure health is full before testing
        heartsSystem.Heal(maxFractions * numberOfHearts);

        //ACTIONS
        bool lastDamageFirst = true;
        heartsSystem.Damage(maxFractions - 1);

        for (int i = 0; i < heartList.Count && lastDamageFirst; i++)
        {
            HeartsSystem.Heart heart = heartList[i];
            if (i < (heartList.Count - 1) && heart.GetFractionsAmount() < maxFractions)
            {
                lastDamageFirst = false;
            }
        }

        //ASSERT
        Assert.True(lastDamageFirst);
    }

    [Test, Order(6)]
    public void DamageIsntSplit()
    {
        //make sure health is full before testing
        heartsSystem.Heal(maxFractions * numberOfHearts);

        //ACTIONS
        bool noSplit = true;
        bool valueLessThanMinMaxFound = false;
        heartsSystem.Damage(maxFractions + 1);

        for (int i = 0; i < heartList.Count && noSplit; i++)
        {
            HeartsSystem.Heart heart = heartList[i];
            int fractionsAmount = heart.GetFractionsAmount();
            if (fractionsAmount < maxFractions && fractionsAmount > 0)
            {
                if (!valueLessThanMinMaxFound)
                {
                    valueLessThanMinMaxFound = true;
                } else
                {
                    noSplit = false;
                }
            }
        }

        //ASSERT
        Assert.True(noSplit);
    }
}
