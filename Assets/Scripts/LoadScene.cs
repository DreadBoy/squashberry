using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadScene : MonoBehaviour
{
    public String name;

    public void loadLevel()
    {
        Application.LoadLevel(name);
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { loadLevel(); });
    }
}
