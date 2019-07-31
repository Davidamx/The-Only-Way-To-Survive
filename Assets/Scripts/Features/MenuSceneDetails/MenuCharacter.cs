using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCharacter : MonoSingleton<MenuCharacter> {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartStand()
    {
        anim.enabled = true;
    }
}
