using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject winLoosePanel;
    GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pausePanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }
}
