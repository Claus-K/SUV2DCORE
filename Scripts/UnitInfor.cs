using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIElements;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfor : MonoBehaviour
{
    public static UnitInfor Instance { get; private set; }

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _healthToString;
    private HealthBarUI _healthBar;

    [SerializeField] private Slider _bloodSlider;
    [SerializeField] private TextMeshProUGUI _bloodCountText;
    private HealthBarUI _bloodHealthBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _healthBar = _healthSlider.GetComponent<HealthBarUI>();
        _bloodHealthBar = _bloodSlider.GetComponent<HealthBarUI>();
    }

    private void OnEnable()
    {
        ScenePoints.SetUnitInfor += SetBloodCount;
        ScenePoints.UpdateUnitInfor += UpdateBloodCount;
    }

    private void OnDisable()
    {
        ScenePoints.SetUnitInfor -= SetBloodCount;
        ScenePoints.UpdateUnitInfor -= UpdateBloodCount;
    }

    public void ShowUnitInfor(string nameOfHover, int health, int maxHealth)
    {
        UnitInforUpdate(nameOfHover, health, maxHealth);
        _healthSlider.gameObject.SetActive(true);
        _name.gameObject.SetActive(true);
    }

    private void UnitInforUpdate(string nameOfHover, int health, int maxHealth)
    {
        _name.text = $"{nameOfHover}";
        _healthToString.text = $"{health}/{maxHealth}";
        _healthBar.SetMaxHealth(maxHealth);
        _healthBar.Health = health;
    }

    public void DisableUnitInfor()
    {
        _healthBar.gameObject.SetActive(false);
        _name.gameObject.SetActive(false);
    }

    private void SetBloodCount(int count, int total)
    {
        _bloodCountText.text = $"{count}";
        _bloodHealthBar.SetMaxHealth(total);
        _bloodHealthBar.Health = count;
    }

    private void UpdateBloodCount(int count)
    {
        _bloodCountText.text = $"{count}";
        // _bloodHealthBar.Health = count;
    }
}