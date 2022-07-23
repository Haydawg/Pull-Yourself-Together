using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject winLoosePanel;
    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    TextMeshProUGUI winLooseText;

    [SerializeField]
    Collider2D endCollider;
    [SerializeField]
    float deathYpos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            pausePanel.SetActive(pausePanel.activeSelf? false : true);
        }
        if (pausePanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;

        if (winLoosePanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;

        if (PlayerCharacter.instance.headSegment.transform.position.y < deathYpos)
        {
            winLoosePanel.SetActive(true);
            winLooseText.text = "You Loose Try again";
        }

        if(endCollider.IsTouching(PlayerCharacter.instance.headSegment.segmentCollider))
        {
            winLoosePanel.SetActive(true);
            winLooseText.text = "Congrats You Win";
        }
    }



    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
