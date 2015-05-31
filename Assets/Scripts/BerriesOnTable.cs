using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BerriesOnTable : MonoBehaviour {

    private Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }
    public void UpdateText(int berries)
    {
        text.text = "Berries: " + berries + "/50";
    }
}
