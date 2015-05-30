using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SubmitNameChange : MonoBehaviour
{
    public GameObject settings = null;
    public InputField input = null;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { submitName(); });
        input.text = Settings.saveData.playerName;
    }

    public void submitName()
    {
        var sett = settings.GetComponent<Settings>();
        sett.PlayerName(input.text);
    }
}
