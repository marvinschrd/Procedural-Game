using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelGame : MonoBehaviour
{
    [SerializeField] GameObject panelPause;

    bool Pause = true;

    void Start()
    {
        panelPause.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) & Pause)
        {
            Pause = false;
            panelPause.SetActive(false);
        }

        else if (Input.GetKeyDown(KeyCode.Escape) & !Pause)
        {
            Pause = true;
            panelPause.SetActive(true);
        }
    }

    public void ChangePanel(GameObject panel)
    {
        panelPause.SetActive(false);
    }
}
