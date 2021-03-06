﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    private PlayerController pController;
    [SerializeField] private Animator animControlDeath, animControlGhostDeath, animControlTutorial;
    private Finish finish;
    private bool inMainMenu;
    private GhostController gController;
    private Tutorial tutorial;
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            inMainMenu = true;
        }

        if (inMainMenu)
        {
            return;
        }
        pController = FindObjectOfType<PlayerController>();
        finish = FindObjectOfType<Finish>();
        gController = FindObjectOfType<GhostController>();
        tutorial = FindObjectOfType<Tutorial>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
    }

    void OnEnable()
    {
        if (inMainMenu)
        {
            return;
        }
        gController.PlayerTurnedToGhost += GController_PlayerTurnedToGhost;
        pController.PlayerDied += PController_PlayerDied;
        finish.LevelCompleted += Finish_LevelCompleted;
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            tutorial.TutorialClosed += Tutorial_TutorialClosed;
        }
        

        animControlGhostDeath.gameObject.SetActive(false);
        animControlDeath.gameObject.SetActive(false);
    } 

    void OnDestroy()
    {
        if (inMainMenu)
        {
            return;
        }
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            tutorial.TutorialClosed -= Tutorial_TutorialClosed;
        }
        gController.PlayerTurnedToGhost -= GController_PlayerTurnedToGhost;
        pController.PlayerDied -= PController_PlayerDied;
        finish.LevelCompleted -= Finish_LevelCompleted;
    }

    public void UnloadGhostUI()
    {
        FreezeTime.i.TimeUnfreezeRequest();
        animControlGhostDeath.gameObject.SetActive(false);
        animControlGhostDeath.SetTrigger("GhostAlive");
    }

    private void Tutorial_TutorialClosed()
    {
        tutorial.gameObject.SetActive(false);
        animControlGhostDeath.SetTrigger("TutorialClosed");
    }

    private void Finish_LevelCompleted()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    private void PController_PlayerDied()
    {
        animControlDeath.gameObject.SetActive(true);
        animControlDeath.SetTrigger("PlayerDied");
        pController.PlayerDied -= PController_PlayerDied;
    }

    private void GController_PlayerTurnedToGhost()
    {
        FreezeTime.i.TimeFreezeRequest();
        animControlGhostDeath.gameObject.SetActive(true);
        animControlGhostDeath.SetTrigger("GhostDied");
    }

    public void LoadScene(string LevelName)
    {
        SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
