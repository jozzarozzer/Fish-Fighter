using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject playerControllerAction;
    public GameObject playerControllerFishing;

    public GameObject fishingArea;
    public GameObject actionArea;

    public GameObject UIReference;

    public GameObject patternSpawner;

    public GameObject enemyTracker;
    public EnemyTrackerController enemyTrackerScript;

    public IntVariable enemyCountVariable;

    public FishSO currentFish;

    bool combatEnding;

    [Header("Runtime Tools")]
    public IntVariable playerCurrency;
    public int currency;
    public bool setCurrency;

    public IntVariable playerHealth;
    public IntVariable playerHealthMax;
    public int healthDebug;
    public bool setHealth;

    public GunArrayVariable gunArray;
    public bool clearGunArray;
    public RodArrayVariable rodArray;
    public bool clearRodArray;


    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

	void Start ()
    {
        enemyCountVariable.value = 0;
        enemyTrackerScript = enemyTracker.GetComponent<EnemyTrackerController>();
	}
	

	void Update ()
    {       
        if (setCurrency)
        {
            playerCurrency.value = currency;
            setCurrency = false;
        }

        if (setHealth)
        {
            playerHealth.value = playerHealthMax.value;
            setHealth = false;
        }

        if (clearGunArray)
        {
            for (int i = 0; i < gunArray.value.Length; i++)
            {
                gunArray.value[i] = null;
            }
            playerControllerFishing.GetComponent<playerController>().gun = null;
            clearGunArray = false;
        }

        if (clearRodArray)
        {
            for (int i = 0; i < rodArray.value.Length; i++)
            {
                rodArray.value[i] = null;
            }
            playerControllerFishing.GetComponent<playerController>().fishingRod = null;
            clearRodArray = false;
        }

        if (enemyTrackerScript.currentlyCounting && enemyTrackerScript.enemyCount <= 0)
        {
            if (!combatEnding)
            {
                combatEnding = true;
                StartCoroutine(EndCombat());
            }
        }
	}

    public IEnumerator EndCombat()
    {
        enemyTrackerScript.currentlyCounting = false;
        AwardCurrency();
        yield return new WaitForSeconds(2);
        playerControllerAction.transform.position = actionArea.transform.position; //reset player position
        SwitchGame();
        combatEnding = false;
    }

    void AwardCurrency()
    {
        playerCurrency.value += currentFish.reward;
    }

    public void SwitchGame()
    {
        actionArea.SetActive(!actionArea.activeInHierarchy);
        fishingArea.SetActive(!fishingArea.activeInHierarchy);
    }
}
