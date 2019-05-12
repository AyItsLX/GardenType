using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public bool ShowWord = false;

	void Update () {
        if (ShowWord)
        {
            if (transform.eulerAngles.x > 0)
            {
                transform.Rotate(Vector3.right * -Time.deltaTime * 150);
                if (transform.eulerAngles.x < 5 && transform.eulerAngles.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }
        }
        else
        {
            if (transform.eulerAngles.x < 90)
            {
                transform.Rotate(Vector3.right * Time.deltaTime * 150);
                if (transform.eulerAngles.x > 85 && transform.eulerAngles.x < 90)
                {
                    transform.eulerAngles = new Vector3(90, 0, 0);
                }
            }
        }
	}
}
