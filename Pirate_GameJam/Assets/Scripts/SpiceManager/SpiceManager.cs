using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpiceManager : MonoBehaviour
{
    [Header("Strings")]
    [SerializeField] private string[] spiceNames;


    [Header("Spice Amount")]
    [SerializeField] private int[] spiceAmount;

    [Header("Buttons")]
    [SerializeField] private Button[] spiceButtons;

    [Header("Index For SpicePicked")]
    [SerializeField] private int spicePickedIndex;

    [Header("TMP_Text")]
    [SerializeField] private TMP_Text displayText;

    [Header("Bools")]
    [SerializeField] private bool[] spicePicked;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the spiceButtons array with the correct size
        spiceButtons = new Button[6];
        spiceButtons[0] = GameObject.Find("Ground Sage").GetComponent<Button>();
        spiceButtons[1] = GameObject.Find("Tarragon").GetComponent<Button>();
        spiceButtons[2] = GameObject.Find("Dill Pollen").GetComponent<Button>();
        spiceButtons[3] = GameObject.Find("Chervil").GetComponent<Button>();
        spiceButtons[4] = GameObject.Find("Spearmint").GetComponent<Button>();
        spiceButtons[5] = GameObject.Find("Sumac").GetComponent<Button>();

        spiceNames = new string[6];
        spiceNames[0] = "Ground Sage";
        spiceNames[1] = "Tarragon";
        spiceNames[2] = "Dill Pollen";
        spiceNames[3] = "Chervil";
        spiceNames[4] = "Spearmint";
        spiceNames[5] = "Sumac";

        spicePicked = new bool[6];
        spicePicked[0] = false;
        spicePicked[1] = false;
        spicePicked[2] = false;
        spicePicked[3] = false;
        spicePicked[4] = false;
        spicePicked[5] = false;


        displayText = GameManager.instance.ReturnSpiceDisplayNameText();

        foreach (Button button in spiceButtons)
        {
            button.onClick.AddListener(() => DisplaySelectedSpice(button));
        }
    }

    // This method will be called when a button is clicked
    private void DisplaySelectedSpice(Button clickedButton)
    {
        
        string spiceName = clickedButton.gameObject.name;
        DetermineSpiceSelected(spiceName);


        if (displayText != null)
        {
            displayText.text = spiceName + " has been selected";
        }
        else
        {
            Debug.LogError("Display text is null.");
        }
    }

    private void DetermineSpiceSelected(string spiceName)
    {
        switch(spiceName) 
        {
            case "Ground Sage":

                spicePickedIndex = 0;

                break;

            case "Tarragon":

                spicePickedIndex = 1;

                break;

            case "Dill Pollen":

                spicePickedIndex = 2;

                break;

            case "Chervil":

                spicePickedIndex = 3;

                break;

            case "Spearmint":

                spicePickedIndex = 4;

                break;

            case "Sumac":

                spicePickedIndex = 5;

                break;

            default:

                break;
        }
    }


    public bool ReturnSpicePicked()
    {
        return this.spicePicked[spicePickedIndex];
    }

    public int ReturnSpiceAmount(int amount) 
    {
        return this.spiceAmount[amount];
    }

    public string ReturnSpiceNames(int number)
    {
        return this.spiceNames[number];
    }
}
