using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameObject HowToPlayPanel;
    private bool isActive;
    public IntScriptable deathTotalCounter, coinTotalCounter;

    private void Start()
    {
        deathTotalCounter.Value = 0;
        coinTotalCounter.Value = 0;
        Time.timeScale = 1;
    }
    private void Update()
    {
        if(HowToPlayPanel !=null)
        {
            if (isActive)
            {
                HowToPlayPanel.SetActive(true);
            }
            else
            {
                HowToPlayPanel.SetActive(false);
            }
        }
    }
    public void ActiveInstructionPanel()
    {
        isActive = !isActive;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void Quit()
    {
        Debug.Log("Quit the game !");
        Application.Quit();
    }
}
