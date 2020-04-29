using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class InteractableTest : MonoBehaviour
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
    public IEnumerator InteractableShowsTooltip()
    {
        //Setup
        GameObject sign = GameObject.Find("Sign");
        Vector3 signPosition = GetPosition(sign);

        //Teleport Player to the the sign
        Teleport(player, signPosition.x, signPosition.y);
        yield return new WaitForSeconds(0.1f);

        //Assign interactText object to a variable
        GameObject interactText = GameObject.Find("InteractText(Clone)");

        //ASSERT that variable is not null.
        Assert.NotNull(interactText);
    }

    [UnityTest, Order(2)]
    public IEnumerator InteractableCanBeActivated()
    {
        //Setup
        playerInputs = player.GetComponent<InputsController>();
        GameObject sign = GameObject.Find("Sign");
        Vector3 signPosition = GetPosition(sign);

        //Teleport Player to the the sign
        Teleport(player, signPosition.x, signPosition.y);
        yield return new WaitForSeconds(0.1f);

        playerInputs.VDirection = 1f;
        yield return new WaitForSeconds(0.1f);

        //Assign DialogueBox object to a variable
        GameObject dialogueBox = GameObject.Find("DialogueBox");

        //ASSERT that variable is not null.
        Assert.IsTrue(dialogueBox.activeInHierarchy);
    }

    //Helper Modules
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
