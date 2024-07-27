using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("TMPro")]
    [SerializeField] private TMP_Text spiceDisplayText;

    [Header("Scripts")]
    [SerializeField] private SpiceManager spiceManager;
    [SerializeField] private GenerateOrderTicket generateTicket;


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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Sam's_Test_Scene":

                if (spiceDisplayText == null)
                {
                    spiceDisplayText = GameObject.Find("DisplayText").GetComponent<TMP_Text>();
                }

                if(spiceManager == null)
                {
                    spiceManager = GameObject.Find("SpiceManager").GetComponent<SpiceManager>();
                }
                if (generateTicket == null)
                {
                    generateTicket = GameObject.Find("GeneratedTicket").GetComponent<GenerateOrderTicket>();
                }

                spiceDisplayText.text = string.Empty;

                break;
        }
    }

    #region // Return Variables
    public TMP_Text ReturnSpiceDisplayNameText()
    {
        if (spiceDisplayText == null)
        {
            Debug.LogError("Spice Display Text is not assigned.");
        }
        return this.spiceDisplayText;
    }

    public SpiceManager ReturnSpiceManager()
    {
        return this.spiceManager;
    }

    public GenerateOrderTicket ReturnGenerateOrderTicket()
    {
        return this.generateTicket;
    }

    #endregion
}
