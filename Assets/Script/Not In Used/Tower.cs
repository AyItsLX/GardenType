using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public float towerHealth = 100f;
    public Transform towerHealthGUI;

	void Start () {
		
	}
	
	void Update () {
        if (towerHealth <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelMenu");
        }
	}

    public void TakeDamage(float Damage)
    {
        towerHealth -= Damage;
        towerHealthGUI.localScale = new Vector3(towerHealth / 1000, towerHealthGUI.localScale.y, towerHealthGUI.localScale.z);
    }
}
