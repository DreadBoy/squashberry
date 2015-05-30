using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SetTrigger : MonoBehaviour {

    Animator animator = null;
    public String trigger;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { setTrigger(); });
        animator = GetComponent<Animator>();
    }

    public void setTrigger()
    {
        animator.SetTrigger(trigger);
    }

}
