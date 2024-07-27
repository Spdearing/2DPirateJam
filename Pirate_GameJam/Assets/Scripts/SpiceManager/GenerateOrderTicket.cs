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

    [Header("Array Of Classes")]
    [SerializeField] private Spice[] spices;

    [Header("Used Names")]
    [SerializeField] private List<Spice> spiceUsed;

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

        spices = new Spice[6];
        spices[0] = new GroundSage();
        spices[1] = new Tarragon();
        spices[2] = new DillPollen();
        spices[3] = new Chervil();
        spices[4] = new Spearmint();
        spices[5] = new Sumac();


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

            Spice randomSpiceHolder = spices[randomSpiceName];
            
            while(spiceUsed.Contains(randomSpiceHolder))
            {
                randomSpiceName = Random.Range(0, spiceNames.Length);
                randomSpiceHolder = spices[randomSpiceName];
            }

            spiceUsed.Add(randomSpiceHolder);
            
            randomSpiceNames[i] = spices[randomSpiceName].nameOfSpice;

            spiceNameText[i].text = randomSpiceNames[i];

            float randomSpiceAmount = spices[randomSpiceName].randomAmount;

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

    [System.Serializable]
    public class Spice
    {
        public string nameOfSpice;
        public float randomAmount;

        public Spice(string name, float amount)
        {
            nameOfSpice = name;
            randomAmount = amount;
        }
    }

    [System.Serializable]
    public class GroundSage : Spice
    {
        public GroundSage() : base("Ground Sage", Random.Range(2000, 4001)) { }
    }

    [System.Serializable]
    public class Tarragon : Spice
    {
        public Tarragon() : base("Tarragon", Random.Range(2000, 4001)) { }
    }

    [System.Serializable]
    public class DillPollen : Spice
    {
        public DillPollen() : base("Dill Pollen", Random.Range(2000, 4001)) { }
    }

    [System.Serializable]
    public class Chervil : Spice
    {
        public Chervil() : base("Chervil", Random.Range(2000, 4001)) { }
    }

    [System.Serializable]
    public class Spearmint : Spice
    {
        public Spearmint() : base("Spearmint", Random.Range(2000, 4001)) { }
    }

    [System.Serializable]
    public class Sumac : Spice
    {
        public Sumac() : base("Sumac", Random.Range(2000, 4001)) { }
    }

}
