using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerateOrderTicket : MonoBehaviour
{
    [Header("Generated Spice Name")]
    [SerializeField] private string[] spiceNames;
    [SerializeField] private string[] randomSpiceNames;

    [Header("Floats")]
    [SerializeField] private float spicePourAmount;
    [SerializeField] private float pouredAmount;
    [SerializeField] private float currentTickets;
    [SerializeField] private float ticketsCompleted;
    [SerializeField] private float orderRotations;
    [SerializeField] private float playerScore;

    [Header("Array Of Classes")]
    [SerializeField] private Spice[] spices;

    [Header("Used Names")]
    [SerializeField] private List<Spice> spiceUsedInTicket;
    [SerializeField] private List<Spice> spiceSelected;

    [Header("Generated Spice Amounts Needed")]
    [SerializeField] private float[] spiceAmountsNeeded;

    [Header("Scripts")]
    [SerializeField] private SpiceManager spiceManager;
    [SerializeField] private SpiceSpawner spiceSpawner;

    [Header("Display Texts")]
    [SerializeField] private TMP_Text[] spiceNameText;
    [SerializeField] private TMP_Text[] spiceNameText2;
    [SerializeField] private TMP_Text[] spiceAmountText;

    [Header("Buttons")]
    [SerializeField] private Button[] spiceButtons;

    [Header("Bools")]
    [SerializeField] private bool pourButtonPressed;
    [SerializeField] private bool correctSelection;
    [SerializeField] private bool successfulRatio;
    [SerializeField] private bool failedRatio;
    [SerializeField] private bool orderSubmitted;
    [SerializeField] private bool spiceHasBeenUsed;

    private Dictionary<string, TMP_Text> spiceAmountDictionary;

    void Start()
    {
        Initialize();
        CreateTheList();
        CreateSpiceAmountDictionary();
        GenerateOrder();
    }

    private void Initialize()
    {
        spiceHasBeenUsed = false;
        playerScore = GameManager.instance.ReturnPlayerScore();
        orderSubmitted = false;
        successfulRatio = false;
        failedRatio = false;
        correctSelection = false;
        spiceSpawner = GameManager.instance.ReturnSpiceSpawner();

        InitializeSpiceNames();
        InitializeSpiceTexts();
        InitializeSpices();
        InitializeButtons();

        ClearTexts();
    }

    private void InitializeSpiceNames()
    {
        spiceNames = new string[8];
    }

    private void InitializeSpiceTexts()
    {
        spiceNameText = new TMP_Text[3];
        spiceNameText[0] = GameObject.Find("SpiceOne").GetComponent<TMP_Text>();
        spiceNameText[1] = GameObject.Find("SpiceTwo").GetComponent<TMP_Text>();
        spiceNameText[2] = GameObject.Find("SpiceThree").GetComponent<TMP_Text>();

        spiceNameText2 = new TMP_Text[3];
        spiceNameText2[0] = GameObject.Find("SpiceOne2").GetComponent<TMP_Text>();
        spiceNameText2[1] = GameObject.Find("SpiceTwo2").GetComponent<TMP_Text>();
        spiceNameText2[2] = GameObject.Find("SpiceThree2").GetComponent<TMP_Text>();

        spiceAmountText = new TMP_Text[3];
        spiceAmountText[0] = GameObject.Find("SpiceAmountOne").GetComponent<TMP_Text>();
        spiceAmountText[1] = GameObject.Find("SpiceAmountTwo").GetComponent<TMP_Text>();
        spiceAmountText[2] = GameObject.Find("SpiceAmountThree").GetComponent<TMP_Text>();

    }

    private void InitializeSpices()
    {
        spices = new Spice[8];
        spices[0] = new Palfnir();
        spices[1] = new Brikkol();
        spices[2] = new Tenalc();
        spices[3] = new Gremlock();
        spices[4] = new Sewort();
        spices[5] = new Ezethaxis();
        spices[6] = new Aidleqar_Sap();
        spices[7] = new Ully();
    }

    private void InitializeButtons()
    {
        spiceButtons = new Button[8];
        spiceButtons[0] = GameObject.Find("Palfnir").GetComponent<Button>();
        spiceButtons[1] = GameObject.Find("Brikkol").GetComponent<Button>();
        spiceButtons[2] = GameObject.Find("Tenalc").GetComponent<Button>();
        spiceButtons[3] = GameObject.Find("Gremlock").GetComponent<Button>();
        spiceButtons[4] = GameObject.Find("Sewort").GetComponent<Button>();
        spiceButtons[5] = GameObject.Find("Ezethaxis").GetComponent<Button>();
        spiceButtons[6] = GameObject.Find("Aidleqar Sap").GetComponent<Button>();
        spiceButtons[7] = GameObject.Find("Ully").GetComponent<Button>();

        foreach (Button button in spiceButtons)
        {
            button.onClick.AddListener(() => DisplaySelectedSpice(button));
            button.interactable = true;
        }
    }

    private void ClearTexts()
    {
        foreach (var text in spiceNameText)
        {
            text.text = string.Empty;
        }

        foreach (var text in spiceAmountText)
        {
            text.text = string.Empty;
        }
    }

    private void ResetRatioCheck()
    {
        successfulRatio = false;
        failedRatio = false;
    }

    private void CreateSpiceAmountDictionary()
    {
        spiceAmountDictionary = new Dictionary<string, TMP_Text>();

        for (int i = 0; i < spiceNameText.Length; i++)
        {
            if (!string.IsNullOrEmpty(spiceNameText[i].text))
            {
                spiceAmountDictionary[spiceNameText[i].text] = spiceAmountText[i];
            }
        }
    }

    

    private void DisplaySelectedSpice(Button clickedButton)
    {
        if(orderSubmitted == false)
        {
            string spiceName = clickedButton.gameObject.name;

            if (spiceSpawner.ReturnPouringBool() == false && spiceSpawner.ReturnStillPouringBool() == false)
            {
                if (spiceUsedInTicket.Any(spice => spice.nameOfSpice == spiceName))
                {
                    spiceHasBeenUsed = false;
                    clickedButton.interactable = false;
                    SelectSpice(spiceName);
                }
            }
        }
    }

    public void GenerateOrder()
    {
        correctSelection = false;
        orderSubmitted = false;
        InitializeSpices();
        ResetRatioCheck();
        spiceUsedInTicket.Clear();
        int randomSpiceIndex = Random.Range(2, 4);

        if(randomSpiceIndex == 2)
        {
            spiceNameText[2].text = string.Empty;
            spiceNameText2[2].text = string.Empty;
            spiceAmountText[2].text = string.Empty;
        }

        if (randomSpiceIndex > spices.Length)
        {
            Debug.LogError("Not enough unique spices available to generate the order.");
            return;
        }

        randomSpiceNames = new string[randomSpiceIndex];

        //ClearSpiceUsedInTicketIfNecessary();

        for (int i = 0; i < randomSpiceIndex; i++)
        {
            int randomSpiceName = GetUniqueRandomSpiceName();
            Spice randomSpiceHolder = spices[randomSpiceName];

            spiceUsedInTicket.Add(randomSpiceHolder);
            randomSpiceNames[i] = randomSpiceHolder.nameOfSpice;
            spiceNameText[i].text = randomSpiceNames[i];
            spiceNameText2[i].text = randomSpiceNames[i];

            float randomSpiceAmount = randomSpiceHolder.startingSpiceAmount;
            spiceAmountText[i].color = new Color(0.8113208f, 0.3559096f, 0.3559096f, 1f);
            spiceAmountText[i].text = randomSpiceAmount.ToString() + "%";
            spiceAmountDictionary[randomSpiceNames[i]] = spiceAmountText[i];
        }
    }

    private int GetUniqueRandomSpiceName()
    {
        const int maxAttempts = 100; // Maximum attempts to find a unique spice
        int attempts = 0;

        int randomSpiceName = Random.Range(0, spiceNames.Length);
        Spice randomSpiceHolder = spices[randomSpiceName];

        while (spiceUsedInTicket.Contains(randomSpiceHolder))
        {
            randomSpiceName = Random.Range(0, spiceNames.Length);
            randomSpiceHolder = spices[randomSpiceName];
            attempts++;

            if (attempts >= maxAttempts)
            {
                Debug.LogError("Failed to find a unique spice after maximum attempts.");
                break;
            }
        }

        return randomSpiceName;
    }


    void CreateTheList()
    {
        for (int i = 0; i < spiceNames.Length; i++)
        {
            spiceNames[i] = spices[i].nameOfSpice;
        }
    }

    public void SelectSpice(string spiceName)
    {
        Spice selectedSpice = spices.FirstOrDefault(spice => spice.nameOfSpice == spiceName);

        if (selectedSpice != null && spiceUsedInTicket.Contains(selectedSpice))
        {
            correctSelection = true;
            spiceSelected.Insert(0, selectedSpice);
            spiceSpawner.ChangeSpiceParticleColor(spiceSelected[0]);
        }
        else
        {
            correctSelection = false;
            Debug.Log("Spice was not put in the list");
        }
    }

    public void PourSpice()
    {
        if (spiceSelected.Count > 0)
        {
            spiceSelected[0].startingSpiceAmount++;
            UpdateSpiceAmount(spiceSelected[0].nameOfSpice);
        }
    }

    IEnumerator EndOfRound()
    {
        yield return new WaitForSeconds(1f);
        GenerateOrder();
    }

    public void GeneratePurity()
    {
        if (spiceSpawner.ReturnPouringBool() == false && spiceSpawner.ReturnStillPouringBool() == false)
        {
            CheckOverFlow();

            float totalPurity = spiceUsedInTicket.Sum(spice => spice.spicePurity);
            float averagePurity = Mathf.FloorToInt(totalPurity / spiceUsedInTicket.Count);


            SubmitOrder(averagePurity);
            UpdatePlayerScore(averagePurity);
            StartCoroutine(EndOfRound());
            InitializeButtons();
        }  
    }

    public void CheckOverFlow()
    {
        for(int i = 0; i < spiceUsedInTicket.Count; i++)
        {
            if (spiceUsedInTicket[i].spicePurity > 100.0f)
            {
                float spiceOverflow = spiceUsedInTicket[i].spicePurity - 100.0f;

                if (spiceOverflow < 100.0f)
                {
                    spiceOverflow = 100.0f - spiceOverflow;
                    spiceUsedInTicket[i].spicePurity = spiceOverflow;
                    Debug.Log("Spice Over flow " + spiceOverflow + " % for " + spiceUsedInTicket[i].nameOfSpice);
                }
                else
                {
                    spiceOverflow = 0f;
                    spiceUsedInTicket[i].spicePurity = spiceOverflow;
                }
            }
        }
    }

    void SubmitOrder(float averagePurity)
    {
        orderSubmitted = true;

        if (averagePurity > 75.0f)
        {
            successfulRatio = true;
            currentTickets++;
            ticketsCompleted = currentTickets;
        }
        else
        {
            failedRatio = true;
            ticketsCompleted = currentTickets;
        }
    }

    void UpdateSpiceAmount(string spiceName)
    {
        if (spiceAmountDictionary.TryGetValue(spiceName, out TMP_Text amountText))
        {
            Spice selectedSpice = spiceSelected.FirstOrDefault(spice => spice.nameOfSpice == spiceName);

            if (selectedSpice != null)
            {
                float spicePourPercent = Mathf.FloorToInt(100 * (selectedSpice.startingSpiceAmount / selectedSpice.randomMaxAmount));

                spiceSelected[0].spicePurity = spicePourPercent;

                amountText.text = spicePourPercent.ToString() + "%";

                if (spicePourPercent <= 100f)
                {
                    amountText.color = Color.Lerp(new Color(0.8113208f, 0.3559096f, 0.3559096f, 1f), new Color(0.1933962f, 1f, 0.4519076f, 1f), spicePourPercent / 100f);
                }
                else if (spicePourPercent <= 150f)
                {
                    amountText.color = Color.Lerp(new Color(0.1933962f, 1f, 0.4519076f, 1f), new Color(0.7924528f, 0.2631899f, 0.1158775f, 1f), (spicePourPercent - 100f) / 50f);
                }
                else if (spicePourPercent <= 200f)
                {
                    amountText.color = Color.Lerp(new Color(0.7924528f, 0.2631899f, 0.1158775f, 1f), new Color(0.1981132f, 0.01958212f, 0.01214846f, 1f), (spicePourPercent - 150f) / 50f);
                }
            }
        }
        else
        {
            Debug.LogError("Spice name not found in dictionary");
        }
    }

    protected void UpdatePlayerScore(float averagePurity)
    {
        if(successfulRatio)
        {
            playerScore += 1000 * spiceUsedInTicket.Count * (averagePurity/100);
            GameManager.instance.UpdatePlayerScore(playerScore);
        }
        else if(failedRatio)
        {
            playerScore += 0;
            GameManager.instance.UpdatePlayerScore(playerScore);
        }

        Debug.Log(playerScore);

        
    }

    public void SetSpiceInfo(int index, string name, string amount)
    {
        if (index >= 0 && index < spiceNameText.Length && index < spiceAmountText.Length)
        {
            spiceNameText[index].text = name;
            spiceAmountText[index].text = amount;
        }
        else
        {
            Debug.LogError("Index out of range");
        }
    }

    public (string name, string amount) GetSpiceInfo(int index)
    {
        if (index >= 0 && index < spiceNameText.Length && index < spiceAmountText.Length)
        {
            return (spiceNameText[index].text, spiceAmountText[index].text);
        }
        else
        {
            Debug.LogError("Index out of range");
            return (null, null);
        }
    }

    public bool ReturnCorrectSelection()
    {
        return this.correctSelection;
    }

    public bool ReturnOrderSubmitted()
    {
        return this.orderSubmitted;
    }

    public List<Spice> ReturnSpiceSelected()
    {
         return this.spiceSelected; 
    }

    public bool ReturnSpiceUsedBool()
    {
        return this.spiceHasBeenUsed;
    }

    public void SetSpiceUsedBool(bool value)
    {
        spiceHasBeenUsed = value;
    }
}

