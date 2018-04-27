using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    GameObject UI;

    public LineRenderer lineRenderer;

    public GameObject lineStart;

    public LayerMask groundMask;
    Vector3 mousePosition; //position that the mouse ray collides with the ground.

    public GameObject caster;
    public GameObject rodModel;
    public GameObject gameController;
    GameController gameControllerScript;
    UIReference UIReferenceScript;

    public float castStrength;
    public float castModifier;

    Vector3 castDestination;
    Vector3 castDirection;
    Vector3 positionLevelled;
    Vector3 initialRotation;
    Vector3 casterStartPosition;
    Vector3 initialPosition;

    public Camera cam;

    ZoneSO waterZone;

    FishSO fishCaught;

    public FishingRodSO fishingRod;
    public RodArrayVariable playerRods;

    public GunSO gun;
    public GunArrayVariable playerGuns;

    public BoolVariable UIopen;

    public AudioSource audioSource;

    bool canCast;
    bool canRotate;

	void Start ()
    {
        canCast = true;
        canRotate = true;
        gameControllerScript = gameController.GetComponent<GameController>();
        UIReferenceScript = gameControllerScript.UIReference.GetComponent<UIReference>();

        initialRotation = transform.eulerAngles;
        initialPosition = transform.position;
        casterStartPosition = caster.transform.localPosition;

	}
	
	void Update ()
    {

        lineRenderer.SetPosition(0, lineStart.transform.position);
        lineRenderer.SetPosition(1, caster.transform.position);

        DetermineMousePosition();

        DetermineRod();

        DetermineGun();

		if (Input.GetMouseButtonDown(0) && canCast && !UIopen.value)
        {
            if (fishingRod != null && gun != null)
            {
                StartCoroutine(DetermineCast());
            }
            if (fishingRod == null)
            {
                StartCoroutine(NoRod());                
            }
            if (gun == null)
            {
                StartCoroutine(NoGun());
            }
        }

	}

    IEnumerator NoRod()
    {
        UIReferenceScript.noRodText.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        UIReferenceScript.noRodText.SetActive(false);
    }

    IEnumerator NoGun()
    {
        UIReferenceScript.noGunText.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        UIReferenceScript.noGunText.SetActive(false);
    }

    void DetermineRod()
    {
        if (playerRods.value[0] != null)
        {
            for (int i = 0; i < playerRods.value.Length; i++)
            {
                if (fishingRod != null)
                {
                    if (playerRods.value[i] != null && playerRods.value[i].level > fishingRod.level)
                    {
                        fishingRod = playerRods.value[i];
                    }
                }
                else
                {
                    fishingRod = playerRods.value[0];
                }

            }
        }

        if (fishingRod != null)
        {
            castStrength = fishingRod.castStrength;
        }
    }

    void DetermineGun()
    {
        if (playerGuns.value[0] != null)
        {
            for (int i = 0; i < playerGuns.value.Length; i++)
            {
                if (gun != null)
                {
                    if (playerGuns.value[i] != null && playerGuns.value[i].level > gun.level)
                    {
                        gun = playerGuns.value[i];
                    }
                }
                else
                {
                    gun = playerGuns.value[0];
                }

            }
        }
    }

    IEnumerator DetermineCast()
    {
        bool canHitSlider = false;

        canCast = false;
        canRotate = false;
        positionLevelled = SetY(transform.position, mousePosition.y); //player position with Y put to the level that the mouse collides with

        Vector3 mouseZeroToPlayer = mousePosition - positionLevelled;
        castDirection = mouseZeroToPlayer.normalized;

        float castingTime = 10;
        
        float loopTime = 0.01f;
        bool modUp = true;
        var sliderObj = gameControllerScript.UIReference.GetComponent<UIReference>().castStrengthSlider;
        var slider = sliderObj.GetComponent<Slider>();
        castModifier = 0;
        slider.value = 0;
        sliderObj.SetActive(true);

        for (float t = 0; t < castingTime; t += loopTime)
        {
            if (modUp)
            {
                if (castModifier < 1)
                {
                    castModifier += 0.03f;
                    slider.value += 0.03f;
                }
                else modUp = false;
            }

            if (!modUp)
            {
                if (castModifier > 0)
                {
                    castModifier -= 0.03f;
                    slider.value -= 0.03f;
                }
                else modUp = true;
            }


            if (Input.GetMouseButtonUp(0))
            {
                canHitSlider = true;
            }

            if (Input.GetMouseButtonDown(0) && canHitSlider)
            {
                yield return new WaitForSeconds(0.5f);
                sliderObj.SetActive(false);
                break;
            }

            yield return new WaitForSeconds(loopTime);
        }
        
        StartCoroutine(Cast());      
    }

    IEnumerator Cast()
    {
        castDestination = positionLevelled + (castDirection * castStrength * castModifier * fishingRod.level);

        Vector3 castDifference = castDestination - caster.transform.position;
        int loopCount = 10;

        audioSource.Play();
        for (int i = 0; i < loopCount; i++)
        {
            //caster.transform.position = Vector3.Lerp(caster.transform.position, castDestination, 0.1f);
            caster.transform.position += castDifference / loopCount;
            yield return new WaitForSeconds(0.01f);
        }
        

        waterZone = CheckZone(); //checks for the ZoneSO of the water zone that the mouse ray collided with and then set waterZone to it

        SpawnFish();

        if (fishCaught != null)
        {
            StartCoroutine(Transition());
        }
        else
        {
            canCast = true;
            canRotate = true;
            caster.transform.localPosition = casterStartPosition;
            audioSource.Stop(); //stops the casting sound
            transform.position = initialPosition;
        }
    }

    IEnumerator Transition()
    {
        UIReferenceScript.fishCaught.SetActive(true);
        yield return new WaitForSeconds(1f);
        UIReferenceScript.fishName.SetActive(true);

        if (fishCaught.quality == Quality.trash)
        {
            UIReferenceScript.fishName.GetComponent<Text>().color = Color.white;
        }
        else if (fishCaught.quality == Quality.common)
        {
            UIReferenceScript.fishName.GetComponent<Text>().color = Color.green;
        }
        else if (fishCaught.quality == Quality.rare)
        {
            UIReferenceScript.fishName.GetComponent<Text>().color = Color.blue;
        }
        else if (fishCaught.quality == Quality.epic)
        {
            UIReferenceScript.fishName.GetComponent<Text>().color = Color.magenta;
        }
        else if (fishCaught.quality == Quality.legendary)
        {
            UIReferenceScript.fishName.GetComponent<Text>().color = Color.yellow;
        }

        UIReferenceScript.fishName.GetComponent<Text>().text = fishCaught.fishType;
        yield return new WaitForSeconds(2);

        UIReferenceScript.fishCaught.SetActive(false);
        UIReferenceScript.fishName.SetActive(false);
        UIReferenceScript.fishName.GetComponent<Text>().text = null;

        Vector3 moveDifference = castDestination - transform.position;
        int loopCount = 20;

        audioSource.Play();
        for (int i = 0; i < loopCount; i++)
        {           
            transform.position += moveDifference / loopCount;
            transform.LookAt(castDestination);
            rodModel.transform.LookAt(castDestination);
            caster.transform.position = castDestination;
            yield return new WaitForSeconds(0.005f);
        }

        canCast = true;
        canRotate = true;
        caster.transform.localPosition = casterStartPosition;
        audioSource.Stop(); //stops the casting sound
        transform.position = initialPosition;

        gameController.GetComponent<GameController>().SwitchGame();

        float randomValue = Random.value;
        int roundedValue = Mathf.RoundToInt(randomValue * (fishCaught.patternArray.Length - 1));
        GameObject chosenPattern = fishCaught.patternArray[roundedValue];

        gameControllerScript.patternSpawner.GetComponent<PatternSpawnerController>().SpawnPattern(chosenPattern);

        fishCaught = null; //resets fish caught
    }

    void SpawnFish()
    {
        float randomValue = Random.value;
        float randomFishFloat = randomValue * (waterZone.fishArray.Length - 1);
        int randomFishInt = Mathf.RoundToInt(randomFishFloat);
        fishCaught = waterZone.fishArray[randomFishInt];
        gameControllerScript.currentFish = fishCaught;
    }

    ZoneSO CheckZone()
    {
        Vector3 rayStart = SetY(castDestination, castDestination.y + 10);
        RaycastHit hit;
        if (Physics.Raycast(rayStart, Vector3.down, out hit))
        {
            if (hit.collider.gameObject.GetComponent<WaterZone>())
            {
                var waterZoneScript = hit.collider.gameObject.GetComponent<WaterZone>();
                return waterZoneScript.zone;
            }
            else
            {
                print("failed checkZone second");
                return null;
            }
        }
        else
        {
            print("failed checkZone first");
            return null;
        }
    }

    void DetermineMousePosition() //determines the mouse position as a ray from screen to ground. 
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); //cast a ray from the camera based on the mouse position
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000, groundMask);

        mousePosition = hit.point;

        if (canRotate)
        {
            Vector3 hitPointRaised = new Vector3(hit.point.x, transform.position.y, hit.point.z); //hit point raised to y of player
            transform.LookAt(hitPointRaised);
            transform.eulerAngles += initialRotation;

            rodModel.transform.LookAt(hit.point);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(positionLevelled, castDestination);
    }

    Vector3 SetY(Vector3 input, float Y)
    {
        return new Vector3(input.x, Y, input.z);
    }
}
