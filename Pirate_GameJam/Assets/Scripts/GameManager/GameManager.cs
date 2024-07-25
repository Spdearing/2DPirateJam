using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TMP_Text spiceDisplayText;


    // Start is called before the first frame update
    void Start()
    {
        spiceDisplayText = GameObject.Find("SpiceSelectedDisplay").GetComponent<TMP_Text>();
    }

    public TMP_Text ReturnSpiceDisplayNameText()
    {
        return this.spiceDisplayText;
    }
}
