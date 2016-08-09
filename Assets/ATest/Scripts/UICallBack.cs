using UnityEngine;
using System.Collections;
using System;

public class UICallBack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void btnCallback()
    {
        Debugger.LogWarning("btnCallback");

        double num2 = Math.Pow(2,2);
        Debugger.LogWarning("2 ^ 2 = " + num2);
    }
}
