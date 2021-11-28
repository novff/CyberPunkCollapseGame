using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject Debug;
    public void SetVolume(float volume){}
    public void DebugInfoToggle(bool flag)
        {
            if (flag) Debug.SetActive(true);
            else Debug.SetActive(false);
        }
}
