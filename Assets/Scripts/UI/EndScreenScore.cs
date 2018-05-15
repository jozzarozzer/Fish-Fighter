using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenScore : MonoBehaviour {

    public Text thousands;
    public Text hundreds;
    public Text tens;
    public Text ones;

    public Text[] digitsText;

    private int[] modNum;
    private int[] digitsNum;

    public float lerpTime;
    public float timeBetweenDigits;

    public int debugScore;
    public int debugTotalDigits;
    public bool startScoreRoll = false;

    public float timeToFinish;

    private void Update()
    {
        if (startScoreRoll)
        {
            startScoreRoll = false;
            SetScore(debugScore, debugTotalDigits);
        }
    }

    private void Start()
    {
        digitsText[0] = ones;
        digitsText[1] = tens;
        digitsText[2] = hundreds;
        digitsText[3] = thousands;
    }

    public void Initialize()
    {
        for (int i = 0; i < digitsText.Length; i++)
        {
            digitsText[i].text = "-";
        }
    }

    public void SetScore(int score, int totalDigits)                                //input example start: (1453, 4)
    {
        Initialize();

        modNum = new int[totalDigits];
        digitsNum = new int[totalDigits];
       

        for (int i = 0; i < totalDigits; i++)                                       //4 loops
        {
            int mod = Mathf.RoundToInt( Mathf.Pow( 10, (i + 1) ) );

            modNum[i] = score % mod;                                                //[3, 53, 453, 1453]
            digitsNum[i] = i == 0 ? modNum[i] : modNum[i] - modNum[i - 1];          //[3, 50, 400, 1000]
            digitsNum[i] = digitsNum[i] / (Mathf.RoundToInt( Mathf.Pow(10, i) ));   //[3, 5, 4, 1]
        }

        timeToFinish = ((digitsNum.Length + 1) * lerpTime) + (totalDigits * timeBetweenDigits);

        StartCoroutine(lerpScoreText(totalDigits));        
    }

    IEnumerator lerpScoreText(int totalDigits)
    {
        for (int i = 0; i < totalDigits; i++)
        {
            for (int j = 0; j <= digitsNum[i]; j++)
            {
                digitsText[i].text = j.ToString();
                yield return new WaitForSeconds(lerpTime);
            }
            yield return new WaitForSeconds(timeBetweenDigits);
        }
    }
}
