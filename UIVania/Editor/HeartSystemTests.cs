using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class HeartSystemTests : MonoBehaviour
{
    [Test]
    public void HeartMaximum_Test()
    {
        //SETUP
        int numberOfHearts = 3;
        int maxFractions = 4;
        int expectedMax = numberOfHearts * maxFractions;

        HeartsSystem heartsSystem = new HeartsSystem(numberOfHearts);
        List<HeartsSystem.Heart> heartList = heartsSystem.GetHeartList();

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
}
