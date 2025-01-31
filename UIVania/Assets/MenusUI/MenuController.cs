﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    #region Default Values
    private int menuNumber;
    private bool joystickToggle = true;
    #endregion

    #region Menu Dialogs
    [Header("Main Menu Components")]
    [SerializeField] private GameObject titleScreenCanvas;
    [SerializeField] private GameObject playScreenCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject menuSettingsCanvas;
    [SerializeField] private GameObject creditsScreenCanvas;
    [Space(10)]
    [Header("Game Components")]
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject joystickCanvas;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        menuNumber = -1;
        hidePlayScreenUI();
        playerObj.SetActive(false);
        joystickCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);
        menuSettingsCanvas.SetActive(false);
        creditsScreenCanvas.SetActive(false);
        titleScreenCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Menu Mouse Clicks
    public void MouseClick(string buttonType)
    {
        #region Title Screen
        if (buttonType == "play")
        {
            showPlayScreenUI();
            playerObj.SetActive(true);

            titleScreenCanvas.SetActive(false);
            menuNumber = 0;
        }
        #endregion

        #region Menus Open/Close
        if (buttonType == "OpenMainMenu")
        {
            hidePlayScreenUI();

            mainMenuCanvas.SetActive(true);
            menuNumber = 1;
        }

        if(buttonType == "CloseMainMenu")
        {
            showPlayScreenUI();

            mainMenuCanvas.SetActive(false);
            menuNumber = 0;
        }

        if (buttonType == "OpenSettingsMenu")
        {
            menuSettingsCanvas.SetActive(true);
            mainMenuCanvas.SetActive(false);
            menuNumber = 2;
        }

        if (buttonType == "CloseSettingsMenu")
        {
            menuSettingsCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            menuNumber = 1;
        }

        if (buttonType == "OpenCreditsScreen")
        {
            creditsScreenCanvas.SetActive(true);
            mainMenuCanvas.SetActive(false);
            menuNumber = 3;
        }

        if (buttonType == "CloseCreditsScreen")
        {
            creditsScreenCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            menuNumber = 1;
        }
        #endregion

        #region Screen Selection options
        if (buttonType == "Status")
        {
            //todo
        }

        if (buttonType == "Abilities")
        {
            //todo
        }

        if (buttonType == "MapView")
        {
            //todo
        }

        if (buttonType == "StatusScreen")
        {
            //todo
        }
        #endregion

        #region Save/Load options
        if (buttonType == "Save")
        {
            //todo
        }

        if (buttonType == "Load")
        {
            //todo
        }
        #endregion

    }
    #endregion

    #region show/hide PlayscreenUI
    private void showPlayScreenUI()
    {
        playScreenCanvas.SetActive(true);
        menuNumber = 0;
        
        if(joystickToggle == true)
        {
            joystickCanvas.SetActive(true);
        } else if(joystickToggle == false)
        {
            joystickCanvas.SetActive(false);
        }
    }
    private void hidePlayScreenUI()
    {
        playScreenCanvas.SetActive(false);
        joystickCanvas.SetActive(false);
    }
    #endregion

    public void toggleJoysticks()
    {
        if (joystickToggle == true)
        {
            joystickToggle = false;
        }
        else if (joystickToggle == false)
        {
            joystickToggle = true;
        }
    }
}
