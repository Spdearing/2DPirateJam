using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GenerateOrderTicket : MonoBehaviour
{
    [Header("Generated Spice Name")]
    [SerializeField] private string[] spiceNames;
    [SerializeField] private string[] randomSpiceNames;

    [Header("Used Names")]
    [SerializeField] private List<string> spiceNamesUsed;

    [Header("Generated Spice Amounts Needed")]
    [SerializeField] private float[] spiceAmountsNeeded;

    [Header("Scripts")]
    [SerializeField] private SpiceManager spiceManager;

    [Header("Display Texts")]
    [SerializeField] private TMP_Text[] spiceNameText;
    [SerializeField] private TMP_Text[] spiceAmount;

    // Start is called before the first frame update
    void Start()
    {
        spiceManager = GameManager.instance.ReturnSpiceManager();
        spiceNames = new string[6];
        spiceNameText = new TMP_Text[3];
        spiceNameText[0] = GameObject.Find("SpiceOne").GetComponent<TMP_Text>();
        spiceNameText[1] = GameObject.Find("SpiceTwo").GetComponent<TMP_Text>();
        spiceNameText[2] = GameObject.Find("SpiceThree").GetComponent<TMP_Text>();

        spiceAmount = new TMP_Text[3];
        spiceAmount[0] = GameObject.Find("SpiceAmountOne").GetComponent<TMP_Text>();
        spiceAmount[1] = GameObject.Find("SpiceAmountTwo").GetComponent<TMP_Text>();
        spiceAmount[2] = GameObject.Find("SpiceAmountThree").GetComponent<TMP_Text>();


        for (int i = 0; i < spiceNameText.Length; i++)
        {
            spiceNameText[i].text = string.Empty;
        }

        for (int i = 0; i < spiceAmount.Length; i++)
        {
            spiceAmount[i].text = string.Empty;
        }

        CreateTheList();

        GenerateOrder();


    }

    public void GenerateOrder()
    {
        int randomSpiceIndex = Random.Range(2, 4);
        
        randomSpiceNames = new string[randomSpiceIndex];

        for (int i = 0; i < randomSpiceIndex ;i++)
        {
            int randomSpiceName = Random.Range(0, spiceNames.Length);

            string randomSpiceNameHolder = spiceNames[randomSpiceName];
            
            while(spiceNamesUsed.Contains(randomSpiceNameHolder))
            {
                randomSpiceName = Random.Range(0, spiceNames.Length);
                randomSpiceNameHolder = spiceNames[randomSpiceName];
            }

            spiceNamesUsed.Add(randomSpiceNameHolder);
            
            randomSpiceNames[i] = randomSpiceNameHolder;

            spiceNameText[i].text = randomSpiceNames[i];

            int randomSpiceAmount = Random.Range(2000, 4001);

            spiceAmount[i].text = randomSpiceAmount.ToString(); 
        }
    }

    void CreateTheList()
    {
        for (int i = 0; i < spiceNames.Length; i++)
        {
            spiceNames[i] = spiceManager.ReturnSpiceNames(i);
        }
    }
}
