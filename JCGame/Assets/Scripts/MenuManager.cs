using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    #region FIELDS
    #region PANEL FIELDS
    private GameObject _titleScreen, _optionsScreen;
    #endregion
    #region TITLE SCREEN BUTTON FIELDS
    private GameObject _playButtonGO, _optionsButtonGO, _quitButtonGO;
    private Button _playButton, _optionsButton, _quitButton;
    #endregion
    #region OPTION SCREEN BUTTON FIELDS
    private GameObject _optionsBackGO;
    private Button _optionsBackButton;
    #endregion
    #region LEVEL LOADER FIELD
    private LevelLoader _levelLoader;
    #endregion
    #endregion

    #region AWAKE INIT
    private void Awake()
    {
        #region PANELS INIT
        _titleScreen = GameObject.Find("TitleScreen");
        _optionsScreen = GameObject.Find("OptionsScreen");
        _titleScreen.SetActive(true);
        _optionsScreen.SetActive(true);
        #endregion
        #region TITLE SCREEN BUTTONS INIT
        _playButtonGO = GameObject.Find("PlayButton");
        _optionsButtonGO = GameObject.Find("OptionsButton");
        _quitButtonGO = GameObject.Find("QuitButton");

        _playButton = _playButtonGO.GetComponent<Button>();
        _optionsButton = _optionsButtonGO.GetComponent<Button>();
        _quitButton = _quitButtonGO.GetComponent<Button>();

        _playButton.onClick.AddListener(OnClickPlay);
        _optionsButton.onClick.AddListener(OnClickOptions);
        _quitButton.onClick.AddListener(OnClickQuit);
        #endregion
        #region OPTION SCREEN BUTTONS INIT
        _optionsBackGO = GameObject.Find("OptionsBackButton");

        _optionsBackButton = _optionsBackGO.GetComponent<Button>();

        _optionsBackButton.onClick.AddListener(OnClickOptionsBack);
        #endregion
        #region LEVEL LOADER INIT
        _levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        #endregion
        #region ACTIVE PANEL INIT
        _titleScreen.SetActive(true);
        _optionsScreen.SetActive(false);
        #endregion
    }
    #endregion

    #region ONCLICK METHODS
    void OnClickPlay()
    {
        Debug.Log("Play clicked");
        _levelLoader.LoadLevel("HomeCampScene");
    }

    void OnClickOptions()
    {
        Debug.Log("Options clicked");
        _titleScreen.SetActive(false);
        _optionsScreen.SetActive(true);
    }

    void OnClickQuit()
    {
        Debug.Log("Quit clicked");
        Application.Quit();
    }

    void OnClickOptionsBack()
    {
        Debug.Log("Back in options clicked");
        _titleScreen.SetActive(true);
        _optionsScreen.SetActive(false);
    }
    #endregion
}
