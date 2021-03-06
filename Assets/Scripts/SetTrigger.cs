﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SetTrigger : MonoBehaviour {

    public Animator animator = null;
    public String trigger;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { setTrigger(); });
    }

    public void setTrigger()
    {
        if (animator == null)
            return;
        animator.SetTrigger(trigger);
    }

}
