using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    //World Controller
    private GameObject WorldController;
    private PauseScript pause;

    //Interactable Variables
    [SerializeField] private string dialogueText;
    [SerializeField] private string interactText;
    [SerializeField] private GameObject InteractTextPrefab;
    [SerializeField] private float heightOffset;

    //Dialogue Box
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Text dialogueBoxText;

    private GameObject interactTextObject;
    private bool interactTextVisible = false;
    private bool playerInRange = false;

    private void Start()
    {
        WorldController = GameObject.FindGameObjectWithTag("GameController");
        pause = WorldController.GetComponent<PauseScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !interactTextVisible)
        {
            playerInRange = true;
            ShowInteractionText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;
            DestroyInteractionText();
        }
    }

    private void Update()
    {
        float VDirection = Input.GetAxis("Vertical");
        bool ConfirmInput = Input.GetButton("Jump");

        if (VDirection > 0.1 && playerInRange && !dialogueBox.activeInHierarchy)
        {
            //If pressing Up
            dialogueBox.SetActive(true);
            dialogueBoxText.text = dialogueText;
            PauseGame();
        }
        else if (dialogueBox.activeInHierarchy && ConfirmInput)
        {
            //If confirm button is pressed
            dialogueBox.SetActive(false);
            UnpauseGame();
        }

    }

    private void ShowInteractionText()
    {
        Vector3 textPosition = new Vector3(transform.position.x, transform.position.y + heightOffset, -1);
        interactTextObject = Instantiate(InteractTextPrefab, textPosition, Quaternion.identity, transform);

        //Get TextMesh
        TextMesh textmesh = interactTextObject.GetComponent<TextMesh>();
        textmesh.text = interactText;

        interactTextVisible = true;
    }

    private void DestroyInteractionText()
    {
        Destroy(interactTextObject);
        interactTextVisible = false;
    }

    private void PauseGame()
    {
        pause.PauseGame();
    }

    private void UnpauseGame()
    {
        pause.UnpauseGame();
    }
}
