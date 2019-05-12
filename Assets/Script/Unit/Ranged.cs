using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : UnitControl {

    void Start() {
        if (unitTeam == 1)
            transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = playerMaterial;
        else
            transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = enemyMaterial;

        anim = GetComponent<Animator>();
    }

    void Update() {
        if (State == UnitState.Walking)
            UnitWalkForward();
        else if (State == UnitState.Attacking)
        {
            if (curTarget != null)
            {
                float dist = Vector3.Distance(transform.position, curTarget.transform.position);
                if (dist < 5)
                {
                    if (dist < 4 && dist < 5)
                        UnitWalkBackward();
                    else
                        anim.SetBool("Attacking", true);
                }
                else
                    UnitWalkForward();
            }
            else if (curTarget == null && mergeTarget == null)
            {
                State = UnitState.Walking;
            }
            else if (curTarget == null)
            {
                State = UnitState.Walking;
            }
        }

        if (unitHealth <= 0)
            Destroy(gameObject);

        if (mergingNow && State == UnitState.Merging && mergeTarget != null)
        {
            Vector3 direction = mergeTarget.transform.position - transform.position; // Direction of Merging
            transform.position += direction * Time.deltaTime * 10;
            if (Vector3.Distance(transform.position, mergeTarget.transform.position) < 0.1f)
                Destroy(gameObject);
        }
        else if (State == UnitState.Merging && mergeTarget == null)
            State = UnitState.Walking;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerUnit") && gameObject.CompareTag("EnemyUnit") && (gameObject.name == "Ranged Unit" || gameObject.name == "Mage Unit"))
        {
            if (curTarget != null && other.name == "Melee Unit" || other.name == "Tank Unit")
            {
                curTarget = other.gameObject;
                State = UnitState.Attacking;
            }
            else if (curTarget == null)
            {
                curTarget = other.gameObject;
                State = UnitState.Attacking;
            }
        }
        else if (other.CompareTag("EnemyUnit") && gameObject.CompareTag("PlayerUnit") && (gameObject.name == "Ranged Unit" || gameObject.name == "Mage Unit"))
        {
            if (curTarget != null && other.name == "Melee Unit" || other.name == "Tank Unit")
            {
                curTarget = other.gameObject;
                State = UnitState.Attacking;
            }
            else if (curTarget == null)
            {
                curTarget = other.gameObject;
                State = UnitState.Attacking;
            }
        }
        if (other.CompareTag("PlayerUnit") && !gameObject.CompareTag("EnemyUnit"))
        {
            if (gameObject.name == "Ranged Unit" && other.gameObject.name == "Ranged Unit")
            {
                if (other.transform.position.x > transform.position.x)
                {
                    mergeTarget = other.gameObject;
                    if (Vector3.Distance(transform.position, mergeTarget.transform.position) < 1)
                        MergeUnits(gameObject, other.gameObject, false);
                }
            }
        }
        else if (other.CompareTag("EnemyUnit") && !gameObject.CompareTag("PlayerUnit"))
        {
            if (gameObject.name == "Ranged Unit" && other.gameObject.name == "Ranged Unit")
            {
                if (other.transform.position.x < transform.position.x)
                {
                    mergeTarget = other.gameObject;
                    if (Vector3.Distance(transform.position, mergeTarget.transform.position) < 1)
                        MergeUnits(gameObject, other.gameObject, true);
                }
            }
        }
    }
}
