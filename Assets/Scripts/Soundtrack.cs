using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class Soundtrack : MonoBehaviour
{
    private Boolean sleeping = false;
    public Single waitTime = 15;
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
        GetComponent<AudioSource>().playOnAwake = false;
        sleeping = true;
        StartCoroutine(startPlaying(waitTime));
    }

    void Update()
    {
        if (sleeping)
            return;
        if (!Settings.saveData.music)
            GetComponent<AudioSource>().Pause();
        else if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
    }

    IEnumerator startPlaying(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (Settings.saveData.music)
            GetComponent<AudioSource>().Play();
        sleeping = false;
    }
}