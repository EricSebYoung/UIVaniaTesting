using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace PlayerTests
{
    public class AbilitySystemTests
    {
        //PlayTest Tests
        private string testSceneName;
        private string UIControllerName;
        private GameObject camera;
        private GameObject player;
        private InputsController playerInputs;
        private int initialOrbs;
        private int orbsRemaining;
        private string equippedAbility;

        [SetUp]
        public void Setup()
        {
            testSceneName = "SampleScene";
            UIControllerName = "UIController";

            player = GameObject.FindGameObjectWithTag("Player");
            camera = GameObject.Find("CameraController");

            if (string.IsNullOrEmpty(equippedAbility))
            {
   
                equippedAbility = "Dagger";
            }
        }

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
        public IEnumerator HasOrbCost()
        {
            playerInputs = player.GetComponent<InputsController>();
            //ACTIONS
            initialOrbs = GetOrbsRemaining();

            playerInputs.fireAbility = true;

            //Wait for event
            yield return new WaitForSeconds(0.1f);

            orbsRemaining = GetOrbsRemaining();

            //ASSERT
            Assert.Less(orbsRemaining, initialOrbs);
        }
        
        [UnityTest, Order(3)]
        public IEnumerator AbilityFired()
        {
            //ACTIONS
            //Wait for event
            yield return new WaitForSeconds(0.3f);
            GameObject abilityObj = GameObject.Find(equippedAbility+"Throw(Clone)");

            //ASSERT
            Assert.NotNull(abilityObj);
        }

        [UnityTest, Order(4)]
        public IEnumerator MultipleOrbsTaken()
        {
            //ACTIONS
            initialOrbs = orbsRemaining;

            AbilitiesController abilitiesController = player.GetComponent<AbilitiesController>();
            ChangeEquip("Axe");
            abilitiesController.ChangeEquip(equippedAbility);
            //Wait for event
            yield return new WaitForSeconds(0.1f);
            playerInputs.fireAbility = true;
            
            //Wait for event
            yield return new WaitForSeconds(3f);
            orbsRemaining = GetOrbsRemaining();

            //ASSERT
            Assert.Equals(initialOrbs - 3, orbsRemaining);
        }

        [UnityTest, Order(5)]
        public IEnumerator AbilityDidNotFire()
        {
            //ACTIONS

            OrbSystem orbSystem = OrbVisual.playerOrbsStatic;
            orbSystem.Use(99);
            orbSystem.Recover(1);
            ChangeEquip("Axe");

            playerInputs.fireAbility = true;

            //Wait for event
            yield return new WaitForSeconds(0.3f);
            GameObject abilityObj = GameObject.Find(equippedAbility + "Throw(Clone)");

            //ASSERT
            Assert.Null(abilityObj);
        }

        [UnityTest, Order(6)]
        public IEnumerator NoOrbsLost()
        {
            //ACTIONS

            OrbSystem orbSystem = OrbVisual.playerOrbsStatic;
            orbSystem.Use(99);
            orbSystem.Recover(1);
            ChangeEquip("Axe");

            playerInputs.fireAbility = true;

            //Wait for event
            yield return new WaitForSeconds(0.5f);
            orbsRemaining = GetOrbsRemaining();
            GameObject abilityObj = GameObject.Find(equippedAbility + "Throw(Clone)");

            //ASSERT
            Assert.That(orbsRemaining, Is.EqualTo(1));
        }

        private int GetOrbsRemaining()
        {
            OrbSystem orbSystem = OrbVisual.playerOrbsStatic;
            List<OrbSystem.Orb> orbList = orbSystem.GetOrbList();

            int totalFractions = 0;

            for (int i = 0; i < orbList.Count; i++)
            {
                OrbSystem.Orb orb = orbList[i];
                totalFractions += orb.GetFractionsAmount();
            }

            return totalFractions;
        }

        private void ChangeEquip(string equip)
        {
            AbilitiesController abilitiesController = player.GetComponent<AbilitiesController>();
            equippedAbility = equip;
            abilitiesController.ChangeEquip(equippedAbility);
        }
    }
}
