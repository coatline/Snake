using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallModeToggle : MonoBehaviour {

    Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        if (GameHandler.wallMode)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

}
