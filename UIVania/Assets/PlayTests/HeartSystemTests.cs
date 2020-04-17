using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace PlayerTests
{
    public class HeartSystemTests
    {
        //PlayTest Tests
        private string testSceneName;
        private string UIControllerName;
        private GameObject camera;
        private GameObject player;
        private GameObject spikes;
        private int initialHearts;
        private int heartsRemaining;

        [SetUp]
        public void Setup()
        {
            testSceneName = "SampleScene";
            UIControllerName = "UIController";

            spikes = GameObject.Find("SpikeTrap");
            player = GameObject.FindGameObjectWithTag("Player");
            camera = GameObject.Find("CameraController");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest, Order(1)]
        public IEnumerator TitleStartup()
        {
            SceneManager.LoadScene(testSceneName);
            yield return new WaitForSeconds(2);

            GameObject UIController = GameObject.Find(UIControllerName);
            MenuController menuControl = UIController.GetComponent<MenuController>();
            menuControl.MouseClick("play");


            yield return new WaitForSeconds(2);
        }
    
        [UnityTest, Order(3)]
        public IEnumerator HeartDamageIsTaken()
        {
            initialHearts = GetHeartsRemaining();

            float xTeleport = spikes.transform.position.x;
            float yTeleport = spikes.transform.position.y + 2;

            camera.SetActive(false);
            player.transform.position = new Vector3(xTeleport, yTeleport);
            camera.SetActive(true);

            //Wait for event to occur
            yield return new WaitForSeconds(2);

            heartsRemaining = GetHeartsRemaining();

            //ASSERT
            Assert.Greater(initialHearts, heartsRemaining);
        }

        [UnityTest, Order(4)]
        public IEnumerator HeartDamageEqualsSpikeDamage()
        {
            //Wait for event to occur
            yield return new WaitForSeconds(0.1f);

            int spikesDamage = spikes.GetComponent<Damage>().DoDamage();
            Assert.AreEqual(initialHearts - spikesDamage, heartsRemaining);
        }

        [UnityTest, Order(2)]
        public IEnumerator PlayerSurvivesHeartDamage()
        {
            HeartsVisual.playerHeartsStatic.Damage(4);
            //Wait for event to occur
            yield return new WaitForSeconds(0.5f);

            //ASSERT
            Assert.NotNull(player);
        }

        [UnityTest, Order(5)]
        public IEnumerator PlayerObjectDestroyedOnEmpty()
        {
            //Deal Damage
            HeartsVisual.playerHeartsStatic.Damage(99);

            //Player Destruction on damage is attached to damage animation thus we need to mimic
            //the player actually hitting a damaging object by teleporting them to a spike object.
            float xTeleport = spikes.transform.position.x;
            float yTeleport = spikes.transform.position.y + 2;
            camera.SetActive(false);
            player.transform.position = new Vector3(xTeleport, yTeleport);
            camera.SetActive(true);

            //Wait for event to occur
            yield return new WaitForSeconds(2f);
            player = GameObject.FindGameObjectWithTag("Player");

            //ASSERT
            Assert.Null(player);
        }



        private int GetHeartsRemaining()
        {
            HeartsSystem heartsSystem = HeartsVisual.playerHeartsStatic;
            List<HeartsSystem.Heart> heartList = heartsSystem.GetHeartList();

            int totalFractions = 0;

            for (int i = 0; i < heartList.Count; i++)
            {
                HeartsSystem.Heart heart = heartList[i];
                totalFractions += heart.GetFractionsAmount();
            }

            return totalFractions;
        }
    }
}
