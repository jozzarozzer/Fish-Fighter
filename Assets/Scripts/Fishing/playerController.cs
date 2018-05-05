using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [Header("Game Control")]
    public GameObject gameController;
    GameController gameControllerScript;


    [Header("UI")]
    GameObject UI;
    UIReference UIReferenceScript;
    public BoolVariable UIopen;


    [Header("Player")]
    public GameObject rodModel;
    public FishingRodSO fishingRod;
    public RodArrayVariable playerRods;
    public TrailRenderer rodTrail;

    [Space]

    public LineRenderer fishingLine;
    public GameObject lineStart;
    public float fishingLineInitialWidth;

    [Space]

    public GameObject caster;
    public GameObject casterRodPosition;
    bool casterOnRod;

    [Space]

    public GunSO gun;
    public GunArrayVariable playerGuns;

    [Space]

    bool canRotate;


    [Header("Casting")]
    bool canCast;

    public float castStrength;
    public float castModifier;
    public float castAngle;

    [Space]

    Vector3 castDestination;
    Vector3 castDirection;
    Vector3 castDifference;
    Vector3 casterStartPosition;

    [Space]

    Vector3 positionLevelled;
    Vector3 initialRotation;   
    Vector3 initialPosition;
    
    [Space]

    public LineRenderer arcIndicator;

    [Space]

    public LayerMask groundMask;
    Vector3 mousePosition; //position that the mouse ray collides with the ground.

    ZoneSO waterZone;

    FishSO fishCaught;

    [Header("Camera")]
    public Camera cam;
    public Vector3 camOffset;
    public Vector3 camTarget;

    [Header("Audio")]
    public AudioSource audioSource;

    void Start ()
    {
        canCast = true;
        canRotate = true;
        casterOnRod = true;

        rodTrail.enabled = false;
        gameControllerScript = gameController.GetComponent<GameController>();
        UIReferenceScript = gameControllerScript.UIReference.GetComponent<UIReference>();

        initialRotation = transform.eulerAngles;
        initialPosition = transform.position;
        casterStartPosition = caster.transform.localPosition;

        camOffset = cam.transform.position - transform.position;
        camTarget = cam.transform.position - camOffset;

        fishingLineInitialWidth = fishingLine.widthMultiplier;
    }

    void Update()
    {
        if (casterOnRod)
        {
            caster.transform.position = casterRodPosition.transform.position;
        }

        SetFishingLine();

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

        LerpCam();

    }

    void LerpCam()
    {
        Vector3 currentFollowedPosition = transform.position - camOffset;
        float distanceToTarget = (camTarget - currentFollowedPosition).magnitude;

        cam.transform.position = Vector3.Lerp(cam.transform.position, camTarget + camOffset, 0.2f);
    }

    void SetFishingLine()
    {
        fishingLine.SetPosition(0, lineStart.transform.position);
        fishingLine.SetPosition(1, caster.transform.position);

        if (fishingRod != null)
        {
            fishingLine.widthMultiplier = fishingRod.level * fishingLineInitialWidth;
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
                    //castAngle
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

            castAngle = castModifier * Mathf.PI / 2;
            castDestination = positionLevelled + (castDirection * castStrength * castModifier * fishingRod.level);
            castDifference = castDestination - caster.transform.position;


            Vector3 castDestinationMax = positionLevelled + (castDirection * castStrength * 1 * fishingRod.level);
            Vector3 camWTVP = cam.WorldToViewportPoint(castDestinationMax);
            if (!(camWTVP.x > 0 && camWTVP.y > 0 && camWTVP.x < 1 && camWTVP.y < 1))
            {               
                camTarget = castDestination;
            }

            arcIndicator.enabled = true;
            ArcIndicator();

            yield return new WaitForSeconds(loopTime);
        }

        arcIndicator.enabled = false;
        StartCoroutine(SwingRod());   
    }


    void ArcIndicator()
    {
        //Vector3 castDestinationMax = positionLevelled + (castDirection * castStrength * fishingRod.level);
        //Vector3 castDifferenceMax = castDestinationMax - caster.transform.position;

        int loopCount = Mathf.RoundToInt(castDifference.magnitude); //=100
        Vector3 linePos;
        Vector3 startPos = caster.transform.position;

        arcIndicator.positionCount = loopCount;
        arcIndicator.endColor = new Color(1,(castModifier -1) * -1, (castModifier - 1) * -1, 1);

        for (int i = 0; i < loopCount; i++)
        {
            linePos = i * (castDifference / loopCount);
            float yArc = -(i * i) + (i * (loopCount - 1));
            Vector3 pointPos = startPos + new Vector3(linePos.x, linePos.y + yArc/100, linePos.z);
            arcIndicator.SetPosition(i, pointPos);
            
        }
    }

    IEnumerator SwingRod()
    {
        float targetAngle = -150;
        float angleDifference = targetAngle - 0;
        float loopCount = 30;

        for (int j = 0; j < loopCount; j++)
        {
            float rotationX = (angleDifference / loopCount);
            Vector3 rotationVector = new Vector3(rotationX, 0, 0);
            rodModel.transform.Rotate(rotationVector);
            yield return new WaitForSeconds(0.03f/loopCount);
        }

        targetAngle = 0;
        angleDifference = targetAngle - -150;
        bool hasCast = false;

        float loopCount2 = loopCount / 5;

        for (int k = 0; k < loopCount2; k++)
        {
            rodTrail.enabled = true;
            float rotationX = (angleDifference / loopCount2);
            Vector3 rotationVector = new Vector3(rotationX, 0, 0);
            rodModel.transform.Rotate(rotationVector);
            if (k >= loopCount2/3 && !hasCast)
            {
                hasCast = true;
                castDifference = castDestination - caster.transform.position;
                StartCoroutine(Cast());                
                print("cast");
            }
            yield return new WaitForSeconds(0.03f/loopCount2);
        }
        

    }

    IEnumerator Cast()
    {
        print("cast start");
        casterOnRod = false;
        //Vector3 castDestinationMax = positionLevelled + (castDirection * castStrength * fishingRod.level);
        //Vector3 castDifferenceMax = castDestinationMax - caster.transform.position;

        int loopCount = Mathf.RoundToInt(castDifference.magnitude); //=100
        Vector3 linePos; //keeps track of the position the caster would be in if it were not adding the quadratic lower down.
        Vector3 startPos = caster.transform.position;

        //audioSource.Play();
        for (int i = 0; i < loopCount; i++)
        {
            linePos = i * (castDifference / loopCount);
            float yArc = -(i * i) + (i * (loopCount-1));
            caster.transform.position = startPos + new Vector3(linePos.x, linePos.y + yArc/100, linePos.z);

            yield return new WaitForSeconds(0.01f);
        }
        

        waterZone = CheckZone(); //checks for the ZoneSO of the water zone that the mouse ray collided with and then set waterZone to it
        if (waterZone == null)
        {
            ReInitialize();
            yield break;
        }

        SpawnFish();
        if (fishCaught == null)
        {
            ReInitialize();
            yield break;
        }

        StartCoroutine(Transition());
        print("cast end");
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
            //rodModel.transform.LookAt(castDestination);
            caster.transform.position = castDestination;
            yield return new WaitForSeconds(0.005f);
        }

        ReInitialize();

        gameController.GetComponent<GameController>().SwitchGame();

        float randomValue = Random.value;
        int roundedValue = Mathf.RoundToInt(randomValue * (fishCaught.patternArray.Length - 1));
        GameObject chosenPattern = fishCaught.patternArray[roundedValue];

        gameControllerScript.patternSpawner.GetComponent<PatternSpawnerController>().SpawnPattern(chosenPattern);

        fishCaught = null; //resets fish caught
    }

    void ReInitialize()
    {
        canCast = true;
        canRotate = true;
        casterOnRod = true;
        rodTrail.enabled = false;
        caster.transform.localPosition = casterStartPosition;
        audioSource.Stop(); //stops the casting sound
        transform.position = initialPosition;
        camTarget = transform.position;
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
        Vector3 rayStart = SetY(caster.transform.position, caster.transform.position.y + 10);
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
                Debug.Log("failed checkZone second");
                return null;
            }
        }
        else
        {
            Debug.Log("failed checkZone first");
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

            //rodModel.transform.LookAt(hit.point);
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
