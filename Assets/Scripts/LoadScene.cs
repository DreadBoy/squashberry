using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadScene : MonoBehaviour
{
    public String LevelName;

    public void loadLevel()
    {
        Application.LoadLevel(LevelName);
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { loadLevel(); });
    }
}
