using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenu;
    public GameObject InGameUI;
    void Pause()
        {
            PauseMenu.SetActive(true);
            InGameUI.SetActive(false);
            Time.timeScale = 0f;
            IsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    public void Resume()
        {
            PauseMenu.SetActive(false);
            InGameUI.SetActive(true);
            Time.timeScale = 1f;
            IsPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    
    public void ReportABug()
        {
            Application.OpenURL("https://github.com/popitochka/CyberPunkCollapseGame/issues");
        }
    public void QuitGame()
        {
            Application.Quit();
        }
    //public void ToMenu()
    //    {
    //        SceneManager.LoadScene(0);
    //    }
    void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (IsPaused)
                        {
                            Resume();
                        }
                    else
                        {
                            Pause();
                        }
                }
        }
}