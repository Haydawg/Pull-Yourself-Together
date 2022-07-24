using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject winLosePanel;
    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    TextMeshProUGUI winLoseText;
    [SerializeField]
    Image ending;
    [SerializeField]
    Sprite[] endingSprites;
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
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(pausePanel.activeSelf? false : true);
        }
        if (pausePanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else if (winLosePanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;

        if (PlayerCharacter.instance.headSegment.transform.position.y < deathYpos)
        {
            winLosePanel.SetActive(true);
            ending.sprite = endingSprites[0];
        }

        if(endCollider.IsTouching(PlayerCharacter.instance.headSegment.segmentCollider))
        {
            winLosePanel.SetActive(true);
            ending.sprite = endingSprites[1];
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
