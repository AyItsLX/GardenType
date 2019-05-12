using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public static bool showTutorial;

    public GameObject tutorialGroup;
    public GameObject hideMenuButton;

	void Start () {
        if (showTutorial)
        {
            tutorialGroup.SetActive(true);
            hideMenuButton.SetActive(false);
        }
	}
	
	void Update () {
        if (!showTutorial)
        {
            tutorialGroup.SetActive(false);
            hideMenuButton.SetActive(true);
        }
	}

    public void DontShowAgain()
    {
        showTutorial = false;
    }
}
