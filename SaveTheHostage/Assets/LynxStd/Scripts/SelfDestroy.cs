using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {

	void Start () {
        Destroy(gameObject, 2f);
	}

}
