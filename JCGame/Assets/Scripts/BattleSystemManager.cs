using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public enum BattleState { START, PLAYERCHOICE, PLAYERTURN, ENEMYTURN, WIN, LOST }
public enum LastAttacked { NEWROUND, PLAYER, ENEMY }

public class BattleSystemManager : MonoBehaviour
{
    private GameObject enemy;
    private GameObject player;

    public Transform enemyBattlePosition;
    public Transform playerBattlePosition;
    
    public CharacterDataSO playerCharData;
    public CharacterDataSO enemyCharData;

    public PlayerStats playerStats;
    public EnemyStats enemyStats;

    public GameObject gameManager;
    public GameController gameController;

    public StatusHUD playerStatusHUD;
    public StatusHUD enemyStatusHUD;

    public MessageHUD messageHUD;

    public string abilityUsed;

    private BattleState battleState;
    private LastAttacked lastAttacked;

    [SerializeField]private bool hasClicked = true;

    void Start()
    {
        battleState = BattleState.START;
        StartCoroutine(BeginBattle());
    }

    IEnumerator BeginBattle()
    {
        gameManager = GameObject.Find("GameManager");
        gameController = gameManager.GetComponent<GameController>();
        enemyCharData = gameController.enemyCharData;

        // spawn characters on the platforms
        player = Instantiate(playerCharData.characterGameObject/*.transform.GetChild(0).gameObject*/, playerBattlePosition); player.SetActive(true);
        enemy = Instantiate(enemyCharData.characterGameObject, enemyBattlePosition); enemy.SetActive(true);

        enemyStats = enemy.GetComponent<EnemyStats>();
        playerStats = player.GetComponent<PlayerStats>();

        // make the characters sprites invisible in the beginning
        //enemy.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        //player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        // set the characters stats in HUD displays
        playerStatusHUD.SetStatusHUD(playerStats);
        enemyStatusHUD.SetStatusHUDEnemy(enemyStats);

        //yield return new WaitForSeconds(1);

        // fade in our characters sprites
        //yield return StartCoroutine(FadeInOpponents());

        yield return new WaitForSeconds(0.5f);

        // new round!
        lastAttacked = LastAttacked.NEWROUND;

        // player turn!
        battleState = BattleState.PLAYERCHOICE;

        // let player select his action now!    
        yield return StartCoroutine(PlayerChoice());
    }

    IEnumerator PlayerChoice()
    {
        // new round!
        lastAttacked = LastAttacked.NEWROUND;
        // probably display some message 
        // stating it's player's turn here
        Debug.Log("Players turn");
        messageHUD.SetMessage("players turn");
        yield return new WaitForSeconds(0.5f);

        // release the blockade on clicking 
        // so that player can click on 'attack' button    
        hasClicked = false;
    }

    public void OnAttackButtonPress()
    {
        // don't allow player to click on 'attack'
        // button if it's not his turn!
        if (battleState != BattleState.PLAYERCHOICE)
            return;

        // allow only a single action per turn
        if (!hasClicked)
        {
            abilityUsed = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            Debug.Log(abilityUsed);
            //here im going to check which character is faster based on agility stat
            if (playerCharData.agility > enemyStats.agility)
            {
                //player faster
                StartCoroutine(PlayerTurn());
            }
            else if (playerCharData.agility < enemyStats.agility)
            {
                //enemy faster
                StartCoroutine(EnemyTurn());
            }
            else
            {
                //same speed, randomise who goes first
                Debug.Log("yeah i havent coded this part yet.. sorry");
            }

            // block user from repeatedly 
            // pressing attack button  
            hasClicked = true;
        }
    }

    IEnumerator PlayerTurn()
    {
        // trigger the execution of attack animation
        // in 'BattlePresence' animator
        //player.GetComponent<Animator>().SetTrigger("Attack");

        //yield return new WaitForSeconds(2);

        // decrease enemy health by a fixed
        // amount of 10. You probably want to have some
        // more complex logic here.
        if (abilityUsed == "Attack")
        {
            enemyStatusHUD.SetHPEnemy(enemyStats, playerCharData.strength);
        }
        else
        {
            enemyStatusHUD.SetHPEnemy(enemyStats, gameController.playerAbilities.PlayerAbilityPower(abilityUsed));
        }
        Debug.Log("Player attack");
        messageHUD.SetMessage("player attacked");
        yield return new WaitForSeconds(0.5f);
        if (enemyStats.currHealth <= 0)
        {
            // if the enemy health drops to 0 
            // we won!
            battleState = BattleState.WIN;
            yield return StartCoroutine(EndBattle());
        }
        else
        {
            if(lastAttacked == LastAttacked.NEWROUND)
            {
                // if the enemy health is still
                // above 0 when the turn finishes
                // it's enemy's turn!
                Debug.Log("enemy health above 0");
                battleState = BattleState.ENEMYTURN;
                lastAttacked = LastAttacked.PLAYER;
                yield return StartCoroutine(EnemyTurn());
            }
            else if(lastAttacked == LastAttacked.ENEMY)
            {
                battleState = BattleState.PLAYERCHOICE;
                yield return StartCoroutine(PlayerChoice());
            }

        }

    }

    IEnumerator EnemyTurn()
    {
        // as before, decrease playerhealth by a fixed
        // amount of 10. You probably want to have some
        // more complex logic here.
        playerStatusHUD.SetHP(playerStats, enemyStats.strength);
        Debug.Log("Enemy attack");
        messageHUD.SetMessage("enemy attacked");

        // play attack animation by triggering
        // it inside the enemy animator
        //enemy.GetComponent<Animator>().SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        if (playerCharData.currHealth <= 0)
        {
            // if the player health drops to 0 
            // we have lost the battle...
            battleState = BattleState.LOST;
            yield return StartCoroutine(EndBattle());
        }
        else
        {
            if (lastAttacked == LastAttacked.NEWROUND)
            {
                // if the player health is still
                // above 0 when the turn finishes
                // it's our turn again!
                battleState = BattleState.PLAYERTURN;
                lastAttacked = LastAttacked.ENEMY;
                yield return StartCoroutine(PlayerTurn());
            }
            else if (lastAttacked == LastAttacked.PLAYER)
            {
                battleState = BattleState.PLAYERCHOICE;
                yield return StartCoroutine(PlayerChoice());
            }
        }
    }

    IEnumerator EndBattle()
    {
        // check if we won
        if (battleState == BattleState.WIN)
        {
            playerStats.SetGameManagerStats(playerStats);
            // you may wish to display some kind
            // of message or play a victory fanfare
            // here
            messageHUD.SetMessage("player wins!");
            Debug.Log("Win");
            yield return new WaitForSeconds(0.5f);
            LevelLoader.instance.LoadLevel("TestScene");
        }
        // otherwise check if we lost
        // You probably want to display some kind of
        // 'Game Over' screen to communicate to the 
        // player that the game is lost
        else if (battleState == BattleState.LOST)
        {
            playerStats.SetGameManagerStats(playerStats);
            // you may wish to display some kind
            // of message or play a sad tune here!
            messageHUD.SetMessage("player lost!");
            Debug.Log("Lose");
            yield return new WaitForSeconds(1);
            LevelLoader.instance.LoadLevel("TestScene");
        }
    }
}
