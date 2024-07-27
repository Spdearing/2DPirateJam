using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GenerateOrderTicket : MonoBehaviour
{
    [Header("Generated Spice Name")]
    [SerializeField] private string[] spiceNames;
    [SerializeField] private string[] randomSpiceNames;

    [Header("Array Of Classes")]
    [SerializeField] private Spice[] spices;

    [Header("Used Names")]
    [SerializeField] private List<Spice> spiceUsedInTicket;
    [SerializeField] private List<Spice> spiceSelected;

    [Header("Generated Spice Amounts Needed")]
    [SerializeField] private float[] spiceAmountsNeeded;

    [Header("Scripts")]
    [SerializeField] private SpiceManager spiceManager;

    [Header("Display Texts")]
    [SerializeField] private TMP_Text[] spiceNameText;
    [SerializeField] private TMP_Text[] spiceAmount;

    [Header("Buttons")]
    [SerializeField] private Button[] spiceButtons;

    [Header("TMP_Text")]
    [SerializeField] private TMP_Text displayText;

    // Start is called before the first frame update
    void Start()
    {

        displayText = GameManager.instance.ReturnSpiceDisplayNameText();

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

        spiceButtons = new Button[6];
        spiceButtons[0] = GameObject.Find("Ground Sage").GetComponent<Button>();
        spiceButtons[1] = GameObject.Find("Tarragon").GetComponent<Button>();
        spiceButtons[2] = GameObject.Find("Dill Pollen").GetComponent<Button>();
        spiceButtons[3] = GameObject.Find("Chervil").GetComponent<Button>();
        spiceButtons[4] = GameObject.Find("Spearmint").GetComponent<Button>();
        spiceButtons[5] = GameObject.Find("Sumac").GetComponent<Button>();


        for (int i = 0; i < spiceNameText.Length; i++)
        {
            spiceNameText[i].text = string.Empty;
        }

        for (int i = 0; i < spiceAmount.Length; i++)
        {
            spiceAmount[i].text = string.Empty;
        }

        foreach (Button button in spiceButtons)
        {
            button.onClick.AddListener(() => DisplaySelectedSpice(button));
        }

        CreateTheList();

        GenerateOrder();


    }

    private void DisplaySelectedSpice(Button clickedButton)
    {
        string spiceName = clickedButton.gameObject.name;
        Debug.Log(spiceName);

        if (spiceUsedInTicket.Any(spice => spice.nameOfSpice == spiceName))
        {
            SelectSpice(spiceName);

            if (displayText != null)
            {
                displayText.text = spiceName + " has been selected";
            }
            else
            {
                Debug.LogError("Display text is null.");
            }
        }
        else
        {
            if (displayText != null)
            {
                displayText.text = "You do not need that spice right now";
            }
            else
            {
                Debug.LogError("Display text is null.");
            }
        }
    }


    public void GenerateOrder()
    {
        int randomSpiceIndex = Random.Range(2, 4);
        
        randomSpiceNames = new string[randomSpiceIndex];

        for (int i = 0; i < randomSpiceIndex ;i++)
        {
            int randomSpiceName = Random.Range(0, spiceNames.Length);

            Spice randomSpiceHolder = spices[randomSpiceName];
            
            while(spiceUsedInTicket.Contains(randomSpiceHolder))
            {
                randomSpiceName = Random.Range(0, spiceNames.Length);
                randomSpiceHolder = spices[randomSpiceName];
            }

            spiceUsedInTicket.Add(randomSpiceHolder);
            
            randomSpiceNames[i] = spices[randomSpiceName].nameOfSpice;

            spiceNameText[i].text = randomSpiceNames[i];

            float randomSpiceAmount = spices[randomSpiceName].randomMaxAmount;

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

    public void SelectSpice(string spiceName)
    {
        for(int i = 0; i < spices.Length; i++ )
        {
            if (spiceName == spices[i].nameOfSpice)
            {
                spiceSelected.Add(spices[i]);
            }
            else
            {
                displayText.text = "You do not need that spice right now";
            }
        }
    }

    public void ChangeSpiceAmount()
    {
        
    }

    [System.Serializable]
    public class Spice
    {
        public string nameOfSpice;
        public float startingSpiceAmount;
        public float randomMaxAmount;

        public Spice(string name, float startingAmount, float maxAmount)
        {
            nameOfSpice = name;
            startingSpiceAmount = startingAmount;
            randomMaxAmount = maxAmount;
        }
    }

    [System.Serializable]
    public class GroundSage : Spice
    {
        public GroundSage() : base("Ground Sage", 0 , Random.Range(2000, 4001)) 
        {
        
        }
    }

    [System.Serializable]
    public class Tarragon : Spice
    {
        public Tarragon() : base("Tarragon", 0 ,  Random.Range(2000, 4001))
        { 
        
        }
    }

    [System.Serializable]
    public class DillPollen : Spice
    {
        public DillPollen() : base("Dill Pollen", 0 , Random.Range(2000, 4001)) 
        {
        
        }
    }

    [System.Serializable]
    public class Chervil : Spice
    {
        public Chervil() : base("Chervil", 0 , Random.Range(2000, 4001))
        {
        
        }
    }

    [System.Serializable]
    public class Spearmint : Spice
    {
        public Spearmint() : base("Spearmint", 0 , Random.Range(2000, 4001)) 
        {
        
        }
    }

    [System.Serializable]
    public class Sumac : Spice
    {
        public Sumac() : base("Sumac", 0 , Random.Range(2000, 4001)) 
        { 
        
        }
    }

}
