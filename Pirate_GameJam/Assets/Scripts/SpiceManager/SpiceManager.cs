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


    void Start()
    {
        spicePicked = new bool[6];
        spicePicked[0] = false;
        spicePicked[1] = false;
        spicePicked[2] = false;
        spicePicked[3] = false;
        spicePicked[4] = false;
        spicePicked[5] = false;
        

        displayText = GameManager.instance.ReturnSpiceDisplayNameText();

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
