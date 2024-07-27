using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private TMP_Text[] spiceAmountText;

    [Header("Buttons")]
    [SerializeField] private Button[] spiceButtons;

    [Header("TMP_Text")]
    [SerializeField] private TMP_Text displayText;

    [Header("Bools")]
    [SerializeField] private bool pourButtonPressed;
    [SerializeField] private bool correctSelection;

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
        orderRotations = 0;
        correctSelection = false;
        displayText = GameManager.instance.ReturnSpiceDisplayNameText();
        spiceManager = GameManager.instance.ReturnSpiceManager();

        InitializeSpiceNames();
        InitializeSpiceTexts();
        InitializeSpices();
        InitializeButtons();

        ClearTexts();
    }

    private void InitializeSpiceNames()
    {
        spiceNames = new string[6];
    }

    private void InitializeSpiceTexts()
    {
        spiceNameText = new TMP_Text[3];
        spiceNameText[0] = GameObject.Find("SpiceOne").GetComponent<TMP_Text>();
        spiceNameText[1] = GameObject.Find("SpiceTwo").GetComponent<TMP_Text>();
        spiceNameText[2] = GameObject.Find("SpiceThree").GetComponent<TMP_Text>();

        spiceAmountText = new TMP_Text[3];
        spiceAmountText[0] = GameObject.Find("SpiceAmountOne").GetComponent<TMP_Text>();
        spiceAmountText[1] = GameObject.Find("SpiceAmountTwo").GetComponent<TMP_Text>();
        spiceAmountText[2] = GameObject.Find("SpiceAmountThree").GetComponent<TMP_Text>();
    }

    private void InitializeSpices()
    {
        spices = new Spice[6];
        spices[0] = new GroundSage();
        spices[1] = new Tarragon();
        spices[2] = new DillPollen();
        spices[3] = new Chervil();
        spices[4] = new Spearmint();
        spices[5] = new Sumac();
    }

    private void InitializeButtons()
    {
        spiceButtons = new Button[6];
        spiceButtons[0] = GameObject.Find("Ground Sage").GetComponent<Button>();
        spiceButtons[1] = GameObject.Find("Tarragon").GetComponent<Button>();
        spiceButtons[2] = GameObject.Find("Dill Pollen").GetComponent<Button>();
        spiceButtons[3] = GameObject.Find("Chervil").GetComponent<Button>();
        spiceButtons[4] = GameObject.Find("Spearmint").GetComponent<Button>();
        spiceButtons[5] = GameObject.Find("Sumac").GetComponent<Button>();

        foreach (Button button in spiceButtons)
        {
            button.onClick.AddListener(() => DisplaySelectedSpice(button));
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
        string spiceName = clickedButton.gameObject.name;

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
        orderRotations++;
        int randomSpiceIndex = Random.Range(2, 4);

        if (randomSpiceIndex > spices.Length)
        {
            Debug.LogError("Not enough unique spices available to generate the order.");
            return;
        }

        randomSpiceNames = new string[randomSpiceIndex];

        ClearSpiceUsedInTicketIfNecessary();

        for (int i = 0; i < randomSpiceIndex; i++)
        {
            int randomSpiceName = GetUniqueRandomSpiceName();
            Spice randomSpiceHolder = spices[randomSpiceName];

            spiceUsedInTicket.Add(randomSpiceHolder);
            randomSpiceNames[i] = randomSpiceHolder.nameOfSpice;
            spiceNameText[i].text = randomSpiceNames[i];

            float randomSpiceAmount = randomSpiceHolder.startingSpiceAmount;
            spiceAmountText[i].text = randomSpiceAmount.ToString();
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

    private void ClearSpiceUsedInTicketIfNecessary()
    {
        if (orderRotations == 3)
        {
            spiceUsedInTicket.Clear();
            orderRotations = 0;
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
        Spice selectedSpice = spices.FirstOrDefault(spice => spice.nameOfSpice == spiceName);

        if (selectedSpice != null && spiceUsedInTicket.Contains(selectedSpice))
        {
            correctSelection = true;
            spiceSelected.Insert(0, selectedSpice);
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

    public void GeneratePurity()
    {
        float totalPurity = spiceUsedInTicket.Sum(spice => spice.spicePurity);
        float averagePurity = totalPurity / spiceUsedInTicket.Count;

        SubmitOrder(averagePurity);
        displayText.text = currentTickets + " Current Tickets have been completed ";
    }

    void SubmitOrder(float averagePurity)
    {
        if (averagePurity > 75.0f)
        {
            currentTickets++;
            ticketsCompleted = currentTickets;
        }
        else
        {
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
                amountText.text = selectedSpice.startingSpiceAmount.ToString();
                float spicePourPercent = Mathf.FloorToInt(100 * (selectedSpice.startingSpiceAmount / selectedSpice.randomMaxAmount));
                displayText.text = selectedSpice.nameOfSpice + " is now " + spicePourPercent + "% full";
                spiceSelected[0].spicePurity = spicePourPercent;
            }
        }
        else
        {
            Debug.LogError("Spice name not found in dictionary");
        }
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
}

[System.Serializable]
public class Spice
{
    public string nameOfSpice;
    public float startingSpiceAmount;
    public float randomMaxAmount;
    public float spicePurity;

    public Spice(string name, float startingAmount, float maxAmount, float purity)
    {
        nameOfSpice = name;
        startingSpiceAmount = startingAmount;
        randomMaxAmount = maxAmount;
        spicePurity = purity;
    }
}

[System.Serializable]
public class GroundSage : Spice
{
    public GroundSage() : base("Ground Sage", 0, Random.Range(2000, 4001), 0) { }
}

[System.Serializable]
public class Tarragon : Spice
{
    public Tarragon() : base("Tarragon", 0, Random.Range(2000, 4001), 0) { }
}

[System.Serializable]
public class DillPollen : Spice
{
    public DillPollen() : base("Dill Pollen", 0, Random.Range(2000, 4001), 0) { }
}

[System.Serializable]
public class Chervil : Spice
{
    public Chervil() : base("Chervil", 0, Random.Range(2000, 4001), 0) { }
}

[System.Serializable]
public class Spearmint : Spice
{
    public Spearmint() : base("Spearmint", 0, Random.Range(2000, 4001), 0) { }
}

[System.Serializable]
public class Sumac : Spice
{
    public Sumac() : base("Sumac", 0, Random.Range(2000, 4001), 0) { }
}
