using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceSpawner : MonoBehaviour
{
    private int spiceAmount = 0;
    private ParticleSystem spiceParticleSystem;
    private float spicePourPercent;
    [SerializeField] private GameObject spiceHolder;
    [SerializeField] private GameObject pourSpicePos;
    [SerializeField] private ParticleSystem.EmissionModule emissionModule;
    [SerializeField] private float minParticles = 0f;
    [SerializeField] private float maxParticles = 0f;
    [SerializeField] private float moveSpiceSpeed = 10f;
    [SerializeField] private bool pouring;
    [SerializeField] private bool inPosition = false;

    private void Start()
    {
        pouring = false;
        pourSpicePos = GameObject.Find("PourSpicePosition");
        spiceHolder = GameObject.Find("SpiceParticleHolder");
        spiceParticleSystem = GetComponent<ParticleSystem>();
        emissionModule = spiceParticleSystem.emission;
        emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(0f, 0f);
    }
    private void Update()
    {
        PourSpice();
        if (pouring == false)
        {
            MoveSpiceWithMouse();
        }
        else if (pouring != false)
        {
            MoveSpiceToPour();
        }

        if (spiceHolder.transform.position.x < (pourSpicePos.transform.position.x + .2f) && spiceHolder.transform.position.x > (pourSpicePos.transform.position.x - .2f) &&
            spiceHolder.transform.position.y < (pourSpicePos.transform.position.y + .2f) && spiceHolder.transform.position.y > (pourSpicePos.transform.position.y - .2f))
        {
            inPosition = true;
        }
        else
        {
            inPosition = false;
        }
    }
    void OnParticleCollision(GameObject other)
    {
        //Necessary for checking if the spice hits something
        spiceAmount++;
        Debug.Log(spiceAmount);
    }
    private void PourSpice()
    {
        //Checks if the player is holding down left click, if they are then the spice particles spawn with a max of 250
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            pouring = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            pouring = false;
            minParticles = 0f;
            maxParticles = 0f;
            emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(minParticles, maxParticles);
        }
        if (inPosition && pouring)
        {
            while (maxParticles < 250f)
            {
                maxParticles = maxParticles + 1f;
                emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(minParticles, maxParticles);
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

        spiceHolder.transform.position = new Vector3(mousePos.x, mousePos.y, 0.0f);
    }
}
