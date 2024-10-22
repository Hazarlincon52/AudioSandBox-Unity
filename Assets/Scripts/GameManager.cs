using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerMovement PM;

    [Header("GameObjects")]
    public GameObject title;
    public GameObject menuPanel;
    public GameObject optionPanel;
    public GameObject creditPanel;
    public GameObject pausePanel;
    public GameObject controllerPanel;
    public GameObject crosshar;
   
    [Header("Variabels")]
    public Animator anim;

    private void Start()
    {
        anim.SetTrigger("StartMenu");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pausePanel.activeSelf == false)
        {
            PauseGame();
            PM.PauseMove();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pausePanel.activeSelf == true)
        {
            ResumeGame();
            PM.PauseMove();
        }

    }
    
    //menu panel
    public void STARTAnim()
    {
        Invoke(nameof(deactivateMenuandTitle_Time), 0.7f);
    }
    public void OpenAudionPanel()
    {
        menuPanel.SetActive(false);
        optionPanel.SetActive(true);
    }
    public void CloseAudionPanel()
    {
        menuPanel.SetActive(true);
        optionPanel.SetActive(false);
    }
    public void OpenCreditPanel()
    {
        title.SetActive(false);
        menuPanel.SetActive(false);
        creditPanel.SetActive(true);
    }
    public void CloseCreditPanel()
    {
        title.SetActive(true);
        menuPanel.SetActive(true);
        creditPanel.SetActive(false);
    }

    //Pause Game
    public void PauseGame()
    {
        title.SetActive(false);
        menuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausePanel.SetActive(true);
        controllerPanel.SetActive(true);
        crosshar.SetActive(false);
        anim.SetTrigger("PausePanel_Open");
    }
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        anim.SetTrigger("PausePanel_Close");
        crosshar.SetActive(true);
        Invoke(nameof(enablePause_Time),0.5f);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void enablePause_Time()
    {
        pausePanel.SetActive(false);
        controllerPanel.SetActive(false);
        menuPanel.SetActive(false);
    }

    public void deactivateMenuandTitle_Time()
    {
        menuPanel.SetActive(false);
        title.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
   

}
