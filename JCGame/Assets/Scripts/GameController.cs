using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private CharacterController playerController;
    public PlayerAbilities playerAbilities;
    public CharacterDataSO enemyCharData;
    public CharacterDataSO playerCharData;
    [SerializeField] private CharacterDataSO enemyToSpawn;
    [SerializeField] private MapGenerator mapGenerator;

    public int currSeed;
    public int currFloor;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        mapGenerator = GameObject.Find("MapManager").GetComponent<MapGenerator>();
        playerAbilities = gameObject.GetComponent<PlayerAbilities>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        currFloor = 1;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "TestScene" && scene.name != "FightScene" && scene.name != "HomeCampScene")
        {

        }
        else if(scene.name == "TestScene")
        {
            if(!PlayerExists())
            {
                SpawnPlayer();
            }
            else
            {
                playerObject = GameObject.Find("Player(Clone)");
                playerController = playerObject.GetComponent<CharacterController>();
            }
            mapGenerator.gameObject.SetActive(true);
            gameObject.GetComponent<PauseManager>().pausePanel.SetActive(false);
        }
        else if(scene.name == "FightScene")
        {
            if (!PlayerExists())
            {
                
            }
            else
            {
                playerObject = GameObject.Find("Player(Clone)");
                playerController = playerObject.GetComponent<CharacterController>();
            }
            mapGenerator.gameObject.SetActive(false);
            playerAbilities.LogPlayerAbilityCount();
            gameObject.GetComponent<PauseManager>().pausePanel.SetActive(false);
        }
        else if (scene.name == "HomeCampScene")
        {
            if (!PlayerExists())
            {
                Debug.Log("spawning player");
                SpawnPlayer();
            }
            else
            {
                playerObject = GameObject.Find("Player(Clone)");
                playerController = playerObject.GetComponent<CharacterController>();
            }
            mapGenerator.gameObject.SetActive(false);
            gameObject.GetComponent<PauseManager>().pausePanel.SetActive(false);
        }
        Debug.Log(scene.name);
    }

    private void SpawnPlayer()
    {
        Vector3 spawnPosition = new Vector3(playerCharData.position[0], playerCharData.position[1], playerCharData.position[2]);
        playerObject = Instantiate(playerCharData.characterGameObject, spawnPosition, default); playerObject.SetActive(true);
        playerController = playerObject.GetComponent<CharacterController>();
    }

    private bool PlayerExists()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            Debug.Log("player doesnt exist");
            return false;
        }
        else
        {
            Debug.Log("player exists");
            return true;
        }
    }

    public void GetFloor()
    {
        Debug.Log("GetFloor() called");
        Transform mapManager = GameObject.Find("MapManager").transform;

        for (int i = 0; i < mapManager.transform.childCount; i++)
        {
            if (mapManager.transform.GetChild(i).gameObject.activeSelf == true)
            {
                currFloor = i + 1;
                Debug.Log("currFloor = " + currFloor);
            }
        }
    }

    public void ChangeFloor(int direction)
    {
        Transform mapManager = GameObject.Find("MapManager").transform;
        playerController.canMove = false;
        Debug.Log("F" + (currFloor + direction));
        if (mapManager.Find("F" + (currFloor + direction)).childCount == 0)
        {
            mapManager.Find("BOSS").gameObject.SetActive(true);
            playerObject.transform.position = new Vector3(5,1,5);
        }
        else
        {
            mapManager.Find("F" + (currFloor + direction)).gameObject.SetActive(true);
            Vector3 stairsPos;
            if (direction == 1)
            {
                stairsPos = mapManager.Find("F" + (currFloor + direction)).Find("StairsTileMini(Clone)").position;
            }
            else
            {
                stairsPos = mapManager.Find("F" + (currFloor + direction)).Find("StairsTileMini2(Clone)").position;
            }
            Vector3 nextToStairsPos = new Vector3(stairsPos.x + 1.5f, 1, stairsPos.z);
            playerObject.transform.position = nextToStairsPos;
        }
        mapManager.Find("F" + currFloor).gameObject.SetActive(false);
        playerController.canMove = true;
    }

    public void SpawnEnemy()
    {
        GameObject spawnedEnemy = Instantiate(enemyToSpawn.characterGameObject); spawnedEnemy.SetActive(true);
    }
}
