using System;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    private Button _quitButton;

    private void Awake()
    {
        _quitButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _quitButton.onClick.AddListener(Quit);
    }

    private void OnDisable()
    {
        _quitButton.onClick.RemoveListener(Quit);
    }

    private void Quit()
    {
        Application.Quit();
        print("Quit");
    }
}