[System.Serializable]
public class Spice
{
    public string nameOfSpice;
    public float startingSpiceAmount;
    public float randomMaxAmount;
    public float spicePurity;
    public Color color;
    public Color color2;

    public Spice(string name, float startingAmount, float maxAmount, float purity, Color color, Color color2)
    {
        this.nameOfSpice = name;
        this.startingSpiceAmount = startingAmount;
        this.randomMaxAmount = maxAmount;
        this.spicePurity = purity;
        this.color = color;
        this.color2 = color2;
    }
}


[System.Serializable]
public class Palfnir : Spice
{
    public Palfnir() : base("Palfnir", 0, Random.Range(2000, 4001), 0, new Color(1.0f, 0f, 0f, 1.0f), new Color(0f, 1f, 0f, 1.0f)) { }
}

[System.Serializable]
public class Brikkol : Spice
{
    public Brikkol() : base("Brikkol", 0, Random.Range(2000, 4001), 0, new Color(1f, 1f, 0f, 1.0f), new Color(1f, 1f, 1f, 1.0f)) { }
}

[System.Serializable]
public class Tenalc : Spice
{
    public Tenalc() : base("Tenalc", 0, Random.Range(2000, 4001), 0, new Color(0.5f, 0.5f, 0.5f, 1.0f), new Color(0f, 0f, 0f, 1.0f)) { }
}

