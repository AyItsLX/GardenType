using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour {

    public string shotFrom;
    public Transform foundTarget;

    void Awake() {
        Destroy(gameObject, 3);
    }

    void Update () {
        transform.position += transform.forward * Time.deltaTime * 5f;
	}

    void OnTriggerEnter(Collider other) {
        if (shotFrom == "Enemy" && other.gameObject.name == "UnitHB")
        {
            UnitControl unitTarget = other.GetComponentInParent<UnitControl>();
            if (unitTarget.unitTeam == 1)
            {
                unitTarget.unitHealth -= 5;
                Destroy(gameObject);
            }
        }
        else if (shotFrom == "Enemy" && other.gameObject.name == "UnitHB")
        {
            UnitControl unitTarget = other.GetComponentInParent<UnitControl>();
            if (unitTarget.unitTeam == 1)
            {
                unitTarget.unitHealth -= 5;
                Destroy(gameObject);
            }
        }
        if (shotFrom == "Player" && other.gameObject.name == "UnitHB")
        {
            UnitControl unitTarget = other.GetComponentInParent<UnitControl>();
            if (unitTarget.unitTeam == 0)
            {
                unitTarget.unitHealth -= 5;
                Destroy(gameObject);
            }
        }
        else if (shotFrom == "Player" && other.gameObject.name == "UnitHB")
        {
            UnitControl unitTarget = other.GetComponentInParent<UnitControl>();
            if (unitTarget.unitTeam == 0)
            {
                unitTarget.unitHealth -= 5;
                Destroy(gameObject);
            }
        }

        if (other.name == "TowerHB")
        {
            other.GetComponentInParent<Tower>().TakeDamage(5);
            Destroy(gameObject);
        }
    }
}
