using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpiceSpawner : MonoBehaviour
{
    private ParticleSystem spiceParticleSystem;
    private float spiceAmount = 0f; // Keeps track of how much spice has hit the bowl
    private float spicePourPercent; // The percentage for how close the player is to the correct amount of spice
    private float elapsedTime = 0f; // Add this as a member variable to keep track of elapsed time
    private float duration = 10f; // The total duration for ramping up particles
    [SerializeField] private GameObject spiceHolder;
    [SerializeField] private GameObject pourSpicePos;
    [SerializeField] private ParticleSystem.EmissionModule emissionModule;
    [SerializeField] private float minParticles = 0f;
    [SerializeField] private float maxParticles = 0f;
    [SerializeField] private float moveSpiceSpeed = 10f;
    [SerializeField] private float spicePourSpeed = .01f;
    [SerializeField] private bool pouring;
    [SerializeField] private bool stillPouring;
    [SerializeField] private bool inPosition = false;
    [SerializeField] private TMP_Text spicePercentText;
    public float temporarySpiceGoal; //The spiceAmount goal


    [SerializeField] private GenerateOrderTicket generateTicket;

    private void Start()
    {
        temporarySpiceGoal = 2000f;
        stillPouring = false;
        pouring = false;
        //spicePercentText = GameObject.Find("SpicePercentText").GetComponent<TMP_Text>();
        pourSpicePos = GameObject.Find("PourSpicePosition");
        spiceHolder = GameObject.Find("SpiceParticleHolder");
        spiceParticleSystem = GetComponent<ParticleSystem>();
        emissionModule = spiceParticleSystem.emission;
        emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(0f, 0f);
        generateTicket = GameManager.instance.ReturnGenerateOrderTicket();
    }
    private void Update()
    {
        PourSpice();
        if (pouring == false)
        {
            MoveSpiceWithMouse();
        }
        else if (pouring == true)
        {
            MoveSpiceToPour();
        }
        //UpdateSpicePercent();
    }
    void OnParticleCollision(GameObject other)
    {
        //Necessary for checking if the spice hits something
        //spiceAmount++;
        //Debug.Log(spiceAmount);
        generateTicket.PourSpice();
    }
    private void PourSpice()
    {
        if (generateTicket.ReturnCorrectSelection() == true && !generateTicket.ReturnOrderSubmitted())
        {
            //Checks if the player is holding down left click, if they are then the spice particles spawn with a max of 250
            if (spiceHolder.transform.position.x < (pourSpicePos.transform.position.x + .2f) && spiceHolder.transform.position.x > (pourSpicePos.transform.position.x - .2f) &&
                spiceHolder.transform.position.y < (pourSpicePos.transform.position.y + .2f) && spiceHolder.transform.position.y > (pourSpicePos.transform.position.y - .2f))
            {
                inPosition = true;
            }
            else
            {
                inPosition = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && !pouring)
            {
                pouring = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1) && pouring)
            {
                stillPouring = true;
                maxParticles = 45f;
                elapsedTime = 0f;
            }

            if (inPosition && pouring && !stillPouring && maxParticles < 1000f)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                float exponent = 2f;
                t = Mathf.Pow(t, exponent);
                maxParticles = Mathf.Lerp(minParticles, 1000, t);
                emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(minParticles, maxParticles);
            }

            if (stillPouring && maxParticles > 0f)
            {
                maxParticles = Mathf.Lerp(maxParticles, -10f, (1f * Time.deltaTime));
                emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(minParticles, maxParticles);
            }

            if (maxParticles < 1 && stillPouring)
            {
                maxParticles = 0f;
                emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(minParticles, maxParticles);
            }

            if (stillPouring && maxParticles == 0f)
            {
                pouring = false;
                stillPouring = false;
            }
        } 
    }
    private void MoveSpiceToPour()
    {
        spiceHolder.transform.position = new Vector3(Mathf.Lerp(spiceHolder.transform.position.x, pourSpicePos.transform.position.x, (moveSpiceSpeed * Time.deltaTime)), Mathf.Lerp(spiceHolder.transform.position.y, pourSpicePos.transform.position.y, (moveSpiceSpeed * Time.deltaTime)), 0f);
    }
    private void MoveSpiceWithMouse()
    {
        //Like function name, just moves the spice to the mouse
        Vector3 mousePos;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        spiceHolder.transform.position = new Vector3(Mathf.Lerp(spiceHolder.transform.position.x, mousePos.x, (15f * Time.deltaTime)), Mathf.Lerp(spiceHolder.transform.position.y, mousePos.y, (15f * Time.deltaTime)), 0.0f);
    }

    private void UpdateSpicePercent()
    {
        //Calculates the percentage of how close the player is to the spice goal
        spicePourPercent = Mathf.FloorToInt(100 * (spiceAmount / temporarySpiceGoal));
        spicePercentText.text = spicePourPercent + "%";
    }
}