[System.Serializable]
public class Gremlock : Spice
{
    public Gremlock() : base("Gremlock", 0, Random.Range(2000, 4001), 0, new Color(0.2127358f, 0.4150943f, 0.03328587f, 1.0f), new Color(0.5899674f, 0.7075472f, 0.3170612f, 1.0f)) { }
}

[System.Serializable]
public class Sewort : Spice
{
    public Sewort() : base("Sewort", 0, Random.Range(2000, 4001), 0, new Color(0.2f, 0.8f, 0.5f, 1.0f), new Color(0.2f, 0.8f, 0.5f, 1.0f)) { }
}

[System.Serializable]
public class Ezethaxis : Spice
{
    public Ezethaxis() : base("Ezethaxis", 0, Random.Range(2000, 4001), 0, new Color(0.4528302f, 0.05767177f, 0.05767177f, 1.0f), new Color(0.8392157f, 0.345098f, 0.4576187f, 1.0f)) { }
}

public class Aidleqar_Sap : Spice
{
    public Aidleqar_Sap() : base("Aidleqar Sap", 0, Random.Range(2000, 4001), 0, new Color(0.2f, 0.8f, 0.5f, 1.0f), new Color(0.2f, 0.8f, 0.5f, 1.0f)) { }
}

[System.Serializable]
public class Ully : Spice
{
    public Ully() : base("Ully", 0, Random.Range(2000, 4001), 0, new Color(0.4528302f, 0.05767177f, 0.05767177f, 1.0f), new Color(0.8392157f, 0.345098f, 0.4576187f, 1.0f)) { }
}
