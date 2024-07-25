using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceSpawner : MonoBehaviour
{
    private int spiceAmount = 0;
    private ParticleSystem spiceParticleSystem;
    private float spicePourPercent;
    [SerializeField] private GameObject spiceParticleObject;
    [SerializeField] private ParticleSystem.EmissionModule emissionModule;
    [SerializeField] private float minParticles = 1;
    [SerializeField] private float maxParticles = 1;

    private void Start()
    {
        spiceParticleObject = GameObject.Find("SpiceParticleSystem");
        spiceParticleSystem = spiceParticleObject.GetComponent<ParticleSystem>();
        emissionModule = spiceParticleSystem.emission;
        emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(minParticles, maxParticles);

        spiceParticleObject.SetActive(false);
    }
    private void Update()
    {
        MoveSpiceSpawner();
        PourSpice();
    }
    void OnParticleCollision(GameObject other)
    {
        spiceAmount++;
        Debug.Log(spiceAmount);
    }
    private void PourSpice()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            spiceParticleObject.SetActive(true);
            while (maxParticles < 250f)
            {
                maxParticles = maxParticles+maxParticles;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            spiceParticleObject.SetActive(false);
        }
    }
    private void MoveSpiceSpawner()
    {
        Vector3 mousePos;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        this.gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0.0f);
    }
}
