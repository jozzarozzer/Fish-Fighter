using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    bool fishingLevelActive;

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
    int rewardFinal;

    bool combatEnding;
    public GameObject ribbonMask;
    public GameObject endScreenScore;
    public GameObject endScreen;
    public GameObject screenCover;

    public AudioMixer audioMixer;
    public AudioMixerSnapshot LPSnapshot;
    public AudioMixerSnapshot CleanSnapshot;

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

        fishingLevelActive = true;
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
        yield return new WaitForSeconds(0.5f);
        Vector3 screenStartPosition = screenCover.transform.position;

        //endscreen
        endScreen.SetActive(true);
        EndScreenScore endScreenScoreScript = endScreenScore.GetComponent<EndScreenScore>();       

        ribbonMask.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds((float)ribbonMask.GetComponent<PlayableDirector>().duration + 0.2f);

        

        rewardFinal = currentFish.reward + Mathf.RoundToInt(Random.Range(0f, 1f) * currentFish.reward * 0.10f);

        endScreenScoreScript.SetScore(rewardFinal, 4);
        yield return new WaitForSeconds(0.05f); //make sure the code on endScreenScore goes through enough
        yield return new WaitForSeconds(endScreenScoreScript.timeToFinish); //wait the amount of time it takes for the score code to finish        

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        //scene change
        screenCover.SetActive(true);
        Vector3 screenCoverTargetPos = new Vector3 (Screen.width/2, Screen.height/2, 0);
        for (int i = 0; i < 40; i++)
        {
            screenCover.transform.position = Vector3.Lerp(screenCover.transform.position, screenCoverTargetPos, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }        
        
        patternSpawner.GetComponent<PatternSpawnerController>().DestroyPattern(); //Remove the remaining terrain

        enemyTrackerScript.currentlyCounting = false;
        AwardCurrency();
        endScreenScoreScript.Initialize();
        endScreen.SetActive(false);

        playerControllerAction.transform.position = actionArea.transform.position; //reset player position
        SwitchGame();
        combatEnding = false;

        screenCoverTargetPos = new Vector3(0 - Screen.width / 1.5f, 0 - Screen.height / 1.5f, 0);
        for (int i = 0; i < 40; i++)
        {
            screenCover.transform.position = Vector3.Lerp(screenCover.transform.position, screenCoverTargetPos, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        screenCover.transform.position = screenStartPosition;
    }

    void AwardCurrency()
    {
        playerCurrency.value += rewardFinal;
    }

    public void SwitchGame()
    {
        actionArea.SetActive(!actionArea.activeInHierarchy);
        fishingArea.SetActive(!fishingArea.activeInHierarchy);
        
        if (fishingLevelActive)
        {
            LPSnapshot.TransitionTo(0.5f);
        }
        else
        {
            CleanSnapshot.TransitionTo(0.1f);
        }

        fishingLevelActive = !fishingLevelActive;
    }
}
