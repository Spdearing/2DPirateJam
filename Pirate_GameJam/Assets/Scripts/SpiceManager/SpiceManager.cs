using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceManager : MonoBehaviour
{
    [SerializeField] private string[] spiceNames;
    


    // Start is called before the first frame update
    void Start()
    {
        spiceNames = new string[6];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplaySelectedSpice()
    {
        string spiceName = gameObject.name;

        GameManager.instance.ReturnSpiceDisplayNameText().text = spiceName + " has been selected";

    }
}
