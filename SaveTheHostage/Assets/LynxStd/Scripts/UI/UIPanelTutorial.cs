using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelTutorial : MonoBehaviour {

    public bool isActive;

    void Start()
    {
        gameObject.SetActive(isActive);
    }
    public void OnTapTutorial()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
