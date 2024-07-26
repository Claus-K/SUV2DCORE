using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManagerV2 : MonoBehaviour
{
    public static ResourceManagerV2 Instance { get; private set; }

    [SerializeField] private Slider populationUI;
    [SerializeField] private Slider energyUI;

    [SerializeField] private int populationCap;
    [SerializeField] private int energyCap;

    [SerializeField] private int passiveEnergy;

    [SerializeField] private bool hasPassiveEnergy;
    private bool isGettingPassiveEnergy;


    private int population;
    private int energy;

    public int Population
    {
        get => population;
        set
        {
            population = value;
            SetPopulationUI();
        }
    }

    public int Energy
    {
        get => energy;
        set
        {
            energy = Mathf.Clamp(value, 0, energyCap);
            SetEnergyUI();
            if (!isGettingPassiveEnergy && hasPassiveEnergy)
            {
                StartCoroutine(PassiveEnergy());
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (hasPassiveEnergy)
        {
            StartCoroutine(PassiveEnergy());
        }
    }

    public bool CanMitosi(int energyRequired, int populationRequired)
    {
        return CheckEnergy(energyRequired) && CheckPopulation(populationRequired);
    }

    private void SetPopulationUI()
    {
        if (populationUI.IsDestroyed()) return;
        var popUI = populationUI.GetComponentInChildren<TextMeshProUGUI>();
        populationUI.value = (float)Population / populationCap;
        popUI.text = $"{Population}/{populationCap}";
    }

    private void SetEnergyUI()
    {
        if (energyUI.IsDestroyed()) return;
        var eneUI = energyUI.GetComponentInChildren<TextMeshProUGUI>();
        energyUI.value = (float)Energy / energyCap;
        eneUI.text = $"{Energy}/{energyCap}";
    }

    private bool CheckEnergy(int value)
    {
        if (value <= Energy)
        {
            return true;
        }

        Notification.Instance.SetNotification("Not enough Energy");
        return false;
    }

    private bool CheckPopulation(int value)
    {
        if (Population + value <= populationCap)
        {
            return true;
        }

        Notification.Instance.SetNotification("Population Cap");
        return false;
    }

    private IEnumerator PassiveEnergy()
    {
        isGettingPassiveEnergy = true;
        while (isGettingPassiveEnergy)
        {
            Energy += passiveEnergy;
            if (Energy == energyCap)
            {
                isGettingPassiveEnergy = false;
            }

            yield return new WaitForSeconds(2);
        }
    }
}