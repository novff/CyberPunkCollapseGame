using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{
    public static bool IsPaused = false;
    public bool InOptions = false;
    public static bool InInventory = false;
    public GameObject PauseMenu;
    public GameObject InGameUI;
    public GameObject Inventory;
    public GameObject Options;

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
    public void OptionsMenu()
        {
            InOptions = true;
            PauseMenu.SetActive(false);
            Options.SetActive(true);
        }
    public void ReportABug()
        {
            Application.OpenURL("https://github.com/popitochka/CyberPunkCollapseGame/issues");
        }
    public void QuitGame()
        {
            Application.Quit();
        }
    public void InventoryOpen()
        {
            Inventory.SetActive(true);
            InGameUI.SetActive(false);
            Time.timeScale = 0f;
            InInventory = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
        }
    public void InventoryClose()
        {
            Inventory.SetActive(false);
            InGameUI.SetActive(true);
            Time.timeScale = 1f;
            InInventory = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
        }


    public void CheckState()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                { 
                    if(!InOptions)
                        {
                            if(!InInventory)
                                {
                                    if(IsPaused) Resume();
                                    else Pause();
                                }
                            else InventoryClose();
                            
                        }
                    else 
                        {
                            InOptions = false;
                            PauseMenu.SetActive(true);
                            Options.SetActive(false);  
                        }
                    
                }
            if (Input.GetKeyDown(KeyCode.Tab)) 
                {
                    if(!IsPaused)
                        {
                            if(InInventory) InventoryClose();
                            else InventoryOpen();
                        }
                }
        }

    
    //public void ToMenu()
    //    {
    //        SceneManager.LoadScene(0);
    //    }
    void Update()
        {
            CheckState();
        }
}