using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

	void Awake () {
        if (Menu.dontShow == 0)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
