using UnityEngine;
using System.Collections;

public class Soundtrack : MonoBehaviour
{

    private static Soundtrack instance = null;
    public static Soundtrack Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if (!Settings.saveData.music)
            GetComponent<AudioSource>().Stop();
    }

    void Update()
    {
        if (!Settings.saveData.music)
            GetComponent<AudioSource>().Pause();
        else if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
    }
}