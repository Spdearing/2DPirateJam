using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("TMPro")]
    [SerializeField] private TMP_Text spiceDisplayText;
    [SerializeField] private TMP_Text playerScoreText;

    [Header("Scripts")]
    [SerializeField] private GenerateOrderTicket generateTicket;
    [SerializeField] private SpiceSpawner spiceSpawner;


    [Header("Floats")]
    [SerializeField] private float playerScore = 0;

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
                if (generateTicket == null)
                {
                    generateTicket = GameObject.Find("GeneratedTicket").GetComponent<GenerateOrderTicket>();
                }
                if (spiceSpawner == null)
                {
                    spiceSpawner = GameObject.Find("SpiceParticleSystem").GetComponent<SpiceSpawner>();
                }
                if (playerScoreText == null)
                {
                    playerScoreText = GameObject.Find("PlayerScoreText").GetComponent<TMP_Text>();
                }

                spiceDisplayText.text = string.Empty;

                playerScoreText.text = "Score: " + playerScore;

                break;
        }
    }

    public void UpdatePlayerScore(float playerScore)
    {
        playerScoreText.text = "Score: " + playerScore;
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

    public GenerateOrderTicket ReturnGenerateOrderTicket()
    {
        return this.generateTicket;
    }

    public SpiceSpawner ReturnSpiceSpawner()
    {
        return this.spiceSpawner;
    }

    public float ReturnPlayerScore()
    {
        return this.playerScore;
    }

    #endregion
}
