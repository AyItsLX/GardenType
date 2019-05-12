using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTile : MonoBehaviour {

    public bool isActive = false;
    public bool ShowWord = false;

    void Update()
    {
        if (ShowWord)
        {
            if (transform.eulerAngles.y > 0)
            {
                transform.Rotate(Vector3.up * -Time.deltaTime * 250);
                if (transform.eulerAngles.y < 5 && transform.eulerAngles.y > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }
        }
        else
        {
            if (transform.eulerAngles.y < 180)
            {
                transform.Rotate(Vector3.up * Time.deltaTime * 250);
                if (transform.eulerAngles.y > 175 && transform.eulerAngles.y < 180)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
            }
        }
    }
}
