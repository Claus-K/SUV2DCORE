using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class Scenes : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public UnityEngine.UI.Button playButton;
    private string selectedScene;

    private void Start()
    {
        playButton.interactable = false;
    }

    public void PlayScene()
    {
        Debug.Log($"{selectedScene}");
        
        if (!playButton.interactable) return;
        playButton.interactable = false;
        SceneManager.LoadScene(selectedScene);
    }

    public void SceneSelection(GameObject sceneText)
    {
        switch (sceneText.name)
        {
            case "Scene 1":
                title.text = "First Scene";
                description.text = "Here first adventure";
                selectedScene = "SampleScene";
                break;
            case "Scene 2":
                title.text = "Second Scene";
                description.text = "Here Second adventure";
                selectedScene = "SampleScene";
                break;
        }
        
        if (!string.IsNullOrEmpty(title.text) && title.text != "Title")
        {
            playButton.interactable = true;
        }
    }

    public void ReturnMainMenu()
    {
        Debug.Log("Returning to Menu");
        SceneManager.LoadScene("Menu");
    }
}