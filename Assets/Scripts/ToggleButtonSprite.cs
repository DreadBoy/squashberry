using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public class ToggleButtonSprite : MonoBehaviour
{


    private Button button;
    [SerializeField]
    private Sprite altSprite;
    private Sprite orgSprite;
    private Boolean altState = false;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => { OnClick(); });

        orgSprite = GetComponent<Image>().sprite;
    }

    void OnClick()
    {
        altState = !altState;
        if (altState)
            GetComponent<Image>().sprite = altSprite;
        else
            GetComponent<Image>().sprite = orgSprite;

    }
}
