﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public Transform bananaTrans;
    public float speed = 5;

    public GameObject FxBanana;

    private float heightMax = 1;
    private bool isPickUp;

	void Start () {
		
	}
	
	void Update () {
        bananaTrans.Rotate(Vector3.right * Time.deltaTime * speed * 10, Space.Self);

        Vector3 bananaPos = bananaTrans.position;
        bananaPos.y = Mathf.PingPong(Time.time, heightMax);
        bananaTrans.position = bananaPos;
	}

    public void OnPickItem()
    {
        Debug.Log("Pick Item!");
        if (!isPickUp)
        {
            Instantiate(FxBanana, transform.position, Quaternion.identity);
            isPickUp = true;
            Destroy(gameObject);
        }

    }
}
