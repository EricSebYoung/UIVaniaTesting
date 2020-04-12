using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class PlayerTests
    {
        private string testSceneName;
        private string UIControllerName;
        private GameObject camera;
        private GameObject player;
        private GameObject spikes;
        private int heartsRemaining;

        [SetUp]
        public void Setup()
        {
            testSceneName = "TestsScene";
            UIControllerName = "UIController";
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
    
        [UnityTest, Order(2)]
        public IEnumerator SpikeDamageTakenOnceLeft()
        {
            int initialHearts = GetHeartsRemaining();

            spikes = GameObject.Find("SpikeTrap");
            player = GameObject.FindGameObjectWithTag("Player");
            camera = GameObject.Find("CameraController");

            float xTeleport = spikes.transform.position.x - .5f;
            float yTeleport = spikes.transform.position.y + 2;

            camera.SetActive(false);
            player.transform.position = new Vector3(xTeleport, yTeleport);
            camera.SetActive(true);

            //Wait for event to occur
            yield return new WaitForSeconds(2);

            heartsRemaining = GetHeartsRemaining();

            //ASSERT
            Assert.AreEqual(initialHearts-2, heartsRemaining);
        }

        [UnityTest, Order(3)]
        public IEnumerator SpikeDamageTakenOnceRight()
        {
            int initialHearts = GetHeartsRemaining();

            float xTeleport = spikes.transform.position.x + .5f;
            float yTeleport = spikes.transform.position.y + 2;

            camera.SetActive(false);
            player.transform.position = new Vector3(xTeleport, yTeleport);
            camera.SetActive(true);

            //Wait for event to occur
            yield return new WaitForSeconds(2);

            heartsRemaining = GetHeartsRemaining();

            //ASSERT
            Assert.AreEqual(initialHearts - 2, heartsRemaining);
        }

        [UnityTest, Order(4)]
        public IEnumerator SpikeDamageTakenOnceMiddle()
        {
            int initialHearts = GetHeartsRemaining();

            float xTeleport = spikes.transform.position.x;
            float yTeleport = spikes.transform.position.y + 2;

            camera.SetActive(false);
            player.transform.position = new Vector3(xTeleport, yTeleport);
            camera.SetActive(true);

            //Wait for event to occur
            yield return new WaitForSeconds(2);

            heartsRemaining = GetHeartsRemaining();

            //ASSERT
            Assert.AreEqual(initialHearts - 2, heartsRemaining);
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
