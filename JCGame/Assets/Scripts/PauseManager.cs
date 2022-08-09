using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject saveButtonGO, loadButtonGO;
    public Button saveButton, loadButton;
    public bool paused;

    private void Awake()
    {
        pausePanel = GameObject.Find("PauseMenu");

        saveButtonGO = GameObject.Find("SaveButton");
        loadButtonGO = GameObject.Find("LoadButton");

        saveButton = saveButtonGO.GetComponent<Button>();
        loadButton = loadButtonGO.GetComponent<Button>();

        saveButton.onClick.AddListener(OnSaveClick);
        loadButton.onClick.AddListener(OnLoadClick);

        pausePanel.SetActive(false);
        paused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escape pressed");
            if (!paused)
            {
                Debug.Log("pause");
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().canMove = false;
                pausePanel.SetActive(true);
                paused = true;
            }
            else
            {
                Debug.Log("unpause");
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().canMove = true;
                pausePanel.SetActive(false);
                paused = false;
            }
        }
    }

    public void OnSaveClick()
    {

    }

    public void OnLoadClick()
    {

    }
}
