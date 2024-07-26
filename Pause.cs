using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject _pause;

    public void Start()
    {
        _pause.SetActive(false);
    }

    public void QuitGame(string type)
    {
        if (type == "menu")
        {
            SceneManager.LoadScene("PlayMenu");
        }
        else
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }

    public void Disable()
    {
        _pause.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pause != null)
            {
                _pause.SetActive(!_pause.activeSelf);
            }
        }
    }
}