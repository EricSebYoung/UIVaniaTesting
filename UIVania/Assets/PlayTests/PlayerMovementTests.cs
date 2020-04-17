using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace PlayerTests
{
    public class PlayerMovementTests
    {
        //PlayTest Tests
        private string testSceneName;
        private string UIControllerName;
        private GameObject camera;
        private GameObject player;
        private InputsController playerInputs;

        [SetUp]
        public void Setup()
        {
            testSceneName = "SampleScene";
            UIControllerName = "UIController";

            player = GameObject.FindGameObjectWithTag("Player");
            camera = GameObject.Find("CameraController");
        }

        [UnityTest, Order(0)]
        public IEnumerator TitleStartup()
        {
            SceneManager.LoadScene(testSceneName);
            yield return new WaitForSeconds(1f);

            GameObject UIController = GameObject.Find(UIControllerName);
            MenuController menuControl = UIController.GetComponent<MenuController>();
            menuControl.MouseClick("play");


            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest, Order(1)]
        public IEnumerator JumpOnGround()
        {
            //Setup
            playerInputs = player.GetComponent<InputsController>();

            //Teleport Player to the Ground
            Teleport(player, -3.7f, -0.95168f);
            yield return new WaitForSeconds(0.1f);

            //Get Player's initial position
            Vector3 initialPosition = GetPosition(player);

            //Jump Trigger
            playerInputs.jump = true;

            //Wait and Get New Position
            yield return new WaitForSeconds(0.5f);
            Vector3 newPosition = GetPosition(player);

            //Assert that initial height is lower than new height.
            Assert.That(initialPosition.y, Is.LessThan(newPosition.y));
        }

        [UnityTest, Order(2)]
        public IEnumerator DoesNotJumpInAir()
        {
            //Setup
            playerInputs = player.GetComponent<InputsController>();

            //Teleport Player to the Air
            Teleport(player, -3.7f, 3f);
            yield return new WaitForSeconds(0.1f);

            //Get Player's initial position
            Vector3 initialPosition = GetPosition(player);

            //Jump Trigger
            playerInputs.jump = true;

            //Wait and Get New Position
            yield return new WaitForSeconds(0.5f);
            Vector3 newPosition = GetPosition(player);

            //Assert that initial height is higher than new height.
            Assert.That(initialPosition.y, Is.GreaterThan(newPosition.y));
        }

        [UnityTest, Order(3)]
        public IEnumerator DoesNotJumpAgainstWall()
        {
            //Setup
            playerInputs = player.GetComponent<InputsController>();

            //Teleport Player to the Air Against A Wall
            Teleport(player, -12.51161f, -3f);
            yield return new WaitForSeconds(0.1f);

            //Get Player's initial position
            Vector3 initialPosition = GetPosition(player);

            //Jump Trigger
            playerInputs.HDirection = 1f;
            playerInputs.jump = true;

            //Wait and Get New Position
            yield return new WaitForSeconds(0.5f);
            Vector3 newPosition = GetPosition(player);

            //Assert that initial height is higher than new height.
            Assert.That(initialPosition.y, Is.GreaterThan(newPosition.y));
        }

        private void Teleport(GameObject gameObject, float x, float y)
        {
            camera.SetActive(false);
            gameObject.transform.position = new Vector3(x, y);
            camera.SetActive(true);
        }

        private Vector3 GetPosition(GameObject gameObject)
        {
            Vector3 objPosition = gameObject.transform.position;
            return objPosition;
        }
    }
}