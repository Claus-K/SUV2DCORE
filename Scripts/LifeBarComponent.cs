using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarComponent : MonoBehaviour
{
    private UnityEngine.Camera _camera;
    public Slider LifeBar;
    private float lifeBarTimeout = 4f;
    private float checkValue;
    private float dt;

    private void Start()
    {
        _camera = UnityEngine.Camera.main;
    }

    private void FixedUpdate()
    {
        LifeBar.transform.rotation = _camera.transform.rotation;
        if (Time.time - dt > lifeBarTimeout)
        {
            if (Math.Abs(LifeBar.value - checkValue) < 0.0005f)
            {
                LifeBar.transform.gameObject.SetActive(false);
            }
            else
            {
                checkValue = LifeBar.value;
                dt = Time.time;
            }
        }
    }

    private void OnEnable()
    {
        checkValue = LifeBar.value;
        dt = Time.time;
    }
}