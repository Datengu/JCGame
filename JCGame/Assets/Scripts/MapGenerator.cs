using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;
    [SerializeField] private GameController gameController;
    #region GAMEOBJECT VARIABLES
    [SerializeField] private GameObject[] tiles;
    private GameObject[,] mapGridSmall;
    [SerializeField] private List<GameObject> floorTiles;
    #endregion
    #region INT VARIABLES
    public int playerLevel, completedDungeonLevel;
    [SerializeField] private int baseQuality, finalQuality, dungeonRank, depth, sMonsterRank, boss, dungeonLevel, dungeonLevelVariation, dungeonLocation, dungeonLocaleRank;
    [SerializeField] private int columns, rows, tileToGenerate, maxTunnels, maxLength, randomLength, initialSeed;
    private readonly (int,int)[] directions = new (int,int)[] {(3, 0), (-3, 0), (0, 3), (0, -3)};
    private readonly int[] directionX = new int[] {1, -1, 0, 0};
    private readonly int[] directionY = new int[] {0, 0, 1, -1};
    private (int, int) lastDirection;
    private int[,] mapGridBig;
    #endregion
    #region STRING VARIABLES
    public string seed;
    private string dungeonLevelVariationSeeding, dungeonName, dungeonNamePrefix, dungeonNameLocation, dungeonNameSuffix;
    #endregion
    #region DUNGEON NAME PREFIX INIT
    private readonly string[] monsterRankOneTwo = new string[] { "Clay", "Rock", "Granite", "Basalt", "Graphite" };
    private readonly string[] monsterRankThreeFour = new string[] { "Basalt", "Graphite", "Iron", "Copper", "Bronze" };
    private readonly string[] monsterRankFiveSix = new string[] { "Copper", "Bronze", "Steel", "Silver", "Gold", "Platinum" };
    private readonly string[] monsterRankSevenEight = new string[] { "Copper", "Bronze", "Steel", "Silver", "Gold", "Platinum", "Ruby", "Emerald", "Sapphire", "Diamond" };
    private readonly string[] monsterRankNine = new string[] { "Platinum", "Ruby", "Emerald", "Sapphire", "Diamond" };
    #endregion
    #region DUNGEON NAME LOCALE INIT
    private readonly string[] localeRankOne = new string[] { "Tunnel", "Cave" };
    private readonly string[] localeRankTwo = new string[] { "Marsh", "Crevasse", "Mine" };
    private readonly string[] localeRankThree = new string[] { "Crater", "Lake", "Icepit", "Lair" };
    private readonly string[] localeRankFour = new string[] { "Dungeon", "Moor", "Snowhall", "Path" };
    private readonly string[] localeRankFive = new string[] { "Crypt"};
    private readonly string[] localeRankSix = new string[] { "Ruins", "Waterway", "Tundra", "Nest" };
    private readonly string[] localeRankSeven = new string[] { "World" };
    private readonly string[] localeRankEight = new string[] { "Abyss", "Maze", "Void", "Chasm", "Glacier" };
    #endregion
    #region DUNGEON NAME SUFFIX INIT
    private readonly string[] bossOneToThree = new string[] { "Joy", "Bliss", "Glee", "Doubt", "Woe", "Dolour" };
    private readonly string[] bossFourToSix = new string[] { "Doubt", "Woe", "Dolour", "Regret", "Bane", "Fear" };
    private readonly string[] bossSevenToNine = new string[] { "Regret", "Bane", "Fear", "Dread", "Hurt", "Gloom" };
    private readonly string[] bossTenToTwelve = new string[] { "Dread", "Hurt", "Gloom", "Doom", "Evil", "Ruin", "Death" };
    #endregion

    public InputField seedInputField;

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
    }

    void Start()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        playerLevel = 1; completedDungeonLevel = 1; //required to calculate quality.
        initialSeed = 0;
        for (int i = 1; i <= 16; i++)
        {
            InstantiateFloorGameObject(i);
        }
        InstantiateBossFloorGameObject();
    }

    public void GenerateDungeonWithSeed(int intSeed)
    {
        playerLevel = 1; completedDungeonLevel = 1; // ???
        ClearLog(); //clears the console log
        ClearFloors();
        #region SEED
        initialSeed = intSeed; Debug.Log("InitialSeed: " + initialSeed);
        Random.InitState(initialSeed);
        seed = initialSeed.ToString();
        Debug.Log("Seed: " + seed);
        Debug.Log("Hex Seed: " + initialSeed.ToString("X4"));
        #endregion
        #region QUALITY
        Debug.Log("Player Level: " + playerLevel); Debug.Log("Dungeon Level: " + completedDungeonLevel);
        baseQuality = playerLevel + completedDungeonLevel;
        Debug.Log("Base Quality: " + baseQuality);
        finalQuality = baseQuality + Random.Range(-Mathf.FloorToInt(baseQuality / 10), Mathf.FloorToInt(baseQuality / 10));
        Debug.Log("Final Quality: " + finalQuality);
        Debug.Log("Hex Final Quality: " + finalQuality.ToString("X2"));
        Debug.Log("Hex Final Quality & Hex Seed: " + finalQuality.ToString("X2") + " & " + initialSeed.ToString("X4"));
        #endregion
        #region DUNGEON RANK
        if (finalQuality >= 2 && finalQuality <= 55)
        {
            dungeonRank = 1; depth = Random.Range(2, 5); sMonsterRank = Random.Range(1, 4); boss = Random.Range(1, 4);
        }
        else if (finalQuality >= 56 && finalQuality <= 60)
        {
            dungeonRank = 2; depth = Random.Range(4, 7); sMonsterRank = Random.Range(2, 5); boss = Random.Range(1, 4);
        }
        else if (finalQuality >= 61 && finalQuality <= 75)
        {
            dungeonRank = 3; depth = Random.Range(4, 7); sMonsterRank = Random.Range(2, 5); boss = Random.Range(2, 6);
        }
        else if (finalQuality >= 76 && finalQuality <= 80)
        {
            dungeonRank = 4; depth = Random.Range(6, 11); sMonsterRank = Random.Range(3, 6); boss = Random.Range(2, 6);
        }
        else if (finalQuality >= 81 && finalQuality <= 100)
        {
            dungeonRank = 5; depth = Random.Range(6, 11); sMonsterRank = Random.Range(3, 6); boss = Random.Range(3, 8);
        }
        else if (finalQuality >= 101 && finalQuality <= 120)
        {
            dungeonRank = 6; depth = Random.Range(8, 13); sMonsterRank = Random.Range(4, 7); boss = Random.Range(4, 8);
        }
        else if (finalQuality >= 121 && finalQuality <= 140)
        {
            dungeonRank = 7; depth = Random.Range(10, 15); sMonsterRank = Random.Range(4, 7); boss = Random.Range(5, 10);
        }
        else if (finalQuality >= 141 && finalQuality <= 160)
        {
            dungeonRank = 8; depth = Random.Range(10, 17); sMonsterRank = Random.Range(5, 8); boss = Random.Range(6, 10);
        }
        else if (finalQuality >= 161 && finalQuality <= 180)
        {
            dungeonRank = 9; depth = Random.Range(10, 17); sMonsterRank = Random.Range(5, 8); boss = Random.Range(7, 11);
        }
        else if (finalQuality >= 181 && finalQuality <= 200)
        {
            dungeonRank = 10; depth = Random.Range(11, 17); sMonsterRank = Random.Range(6, 10); boss = Random.Range(8, 13);
        }
        else if (finalQuality >= 201 && finalQuality <= 220)
        {
            dungeonRank = 11; depth = Random.Range(12, 17); sMonsterRank = Random.Range(8, 10); boss = Random.Range(1, 13);
        }
        else if (finalQuality >= 221 && finalQuality <= 248)
        {
            dungeonRank = 12; depth = Random.Range(14, 17); sMonsterRank = 9; boss = Random.Range(1, 13);
        }
        Debug.Log("Dungeon Rank: " + dungeonRank); Debug.Log("Depth: " + depth); Debug.Log("Monster Rank: " + sMonsterRank); Debug.Log("Boss: " + boss);
        #endregion
        #region DUNGEON LEVEL
        dungeonLevel = 3 * (depth + sMonsterRank + boss - 4);
        Debug.Log("Dungeon Level: " + dungeonLevel);
        dungeonLevelVariation = Random.Range(-5, 6);
        Debug.Log("Dungeon Level Variation: " + dungeonLevelVariation);
        if (dungeonLevelVariation + 5 == 10) { dungeonLevelVariationSeeding = "a"; } else { dungeonLevelVariationSeeding = (dungeonLevelVariation + 5).ToString(); }
        Debug.Log("Dungeon Level Variation Seeding: " + dungeonLevelVariationSeeding);
        dungeonLevel += dungeonLevelVariation;
        if (dungeonLevel <= 0) { dungeonLevel = 1; } else if (dungeonLevel >= 100) { dungeonLevel = 99; } //prevents levels under 1 and over 99
        Debug.Log("Dungeon Level After Variation: " + dungeonLevel);
        #endregion
        #region LOCATION
        dungeonLocation = Random.Range(1, 6);
        Debug.Log("Dungeon Location: " + dungeonLocation);
        #endregion
        #region DUNGEON NAME GENERATION
        #region PREFIX
        if (sMonsterRank == 1 || sMonsterRank == 2)
        {
            dungeonNamePrefix = monsterRankOneTwo[Random.Range(0, monsterRankOneTwo.Length)];
        }
        else if (sMonsterRank == 3 || sMonsterRank == 4)
        {
            dungeonNamePrefix = monsterRankThreeFour[Random.Range(0, monsterRankOneTwo.Length)];
        }
        else if (sMonsterRank == 5 || sMonsterRank == 6)
        {
            dungeonNamePrefix = monsterRankFiveSix[Random.Range(0, monsterRankOneTwo.Length)];
        }
        else if (sMonsterRank == 7 || sMonsterRank == 8)
        {
            dungeonNamePrefix = monsterRankSevenEight[Random.Range(0, monsterRankOneTwo.Length)];
        }
        else
        {
            dungeonNamePrefix = monsterRankNine[Random.Range(0, monsterRankOneTwo.Length)];
        }
        #endregion PREFIX
        #region LOCALE
        #region LOCALE RANK
        if (depth >= 2 && depth <= 3)
        {
            dungeonLocaleRank = Random.Range(1, 3);
        }
        else if (depth >= 4 && depth <= 5)
        {
            dungeonLocaleRank = Random.Range(1, 4);
        }
        else if (depth >= 6 && depth <= 7)
        {
            dungeonLocaleRank = Random.Range(1, 5);
        }
        else if (depth >= 8 && depth <= 9)
        {
            dungeonLocaleRank = Random.Range(2, 6);
        }
        else if (depth >= 10 && depth <= 11)
        {
            dungeonLocaleRank = Random.Range(2, 7);
        }
        else if (depth >= 12 && depth <= 13)
        {
            dungeonLocaleRank = Random.Range(3, 8);
        }
        else if (depth >= 14 && depth <= 15)
        {
            dungeonLocaleRank = Random.Range(4, 9);
        }
        else
        {
            dungeonLocaleRank = Random.Range(6, 9);
        }
        #endregion
        #region LOCALE NAME
        if (dungeonLocaleRank == 1)
        {
            if (dungeonLocation == 2)
            {
                dungeonNameLocation = localeRankOne[0];
            }
            else
            {
                dungeonNameLocation = localeRankOne[1];
            }
        }
        else if (dungeonLocaleRank == 2)
        {
            if (dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankTwo[0];
            }
            else if (dungeonLocation == 5)
            {
                dungeonNameLocation = localeRankTwo[1];
            }
            else
            {
                dungeonNameLocation = localeRankTwo[2];
            }
        }
        else if (dungeonLocaleRank == 3)
        {
            if (dungeonLocation == 3)
            {
                dungeonNameLocation = localeRankThree[0];
            }
            else if (dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankThree[1];
            }
            else if (dungeonLocation == 5)
            {
                dungeonNameLocation = localeRankThree[2];
            }
            else
            {
                dungeonNameLocation = localeRankThree[3];
            }
        }
        else if (dungeonLocaleRank == 4)
        {
            if (dungeonLocation == 3)
            {
                dungeonNameLocation = localeRankFour[0];
            }
            else if (dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankFour[1];
            }
            else if (dungeonLocation == 5)
            {
                dungeonNameLocation = localeRankFour[2];
            }
            else
            {
                dungeonNameLocation = localeRankFour[3];
            }
        }
        else if (dungeonLocaleRank == 5)
        {
            dungeonNameLocation = localeRankFive[0];
        }
        else if (dungeonLocaleRank == 6)
        {
            if (dungeonLocation == 2)
            {
                dungeonNameLocation = localeRankSix[0];
            }
            else if (dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankSix[1];
            }
            else if (dungeonLocation == 5)
            {
                dungeonNameLocation = localeRankSix[2];
            }
            else
            {
                dungeonNameLocation = localeRankSix[3];
            }
        }
        else if (dungeonLocaleRank == 7)
        {
            dungeonNameLocation = localeRankSeven[0];
        }
        else
        {
            if (dungeonLocation == 1)
            {
                dungeonNameLocation = localeRankEight[0];
            }
            else if (dungeonLocation == 2)
            {
                dungeonNameLocation = localeRankEight[1];
            }
            else if (dungeonLocation == 3)
            {
                dungeonNameLocation = localeRankEight[2];
            }
            else if (dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankEight[3];
            }
            else
            {
                dungeonNameLocation = localeRankEight[4];
            }
        }
        #endregion
        #endregion
        #region SUFFIX
        if (boss >= 1 && boss <= 3)
        {
            dungeonNameSuffix = bossOneToThree[Random.Range(0, bossOneToThree.Length)];
        }
        else if (boss >= 4 && boss <= 6)
        {
            dungeonNameSuffix = bossFourToSix[Random.Range(0, bossFourToSix.Length)];
        }
        else if (boss >= 7 && boss <= 9)
        {
            dungeonNameSuffix = bossSevenToNine[Random.Range(0, bossSevenToNine.Length)];
        }
        else
        {
            dungeonNameSuffix = bossTenToTwelve[Random.Range(0, bossTenToTwelve.Length)];
        }
        #endregion
        dungeonName = dungeonNamePrefix + " " + dungeonNameLocation + " of " + dungeonNameSuffix + " Lv. " + dungeonLevel; Debug.Log("Dungeon Name: " + dungeonName);
        #endregion
        #region GENERATE DUNGEON MAPS
        Debug.Log("Floors to generate: " + depth);
        GenerateDungeonMaps(depth); //generates the dungeons
        #endregion
    }

    public void GenerateInitialDungeonInfo()
    {
        ClearLog(); //clears the console log
        ClearFloors();
        #region SEED
        seed = seedInputField.text;
        if(seed == string.Empty)
        {
            initialSeed = Random.Range(1, 32769); Debug.Log("InitialSeed: " + initialSeed);
            Random.InitState(initialSeed);
        }
        else
        {
            initialSeed = int.Parse(seed); Debug.Log("InitialSeed: " + initialSeed);
            Random.InitState(initialSeed);
        }
        gameController.currSeed = initialSeed;
        seed = initialSeed.ToString();
        Debug.Log("Seed: " + seed);
        Debug.Log("Hex Seed: " + initialSeed.ToString("X4"));
        #endregion
        #region QUALITY
        Debug.Log("Player Level: " + playerLevel); Debug.Log("Dungeon Level: " + completedDungeonLevel);
        baseQuality = playerLevel + completedDungeonLevel;
        Debug.Log("Base Quality: " + baseQuality);
        finalQuality = baseQuality + Random.Range(-Mathf.FloorToInt(baseQuality / 10), Mathf.FloorToInt(baseQuality / 10));
        Debug.Log("Final Quality: " + finalQuality);
        Debug.Log("Hex Final Quality: " + finalQuality.ToString("X2"));
        Debug.Log("Hex Final Quality & Hex Seed: " + finalQuality.ToString("X2") + " & " + initialSeed.ToString("X4"));
        #endregion
        #region DUNGEON RANK
        if (finalQuality >= 2 && finalQuality <= 55)
        {
            dungeonRank = 1; depth = Random.Range(2, 5); sMonsterRank = Random.Range(1, 4); boss = Random.Range(1, 4);
        }
        else if (finalQuality >= 56 && finalQuality <= 60)
        {
            dungeonRank = 2; depth = Random.Range(4, 7); sMonsterRank = Random.Range(2, 5); boss = Random.Range(1, 4);
        }
        else if (finalQuality >= 61 && finalQuality <= 75)
        {
            dungeonRank = 3; depth = Random.Range(4, 7); sMonsterRank = Random.Range(2, 5); boss = Random.Range(2, 6);
        }
        else if (finalQuality >= 76 && finalQuality <= 80)
        {
            dungeonRank = 4; depth = Random.Range(6, 11); sMonsterRank = Random.Range(3, 6); boss = Random.Range(2, 6);
        }
        else if (finalQuality >= 81 && finalQuality <= 100)
        {
            dungeonRank = 5; depth = Random.Range(6, 11); sMonsterRank = Random.Range(3, 6); boss = Random.Range(3, 8);
        }
        else if (finalQuality >= 101 && finalQuality <= 120)
        {
            dungeonRank = 6; depth = Random.Range(8, 13); sMonsterRank = Random.Range(4, 7); boss = Random.Range(4, 8); 
        }
        else if (finalQuality >= 121 && finalQuality <= 140)
        {
            dungeonRank = 7; depth = Random.Range(10, 15); sMonsterRank = Random.Range(4, 7); boss = Random.Range(5, 10); 
        }
        else if (finalQuality >= 141 && finalQuality <= 160)
        {
            dungeonRank = 8; depth = Random.Range(10, 17); sMonsterRank = Random.Range(5, 8); boss = Random.Range(6, 10); 
        }
        else if (finalQuality >= 161 && finalQuality <= 180)
        {
            dungeonRank = 9; depth = Random.Range(10, 17); sMonsterRank = Random.Range(5, 8); boss = Random.Range(7, 11);
        }
        else if (finalQuality >= 181 && finalQuality <= 200)
        {
            dungeonRank = 10; depth = Random.Range(11, 17); sMonsterRank = Random.Range(6, 10); boss = Random.Range(8, 13);
        }
        else if (finalQuality >= 201 && finalQuality <= 220)
        {
            dungeonRank = 11; depth = Random.Range(12, 17); sMonsterRank = Random.Range(8, 10); boss = Random.Range(1, 13);
        }
        else if (finalQuality >= 221 && finalQuality <= 248)
        {
            dungeonRank = 12; depth = Random.Range(14, 17); sMonsterRank = 9; boss = Random.Range(1, 13);
        }
        Debug.Log("Dungeon Rank: " + dungeonRank); Debug.Log("Depth: " + depth); Debug.Log("Monster Rank: " + sMonsterRank); Debug.Log("Boss: " + boss);
        #endregion
        #region DUNGEON LEVEL
        dungeonLevel = 3 * (depth + sMonsterRank + boss - 4);
        Debug.Log("Dungeon Level: " + dungeonLevel);
        dungeonLevelVariation = Random.Range(-5, 6);
        Debug.Log("Dungeon Level Variation: " + dungeonLevelVariation);
        if (dungeonLevelVariation + 5 == 10) { dungeonLevelVariationSeeding = "a"; } else { dungeonLevelVariationSeeding = (dungeonLevelVariation + 5).ToString(); }
        Debug.Log("Dungeon Level Variation Seeding: " + dungeonLevelVariationSeeding);
        dungeonLevel += dungeonLevelVariation;
        if (dungeonLevel <= 0) { dungeonLevel = 1; } else if (dungeonLevel >= 100) { dungeonLevel = 99; } //prevents levels under 1 and over 99
        Debug.Log("Dungeon Level After Variation: " + dungeonLevel);
        #endregion
        #region LOCATION
        dungeonLocation = Random.Range(1, 6);
        Debug.Log("Dungeon Location: " + dungeonLocation);
        #endregion
        #region DUNGEON NAME GENERATION
        #region PREFIX
        if (sMonsterRank == 1 || sMonsterRank == 2)
        {
            dungeonNamePrefix = monsterRankOneTwo[Random.Range(0, monsterRankOneTwo.Length)];
        }
        else if (sMonsterRank == 3 || sMonsterRank == 4)
        {
            dungeonNamePrefix = monsterRankThreeFour[Random.Range(0, monsterRankOneTwo.Length)];
        }
        else if (sMonsterRank == 5 || sMonsterRank == 6)
        {
            dungeonNamePrefix = monsterRankFiveSix[Random.Range(0, monsterRankOneTwo.Length)];
        }
        else if (sMonsterRank == 7 || sMonsterRank == 8)
        {
            dungeonNamePrefix = monsterRankSevenEight[Random.Range(0, monsterRankOneTwo.Length)];
        }
        else
        {
            dungeonNamePrefix = monsterRankNine[Random.Range(0, monsterRankOneTwo.Length)];
        }
        #endregion PREFIX
        #region LOCALE
        #region LOCALE RANK
        if (depth >= 2 && depth <= 3)
        {
            dungeonLocaleRank = Random.Range(1, 3);
        }
        else if (depth >= 4 && depth <= 5)
        {
            dungeonLocaleRank = Random.Range(1, 4);
        }
        else if (depth >= 6 && depth <= 7)
        {
            dungeonLocaleRank = Random.Range(1, 5);
        }
        else if (depth >= 8 && depth <= 9)
        {
            dungeonLocaleRank = Random.Range(2, 6);
        }
        else if (depth >= 10 && depth <= 11)
        {
            dungeonLocaleRank = Random.Range(2, 7);
        }
        else if (depth >= 12 && depth <= 13)
        {
            dungeonLocaleRank = Random.Range(3, 8);
        }
        else if (depth >= 14 && depth <= 15)
        {
            dungeonLocaleRank = Random.Range(4, 9);
        }
        else
        {
            dungeonLocaleRank = Random.Range(6, 9);
        }
        #endregion
        #region LOCALE NAME
        if (dungeonLocaleRank == 1)
        {
            if(dungeonLocation == 2)
            {
                dungeonNameLocation = localeRankOne[0];
            }
            else
            {
                dungeonNameLocation = localeRankOne[1];
            }
        }
        else if (dungeonLocaleRank == 2)
        {
            if(dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankTwo[0];
            }
            else if (dungeonLocation == 5)
            {
                dungeonNameLocation = localeRankTwo[1];
            }
            else
            {
                dungeonNameLocation = localeRankTwo[2];
            }
        }
        else if (dungeonLocaleRank == 3)
        {
            if (dungeonLocation == 3)
            {
                dungeonNameLocation = localeRankThree[0];
            }
            else if (dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankThree[1];
            }
            else if (dungeonLocation == 5)
            {
                dungeonNameLocation = localeRankThree[2];
            }
            else
            {
                dungeonNameLocation = localeRankThree[3];
            }
        }
        else if (dungeonLocaleRank == 4)
        {
            if (dungeonLocation == 3)
            {
                dungeonNameLocation = localeRankFour[0];
            }
            else if (dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankFour[1];
            }
            else if (dungeonLocation == 5)
            {
                dungeonNameLocation = localeRankFour[2];
            }
            else
            {
                dungeonNameLocation = localeRankFour[3];
            }
        }
        else if (dungeonLocaleRank == 5)
        {
            dungeonNameLocation = localeRankFive[0];
        }
        else if (dungeonLocaleRank == 6)
        {
            if (dungeonLocation == 2)
            {
                dungeonNameLocation = localeRankSix[0];
            }
            else if (dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankSix[1];
            }
            else if (dungeonLocation == 5)
            {
                dungeonNameLocation = localeRankSix[2];
            }
            else
            {
                dungeonNameLocation = localeRankSix[3];
            }
        }
        else if (dungeonLocaleRank == 7)
        {
            dungeonNameLocation = localeRankSeven[0];
        }
        else
        {
            if (dungeonLocation == 1)
            {
                dungeonNameLocation = localeRankEight[0];
            }
            else if (dungeonLocation == 2)
            {
                dungeonNameLocation = localeRankEight[1];
            }
            else if (dungeonLocation == 3)
            {
                dungeonNameLocation = localeRankEight[2];
            }
            else if (dungeonLocation == 4)
            {
                dungeonNameLocation = localeRankEight[3];
            }
            else
            {
                dungeonNameLocation = localeRankEight[4];
            }
        }
        #endregion
        #endregion
        #region SUFFIX
        if(boss >= 1 && boss <= 3)
        {
            dungeonNameSuffix = bossOneToThree[Random.Range(0, bossOneToThree.Length)];
        }
        else if(boss >= 4 && boss <= 6)
        {
            dungeonNameSuffix = bossFourToSix[Random.Range(0, bossFourToSix.Length)];
        }
        else if (boss >= 7 && boss <= 9)
        {
            dungeonNameSuffix = bossSevenToNine[Random.Range(0, bossSevenToNine.Length)];
        }
        else
        {
            dungeonNameSuffix = bossTenToTwelve[Random.Range(0, bossTenToTwelve.Length)];
        }
        #endregion
        dungeonName = dungeonNamePrefix + " " + dungeonNameLocation + " of " + dungeonNameSuffix + " Lv. " + dungeonLevel; Debug.Log("Dungeon Name: " + dungeonName);
        #endregion
        #region GENERATE DUNGEON MAPS
        Debug.Log("Floors to generate: " + depth);
        GenerateDungeonMaps(depth); //generates the dungeons
        #endregion
    }

    /*void SetSeed()
    {
        #region SEED
        seed = seedInputField.text;
        if (seed == string.Empty)
        {
            initialSeed = Random.Range(1, 32769); Debug.Log("InitialSeed: " + initialSeed);
            Random.InitState(initialSeed);
        }
        else
        {
            initialSeed = int.Parse(seed); Debug.Log("InitialSeed: " + initialSeed);
            Random.InitState(initialSeed);
        }
        seed = initialSeed.ToString();
        Debug.Log("Seed: " + seed);
        Debug.Log("Hex Seed: " + initialSeed.ToString("X4"));
        #endregion
    }*/

    void GenerateDungeonMaps(int floors)
    {
        rows = Random.Range(8, 17) * 3;
        columns = rows;
        maxTunnels = Mathf.CeilToInt((rows / 3) * 2); maxLength = Mathf.CeilToInt((rows / 3) / 3);
        #region MAP GENERATION
        SetStartMapState();
        for (int i = 1; i <= floors; i++) 
        {
            Debug.Log("Generating Floor: " + i);
            FillInMap(i);
            FirstFloorTilePlacement(i);
            RandomWalkAlgorithm(i);
            PlaceWalls(i);
            StairTilePlacement(i);
            ChestTilePlacement(i);
        }
        SetEndMapState();
        #endregion
    }

    void InstantiateFloorGameObject(int floorNumber)
    {
        GameObject newObj = new GameObject("F" + floorNumber);
        newObj.transform.parent = transform;
    }

    void InstantiateBossFloorGameObject()
    {
        GameObject newObj = new GameObject("BOSS");
        newObj.transform.parent = transform;
    }

    void FillInMap(int floorNumber)
    {
        mapGridBig = new int[rows, columns];
        mapGridSmall = new GameObject[rows, columns];
        floorTiles = new List<GameObject>();
        for (int x = 1; x < rows; x = x++ + 3)
        {
            for (int y = 1; y < columns; y = y++ + 3)
            {
                mapGridBig[x, y] = 0;
                for (int i = x - 1; i < x + 2; i++)
                {
                    for (int j = y - 1; j < y + 2; j++)
                    {
                        mapGridSmall[i, j] = Instantiate(tiles[mapGridBig[x,y]], new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        mapGridSmall[i, j].transform.parent = transform.Find("F" + floorNumber);
                    }
                }
            }
        }
    }

    void FirstFloorTilePlacement(int floorNumber)
    {
    ReRandomiseX:
        int randomX = Random.Range(1, rows - 4);
        if (randomX % 3 != 0)
        {
            goto ReRandomiseX;
        }
    ReRandomiseY:
        int randomY = Random.Range(1, columns - 4);
        if (randomY % 3 != 0)
        {
            goto ReRandomiseY;
        }
        randomX++; randomY++;

        mapGridBig[randomX, randomY] = 1;
        for (int i = randomX - 1; i < randomX + 2; i++)
        {
            for (int j = randomY - 1; j < randomY + 2; j++)
            {
                Destroy(mapGridSmall[i, j]);
                mapGridSmall[i, j] = Instantiate(tiles[mapGridBig[randomX, randomY]], new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                mapGridSmall[i, j].transform.parent = transform.Find("F" + floorNumber);
            }
        }
        floorTiles.Add(mapGridSmall[randomX, randomY]);
    }
    
    void RandomWalkAlgorithm(int floorNumber)
    {
        for (int a = 0; a < maxTunnels; a++)
        {
            NewDirection:
            int randomDirection = Random.Range(0, 4);
            int directionX; int directionY;
            int nextTileX; int nextTileY;
            GameObject latestFloorTile;
            latestFloorTile = floorTiles[floorTiles.Count - 1];
            
            randomLength = Random.Range(1, maxLength + 1);

            switch (randomDirection)
            {
                case 0:
                    //Walk Right
                    //Debug.Log("Walk Right");
                    (directionX, directionY) = directions[randomDirection];
                    nextTileX = (int)latestFloorTile.transform.localPosition.x + directionX;
                    nextTileY = (int)latestFloorTile.transform.localPosition.z + directionY;
                    if (IsOutOfBounds(nextTileX, nextTileY) || lastDirection == directions[0] || lastDirection == directions[1])
                    {
                        goto NewDirection;
                    }
                    else
                    {
                        for (int tunnelLength = 0; tunnelLength < randomLength; tunnelLength++)
                        {
                            if (IsOutOfBounds(nextTileX, nextTileY))
                            {
                                goto NewDirection;
                            }
                            else
                            {
                                mapGridBig[nextTileX, nextTileY] = 1;
                                for (int i = nextTileX - 1; i < nextTileX + 2; i++)
                                {
                                    for (int j = nextTileY - 1; j < nextTileY + 2; j++)
                                    {
                                        Destroy(mapGridSmall[i, j]);
                                        mapGridSmall[i, j] = Instantiate(tiles[mapGridBig[nextTileX, nextTileY]], new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                                        mapGridSmall[i, j].transform.parent = transform.Find("F" + floorNumber);
                                    }
                                }
                                floorTiles.Add(mapGridSmall[nextTileX, nextTileY]);
                                nextTileX += directionX; nextTileY += directionY;
                            }
                        }
                        lastDirection = directions[randomDirection];
                    }
                    break;
                case 1:
                    //Walk Left
                    //Debug.Log("Walk Left");
                    (directionX, directionY) = directions[randomDirection];
                    nextTileX = (int)latestFloorTile.transform.localPosition.x + directionX;
                    nextTileY = (int)latestFloorTile.transform.localPosition.z + directionY;
                    if (IsOutOfBounds(nextTileX, nextTileY) || lastDirection == directions[0] || lastDirection == directions[1])
                    {
                        goto NewDirection;
                    }
                    else
                    {
                        for (int tunnelLength = 0; tunnelLength < randomLength; tunnelLength++)
                        {
                            if (IsOutOfBounds(nextTileX, nextTileY))
                            {
                                goto NewDirection;
                            }
                            else
                            {
                                mapGridBig[nextTileX, nextTileY] = 1;
                                for (int i = nextTileX - 1; i < nextTileX + 2; i++)
                                {
                                    for (int j = nextTileY - 1; j < nextTileY + 2; j++)
                                    {
                                        Destroy(mapGridSmall[i, j]);
                                        mapGridSmall[i, j] = Instantiate(tiles[mapGridBig[nextTileX, nextTileY]], new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                                        mapGridSmall[i, j].transform.parent = transform.Find("F" + floorNumber);
                                    }
                                }
                                floorTiles.Add(mapGridSmall[nextTileX, nextTileY]);
                                nextTileX += directionX; nextTileY += directionY;
                            }
                        }
                        lastDirection = directions[randomDirection];
                    }
                    break;
                case 2:
                    //Walk Up
                    //Debug.Log("Walk Up");
                    (directionX, directionY) = directions[randomDirection];
                    nextTileX = (int)latestFloorTile.transform.localPosition.x + directionX;
                    nextTileY = (int)latestFloorTile.transform.localPosition.z + directionY;
                    if (IsOutOfBounds(nextTileX, nextTileY) || lastDirection == directions[2] || lastDirection == directions[3])
                    {
                        goto NewDirection;
                    }
                    else
                    {
                        for (int tunnelLength = 0; tunnelLength < randomLength; tunnelLength++)
                        {
                            if (IsOutOfBounds(nextTileX, nextTileY))
                            {
                                goto NewDirection;
                            }
                            else
                            {
                                mapGridBig[nextTileX, nextTileY] = 1;
                                for (int i = nextTileX - 1; i < nextTileX + 2; i++)
                                {
                                    for (int j = nextTileY - 1; j < nextTileY + 2; j++)
                                    {
                                        Destroy(mapGridSmall[i, j]);
                                        mapGridSmall[i, j] = Instantiate(tiles[mapGridBig[nextTileX, nextTileY]], new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                                        mapGridSmall[i, j].transform.parent = transform.Find("F" + floorNumber);
                                    }
                                }
                                floorTiles.Add(mapGridSmall[nextTileX, nextTileY]);
                                nextTileX += directionX; nextTileY += directionY;
                            }
                        }
                        lastDirection = directions[randomDirection];
                    }
                    break;
                case 3:
                    //Walk Down
                    //Debug.Log("Walk Down");
                    (directionX, directionY) = directions[randomDirection];
                    nextTileX = (int)latestFloorTile.transform.localPosition.x + directionX;
                    nextTileY = (int)latestFloorTile.transform.localPosition.z + directionY;
                    if (IsOutOfBounds(nextTileX, nextTileY) || lastDirection == directions[2] || lastDirection == directions[3])
                    {
                        goto NewDirection;
                    }
                    else
                    {
                        for (int tunnelLength = 0; tunnelLength < randomLength; tunnelLength++)
                        {
                            if (IsOutOfBounds(nextTileX, nextTileY))
                            {
                                goto NewDirection;
                            }
                            else
                            {
                                mapGridBig[nextTileX, nextTileY] = 1;
                                for (int i = nextTileX - 1; i < nextTileX + 2; i++)
                                {
                                    for (int j = nextTileY - 1; j < nextTileY + 2; j++)
                                    {
                                        Destroy(mapGridSmall[i, j]);
                                        mapGridSmall[i, j] = Instantiate(tiles[mapGridBig[nextTileX, nextTileY]], new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                                        mapGridSmall[i, j].transform.parent = transform.Find("F" + floorNumber);
                                    }
                                }
                                floorTiles.Add(mapGridSmall[nextTileX, nextTileY]);
                                nextTileX += directionX; nextTileY += directionY;
                            }
                        }
                        lastDirection = directions[randomDirection];
                    }
                    break;
                default:
                    Debug.LogError("Switch Defaulted.");
                    break;
            }
        }
    }

    void PlaceWalls(int floorNumber)
    {
        for (int x = 1; x < rows; x = x++ + 3)
        {
            for (int y = 1; y < columns; y = y++ + 3)
            {
                if (mapGridBig[x, y] == 1)
                {
                    //Debug.Log("WhereAdjacentFloors of: " + x + ", " + y + " - " + WhereAdjacentFloors(x, y)[0] + WhereAdjacentFloors(x, y)[1] + WhereAdjacentFloors(x, y)[2] + WhereAdjacentFloors(x, y)[3]);
                    if (WhereAdjacentFloors(x, y)[0] == (0, 0))
                    {
                        Destroy(mapGridSmall[x + directionX[0], y + directionY[0]]);
                        mapGridSmall[x + directionX[0], y + directionY[0]] = Instantiate(tiles[0], new Vector3(x + directionX[0], 0, y + directionY[0]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[0], y + directionY[0]].transform.parent = transform.Find("F" + floorNumber);

                        Destroy(mapGridSmall[x + directionX[0], y + directionY[2]]);
                        mapGridSmall[x + directionX[0], y + directionY[2]] = Instantiate(tiles[0], new Vector3(x + directionX[0], 0, y + directionY[2]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[0], y + directionY[2]].transform.parent = transform.Find("F" + floorNumber);

                        Destroy(mapGridSmall[x + directionX[0], y + directionY[3]]);
                        mapGridSmall[x + directionX[0], y + directionY[3]] = Instantiate(tiles[0], new Vector3(x + directionX[0], 0, y + directionY[3]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[0], y + directionY[3]].transform.parent = transform.Find("F" + floorNumber);
                    }
                    if (WhereAdjacentFloors(x, y)[1] == (0, 0))
                    {
                        Destroy(mapGridSmall[x + directionX[1], y + directionY[1]]);
                        mapGridSmall[x + directionX[1], y + directionY[1]] = Instantiate(tiles[0], new Vector3(x + directionX[1], 0, y + directionY[1]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[1], y + directionY[1]].transform.parent = transform.Find("F" + floorNumber);

                        Destroy(mapGridSmall[x + directionX[1], y + directionY[2]]);
                        mapGridSmall[x + directionX[1], y + directionY[2]] = Instantiate(tiles[0], new Vector3(x + directionX[1], 0, y + directionY[2]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[1], y + directionY[2]].transform.parent = transform.Find("F" + floorNumber);

                        Destroy(mapGridSmall[x + directionX[1], y + directionY[3]]);
                        mapGridSmall[x + directionX[1], y + directionY[3]] = Instantiate(tiles[0], new Vector3(x + directionX[1], 0, y + directionY[3]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[1], y + directionY[3]].transform.parent = transform.Find("F" + floorNumber);
                    }
                    if (WhereAdjacentFloors(x, y)[2] == (0, 0))
                    {
                        Destroy(mapGridSmall[x + directionX[2], y + directionY[2]]);
                        mapGridSmall[x + directionX[2], y + directionY[2]] = Instantiate(tiles[0], new Vector3(x + directionX[2], 0, y + directionY[2]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[2], y + directionY[2]].transform.parent = transform.Find("F" + floorNumber);

                        Destroy(mapGridSmall[x + directionX[0], y + directionY[2]]);
                        mapGridSmall[x + directionX[0], y + directionY[2]] = Instantiate(tiles[0], new Vector3(x + directionX[0], 0, y + directionY[2]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[0], y + directionY[2]].transform.parent = transform.Find("F" + floorNumber);

                        Destroy(mapGridSmall[x + directionX[1], y + directionY[2]]);
                        mapGridSmall[x + directionX[1], y + directionY[2]] = Instantiate(tiles[0], new Vector3(x + directionX[1], 0, y + directionY[2]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[1], y + directionY[2]].transform.parent = transform.Find("F" + floorNumber);
                    }
                    if (WhereAdjacentFloors(x, y)[3] == (0, 0))
                    {
                        Destroy(mapGridSmall[x + directionX[3], y + directionY[3]]);
                        mapGridSmall[x + directionX[3], y + directionY[3]] = Instantiate(tiles[0], new Vector3(x + directionX[3], 0, y + directionY[3]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[3], y + directionY[3]].transform.parent = transform.Find("F" + floorNumber);

                        Destroy(mapGridSmall[x + directionX[0], y + directionY[3]]);
                        mapGridSmall[x + directionX[0], y + directionY[3]] = Instantiate(tiles[0], new Vector3(x + directionX[0], 0, y + directionY[3]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[0], y + directionY[3]].transform.parent = transform.Find("F" + floorNumber);

                        Destroy(mapGridSmall[x + directionX[1], y + directionY[3]]);
                        mapGridSmall[x + directionX[1], y + directionY[3]] = Instantiate(tiles[0], new Vector3(x + directionX[1], 0, y + directionY[3]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[1], y + directionY[3]].transform.parent = transform.Find("F" + floorNumber);
                    }
                    if (WhereDiagonalFloors(x, y)[0] == (0, 0))
                    {
                        Destroy(mapGridSmall[x + directionX[0], y + directionY[3]]);
                        mapGridSmall[x + directionX[0], y + directionY[3]] = Instantiate(tiles[0], new Vector3(x + directionX[0], 0, y + directionY[3]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[0], y + directionY[3]].transform.parent = transform.Find("F" + floorNumber);
                    }
                    if (WhereDiagonalFloors(x, y)[1] == (0, 0))
                    {
                        Destroy(mapGridSmall[x + directionX[0], y + directionY[2]]);
                        mapGridSmall[x + directionX[0], y + directionY[2]] = Instantiate(tiles[0], new Vector3(x + directionX[0], 0, y + directionY[2]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[0], y + directionY[2]].transform.parent = transform.Find("F" + floorNumber);
                    }
                    if (WhereDiagonalFloors(x, y)[2] == (0, 0))
                    {
                        Destroy(mapGridSmall[x + directionX[1], y + directionY[3]]);
                        mapGridSmall[x + directionX[1], y + directionY[3]] = Instantiate(tiles[0], new Vector3(x + directionX[1], 0, y + directionY[3]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[1], y + directionY[3]].transform.parent = transform.Find("F" + floorNumber);
                    }
                    if (WhereDiagonalFloors(x, y)[3] == (0, 0))
                    {
                        Destroy(mapGridSmall[x + directionX[1], y + directionY[2]]);
                        mapGridSmall[x + directionX[1], y + directionY[2]] = Instantiate(tiles[0], new Vector3(x + directionX[1], 0, y + directionY[2]), Quaternion.identity) as GameObject;
                        mapGridSmall[x + directionX[1], y + directionY[2]].transform.parent = transform.Find("F" + floorNumber);
                    }
                }
            }
        }
    }

    void StairTilePlacement(int floorNumber)
    {
        for (int k = 2; k <= 3; k++)
        {
        ReRandomiseX:
            int randomX = Random.Range(1, rows - 4);
            if (randomX % 3 != 0)
            {
                goto ReRandomiseX;
            }
        ReRandomiseY:
            int randomY = Random.Range(1, columns - 4);
            if (randomY % 3 != 0)
            {
                goto ReRandomiseY;
            }
            randomX++; randomY++;

            if (mapGridBig[randomX, randomY] != 1)
            {
                goto ReRandomiseX;
            }

            mapGridBig[randomX, randomY] = k;

        ReRandomiseMini:
            int randomMiniX = Random.Range(randomX - 1, randomX + 2);
            int randomMiniY = Random.Range(randomY - 1, randomY + 2);
            if (mapGridSmall[randomMiniX, randomMiniY].name == "FloorTileMini(Clone)")
            {
                for (int i = randomX - 1; i < randomX + 2; i++)
                {
                    for (int j = randomY - 1; j < randomY + 2; j++)
                    {
                        if (i == randomMiniX && j == randomMiniY)
                        {
                            floorTiles.Remove(mapGridSmall[randomX, randomY]);
                            Destroy(mapGridSmall[i, j]);
                            mapGridSmall[i, j] = Instantiate(tiles[mapGridBig[randomX, randomY]], new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                            mapGridSmall[i, j].transform.parent = transform.Find("F" + floorNumber);
                        }
                    }

                }
            }
            else
            {
                goto ReRandomiseMini;
            }
        }
    }

    void ChestTilePlacement(int floorNumber)
    {
        if (floorNumber > 2)
        {
            int randomChestCount = Random.Range(0,5);
            for (int k = 0; k <= randomChestCount; k++)
            {
            ReRandomiseX:
                int randomX = Random.Range(1, rows - 4);
                if (randomX % 3 != 0)
                {
                    goto ReRandomiseX;
                }
            ReRandomiseY:
                int randomY = Random.Range(1, columns - 4);
                if (randomY % 3 != 0)
                {
                    goto ReRandomiseY;
                }
                randomX++; randomY++;

                if (mapGridBig[randomX, randomY] != 1)
                {
                    goto ReRandomiseX;
                }

                mapGridBig[randomX, randomY] = 4;
            ReRandomiseMini:
                int randomMiniX = Random.Range(randomX - 1, randomX + 2);
                int randomMiniY = Random.Range(randomY - 1, randomY + 2);
                if (mapGridSmall[randomMiniX, randomMiniY].name == "FloorTileMini(Clone)")
                {
                    for (int i = randomX - 1; i < randomX + 2; i++)
                    {
                        for (int j = randomY - 1; j < randomY + 2; j++)
                        {
                            if (i == randomMiniX && j == randomMiniY)
                            {
                                floorTiles.Remove(mapGridSmall[randomX, randomY]);
                                Destroy(mapGridSmall[i, j]);
                                mapGridSmall[i, j] = Instantiate(tiles[mapGridBig[randomX, randomY]], new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                                mapGridSmall[i, j].transform.parent = transform.Find("F" + floorNumber);
                            }
                        }

                    }
                }
                else
                {
                    goto ReRandomiseMini;
                }
            }
        }
    }

    void SetStartMapState()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    void SetEndMapState()
    {
        foreach (Transform child in transform)
        {
            if (child.name != "F1")
                child.gameObject.SetActive(false);
            else
                child.gameObject.SetActive(true);
        }
    }

    int GetAdjacentFloors(int x, int y, int scopeX, int scopeY)
    {
        int startX = x - scopeX;
        int startY = y - scopeY;
        int endX = x + scopeX;
        int endY = y + scopeY;

        int iX = startX;
        int iY = startY;

        int floorCounter = 0;

        for (iY = startY; iY <= endY; iY++)
        {
            for (iX = startX; iX <= endX; iX++)
            {
                if ((iX == x || iY == y) && !(iX == x && iY == y))
                {
                    if (IsFloor(iX, iY))
                    {
                        floorCounter += 1;
                    }
                }
            }
        }
        //Debug.Log("floor counter before return value: " + floorCounter);
        return floorCounter;
    }

    (int, int)[] WhereAdjacentFloors(int x, int y)
    {
        (int, int)[] direction = new (int, int)[4];
        if (FloorRight(x,y))
        {
            direction[0] = (3, 0);
        }
        if (FloorLeft(x,y))
        {
            direction[1] = (-3, 0);
        }
        if (FloorUp(x,y))
        {
            direction[2] = (0, 3);
        }
        if (FloorDown(x,y))
        {
            direction[3] = (0, -3);
        }
        return direction;
    }

    bool FloorRight(int x, int y)
    {
        if (x < columns - 3)
        {
            if (mapGridBig[x + 3, y] != 1)
            {
                //Debug.Log(mapGridBig[x + 3, y]);
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    bool FloorLeft(int x, int y)
    {
        if (x > 1)
        {
            if (mapGridBig[x - 3, y] != 1)
            {
                //Debug.Log(mapGridBig[x - 3, y]);
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    bool FloorUp(int x, int y)
    {
        if (y < rows - 3)
        {
            if (mapGridBig[x, y + 3] != 1)
            {
                //Debug.Log(mapGridBig[x, y + 3]);
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    bool FloorDown(int x, int y)
    {
        if (y > 1)
        {
            if (mapGridBig[x, y - 3] != 1)
            {
                //Debug.Log(mapGridBig[x, y - 3]);
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    (int, int)[] WhereDiagonalFloors(int x, int y)
    {
        (int, int)[] direction = new (int, int)[4];
        if (FloorRightDown(x, y))
        {
            direction[0] = (3, -3);
        }
        if (FloorRightUp(x, y))
        {
            direction[1] = (3, 3);
        }
        if (FloorLeftDown(x, y))
        {
            direction[2] = (-3, -3);
        }
        if (FloorLeftUp(x, y))
        {
            direction[3] = (-3, 3);
        }
        return direction;
    }

    bool FloorRightDown(int x, int y)
    {
        if (x < columns - 3 && y > 1)
        {
            if (mapGridBig[x + 3, y - 3] != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    bool FloorRightUp(int x, int y)
    {
        if (x < columns - 3 && y < rows - 3)
        {
            if (mapGridBig[x + 3, y + 3] != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    bool FloorLeftDown(int x, int y)
    {
        if (x > 1 && y > 1)
        {
            if (mapGridBig[x - 3, y - 3] != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    bool FloorLeftUp(int x, int y)
    {
        if (x < columns - 3 && y > 1)
        {
            if (mapGridBig[x - 3, y + 3] != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    bool IsFloor(int x, int y)
    {
        // Consider out-of-bound a wall
        if (IsOutOfBounds(x, y))
        {
            return false;
        }

        if (mapGridBig[x, y] == 1)
        {
            return true;
        }

        if (mapGridBig[x, y] == 0)
        {
            return false;
        }
        return false;
    }

    bool IsOutOfBounds(int x, int y)
    {
        if (x <= 1 || y <= 1)
        {
            return true;
        }
        else if (x >= rows - 2 || y >= columns - 2)
        {
            return true;
        }
        return false;
    }

    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    public void ClearFloors()//destroys the maps before generating new maps
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandchild in child)
                Destroy(grandchild.gameObject);
        }
    }

}