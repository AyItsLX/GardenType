using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControl : MonoBehaviour {

    public enum UnitState
    {
        Walking,
        Attacking,
        Merging
    }

    [Header("Unit State")]
    public UnitState State;

    [Header("Unit Stats")]
    public int unitTeam = 1;
    public float unitHealth = 40;
    public float unitDamage = 20;
    public float unitSpeed = 1;
    public int unitLevel = 1;
    public bool hasFlag = false;
    public bool mergingNow;
    public GameObject curTarget;
    public GameObject mergeTarget;
    public GameObject unitFlag;

    [Header("Unit Team")]
    public Material enemyMaterial;
    public Material playerMaterial;

    public GameObject arrowPrefab;
    public GameObject magicPrefab;
    public GameObject playerFlag;
    public GameObject enemyFlag;
    public Animator anim;

    #region UnitWalk
    public void UnitWalkForward()
    {
        anim.SetBool("Attacking", false);
        transform.position += transform.forward * Time.deltaTime * unitSpeed;
    }
    public void UnitWalkBackward()
    {
        anim.SetBool("Attacking", false);
        transform.position += -transform.forward * Time.deltaTime * unitSpeed * 0.25f;
    }
    #endregion

    #region Update Flag
    public void UpdateFlag(bool spawnFlag, GameObject flagPrefab, GameObject otherUnit, GameObject ownGO)
    {
        if (spawnFlag && !hasFlag)
        {
            hasFlag = true;

            if (otherUnit.name == "Melee Unit")
                unitFlag = Instantiate(flagPrefab, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity, transform);
            else if (otherUnit.name == "Tank Unit")
                unitFlag = Instantiate(flagPrefab, transform.position + new Vector3(0, 3.5f, 0), Quaternion.identity, transform);
            if (otherUnit.name == "Ranged Unit")
                unitFlag = Instantiate(flagPrefab, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity, transform);
            else if (otherUnit.name == "Mage Unit")
                unitFlag = Instantiate(flagPrefab, transform.position + new Vector3(0, 3.5f, 0), Quaternion.identity, transform);

            unitFlag.transform.eulerAngles = new Vector3(0, -90, 0);

            otherUnit.GetComponent<UnitControl>().unitDamage += ownGO.GetComponent<UnitControl>().unitDamage * 0.05f;
            otherUnit.GetComponent<UnitControl>().unitHealth += ownGO.GetComponent<UnitControl>().unitHealth;
            otherUnit.GetComponent<UnitControl>().unitLevel += ownGO.GetComponent<UnitControl>().unitLevel;

            unitFlag.GetComponentInChildren<UnityEngine.UI.Text>().text = unitLevel.ToString("0");
        }
        else if (hasFlag)
        {
            otherUnit.GetComponent<UnitControl>().unitDamage += ownGO.GetComponent<UnitControl>().unitDamage * 0.05f;
            otherUnit.GetComponent<UnitControl>().unitHealth += ownGO.GetComponent<UnitControl>().unitHealth;
            otherUnit.GetComponent<UnitControl>().unitLevel += ownGO.GetComponent<UnitControl>().unitLevel;
            unitFlag.GetComponentInChildren<UnityEngine.UI.Text>().text = unitLevel.ToString("0");
        }
    }
    #endregion

    #region Merge Unit
    public void MergeUnits(GameObject ownGO, GameObject other, bool isEnemy)
    {
        if (!isEnemy)
        {
            if (mergeTarget.transform.position.x > transform.position.x)
            {
                mergeTarget.GetComponent<UnitControl>().UpdateFlag(true, playerFlag, other, ownGO);
                mergingNow = true;
                State = UnitState.Merging;
                GetComponent<Collider>().enabled = false;
            }
        }
        else if (isEnemy)
        {
            if (mergeTarget.transform.position.x < transform.position.x)
            {
                mergeTarget.GetComponent<UnitControl>().UpdateFlag(true, enemyFlag, other, ownGO);
                mergingNow = true;
                State = UnitState.Merging;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
    #endregion

    #region Attack Method
    public void MeleeAttack()
    {
        if (curTarget != null)
        {
            if (curTarget.name == "Mage Unit")
            {
                curTarget.GetComponent<UnitControl>().unitHealth -= unitDamage;
            }
            else if (curTarget.name == "Melee Unit" || curTarget.name == "Ranged Unit" || curTarget.name == "Tank Unit")
            {
                curTarget.GetComponent<UnitControl>().unitHealth -= unitDamage * 0.5f;
            }
            else if (curTarget != null && curTarget.name == "Player Castle" || curTarget.name == "Enemy Castle")
            {
                curTarget.GetComponent<Tower>().TakeDamage(unitDamage);
            }
        }
        else
            State = UnitState.Walking;
    }

    public void TankAttack()
    {
        if (curTarget != null)
        {
            if (curTarget.name == "Mage Unit")
            {
                curTarget.GetComponent<UnitControl>().unitHealth -= unitDamage;
            }
            else if (curTarget.name == "Tank Unit" || curTarget.name == "Mage Unit" || curTarget.name == "Ranged Unit")
            {
                curTarget.GetComponent<UnitControl>().unitHealth -= unitDamage * 0.5f;
            }
            else if (curTarget != null && curTarget.name == "Player Castle" || curTarget.name == "Enemy Castle")
            {
                curTarget.GetComponent<Tower>().TakeDamage(unitDamage);
            }
        }
        else
            State = UnitState.Walking;
    }

    public void RangedAttack()
    {
        if (curTarget != null)
        {
            GameObject temp = Instantiate(arrowPrefab, transform.position + new Vector3(0, 0.4f, 0), transform.rotation, transform); // Spawn a Arrow.
            if (gameObject.tag == "EnemyUnit")
                temp.GetComponent<Projectiles>().shotFrom = "Enemy";
            else if (gameObject.tag == "PlayerUnit")
                temp.GetComponent<Projectiles>().shotFrom = "Player";
        }
        else
            State = UnitState.Walking;
        //if (currentTarget.name == "Melee Unit") {
        //    currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage;
        //} else if (currentTarget.name == "Ranged Unit" || currentTarget.name == "Mage Unit" || currentTarget.name == "Tank Unit") {
        //    currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage * 0.5f;
        //} else if (currentTarget != null && currentTarget.name == "Player Castle" || currentTarget.name == "Enemy Castle") {
        //    currentTarget.GetComponent<Tower>().TakeDamage(aiDamage);
        //}
    }

    public void MageAttack()
    {
        if (curTarget != null)
        {
            GameObject temp = Instantiate(magicPrefab, transform.position + new Vector3(0, 0.4f, 0), transform.rotation, transform); // Spawn a Magic attack.
            if (gameObject.tag == "EnemyUnit")
                temp.GetComponent<Projectiles>().shotFrom = "Enemy";
            else if (gameObject.tag == "PlayerUnit")
                temp.GetComponent<Projectiles>().shotFrom = "Player";
        }
        else
            State = UnitState.Walking;
        //if (currentTarget.name == "Tank Unit") {
        //    currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage;
        //} else if (currentTarget.name == "Mage Unit" || currentTarget.name == "Ranged Unit" || currentTarget.name == "Tank Unit") {
        //    currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage * 0.5f;
        //} else if (currentTarget != null && currentTarget.name == "Player Castle" || currentTarget.name == "Enemy Castle") {
        //    currentTarget.GetComponent<Tower>().TakeDamage(aiDamage);
        //}
    }
    #endregion

    #region Set Team
    public void SetTeam(int teamNumber)
    {
        unitTeam = teamNumber;
        if (teamNumber == 1)
            gameObject.tag = "PlayerUnit";
        else if (teamNumber == 0)
            gameObject.tag = "EnemyUnit";
    }
    #endregion
}
