using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TMP_Text spiceDisplayText;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (spiceDisplayText == null)
        {
            spiceDisplayText = GameObject.Find("DisplayText").GetComponent<TMP_Text>();
        }

        spiceDisplayText.text = string.Empty;
    }


    public TMP_Text ReturnSpiceDisplayNameText()
    {
        if (spiceDisplayText == null)
        {
            Debug.LogError("Spice Display Text is not assigned.");
        }
        return this.spiceDisplayText;
    }
}
