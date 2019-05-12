using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AutoMove : MonoBehaviour {

    public enum AiState
    {
        Walking,
        Attacking,
        Merging
    }
    [Header("State")]
    public AiState State;

    [Header("Ai Stats")]
    public float aiHealth = 100f;
    public float aiDamage = 20f;
    public float aiSpeed = 0.5f;
    public int unitLevel = 1;

    [Header("Merge Field")]
    public bool mergeMelee;
    public bool mergeRanged;
    public bool mergeMage;
    public bool mergeTank;
    public GameObject mergeTarget;
    public GameObject currentTarget;
    public GameObject unitFlag;

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private GameObject playerFlag;
    [SerializeField] private GameObject enemyFlag;
    private bool hasFlag = false;
    private Animator anim;

	void Start () {
        anim = GetComponent<Animator>();

        State = AiState.Walking;
    }

	void Update () {

        #region Ai State
        if (PatternGenerator.hasStarted && State == AiState.Walking)
        {
            anim.SetBool("Attacking", false);
            transform.position += transform.forward * Time.deltaTime * aiSpeed;
        }
        else if (State == AiState.Attacking)
        {
            if (name == "Tank Unit" || name == "Melee Unit")
            {
                if (Vector3.Distance(transform.position, currentTarget.transform.position) < 1.5f)
                    anim.SetBool("Attacking", true);
                else
                    anim.SetBool("Attacking", false);
            }
            else
            {
                anim.SetBool("Attacking", true);
            }
            if (currentTarget == null)
            {
                State = AiState.Walking;
            }
        }
        #endregion

        #region Death State
        if (aiHealth <= 0)
        {
            Destroy(gameObject);
        }
        #endregion

        // Merge Functions
        if (mergeMelee && State == AiState.Merging && mergeTarget != null) {
            Vector3 direction = mergeTarget.transform.position - transform.position;
            transform.position += direction * Time.deltaTime * 5f;
            if (Vector3.Distance(transform.position, mergeTarget.transform.position) < 0.1f) {
                Destroy(gameObject);
            }
        }
        else if (State == AiState.Merging && mergeTarget == null)
        {
            State = AiState.Walking;
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerUnit") && gameObject.CompareTag("EnemyUnit") && (gameObject.name == "Ranged Unit" || gameObject.name == "Mage Unit"))
        {
            currentTarget = other.gameObject;
            State = AiState.Attacking;
        }
        else if (other.CompareTag("EnemyUnit") && gameObject.CompareTag("PlayerUnit") && (gameObject.name == "Ranged Unit" || gameObject.name == "Mage Unit"))
        {
            currentTarget = other.gameObject;
            State = AiState.Attacking;
        }

        // Merge Functions
        if (other.CompareTag("PlayerUnit") && !gameObject.CompareTag("EnemyUnit")) {
            if (gameObject.name == "Melee Unit" && other.gameObject.name == "Melee Unit") {
                mergeTarget = other.gameObject;
                MergeUnits(other.gameObject, false);
            }
            if (gameObject.name == "Tank Unit" && other.gameObject.name == "Tank Unit")
            {
                mergeTarget = other.gameObject;
                MergeUnits(other.gameObject, false);
            }
        }
        else if (other.CompareTag("EnemyUnit") && !gameObject.CompareTag("PlayerUnit")) {
            if (gameObject.name == "Melee Unit" && other.gameObject.name == "Melee Unit") {
                mergeTarget = other.gameObject;
                MergeUnits(other.gameObject, true);
            }
            else if (gameObject.name == "Tank Unit" && other.gameObject.name == "Tank Unit")
            {
                mergeTarget = other.gameObject;
                MergeUnits(other.gameObject, true);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerUnit") && gameObject.CompareTag("EnemyUnit") && (gameObject.name == "Melee Unit" || gameObject.name == "Tank Unit"))
        {
            currentTarget = other.gameObject;
            if (Vector3.Distance(transform.position, currentTarget.transform.position) < 1.5f)
                State = AiState.Attacking;
        }
        else if (other.CompareTag("EnemyUnit") && gameObject.CompareTag("PlayerUnit") && (gameObject.name == "Melee Unit" || gameObject.name == "Tank Unit"))
        {
            currentTarget = other.gameObject;
            if (Vector3.Distance(transform.position, currentTarget.transform.position) < 1.5f)
                State = AiState.Attacking;
        }

        // Merge Functions
        if (other.CompareTag("PlayerUnit") && !gameObject.CompareTag("EnemyUnit")) {
            if (gameObject.name == "Mage Unit" && other.gameObject.name == "Mage Unit") {
                mergeTarget = other.gameObject;
                if (Vector3.Distance(transform.position, other.gameObject.transform.position) < 0.5f) {
                    MergeUnits(other.gameObject, false);
                }
            }
            if (gameObject.name == "Ranged Unit" && other.gameObject.name == "Ranged Unit") {
                mergeTarget = other.gameObject;
                if (Vector3.Distance(transform.position, mergeTarget.transform.position) < 0.5f) {
                    MergeUnits(other.gameObject, false);
                }
            }
        }

        else if (other.CompareTag("EnemyUnit") && !gameObject.CompareTag("PlayerUnit"))
        {
            if (gameObject.name == "Mage Unit" && other.gameObject.name == "Mage Unit")
            {
                mergeTarget = other.gameObject;
                if (Vector3.Distance(transform.position, other.gameObject.transform.position) < 0.5f)
                {
                    MergeUnits(other.gameObject, true);
                }
            }
            if (gameObject.name == "Ranged Unit" && other.gameObject.name == "Ranged Unit")
            {
                mergeTarget = other.gameObject;
                if (Vector3.Distance(transform.position, mergeTarget.transform.position) < 0.5f)
                {
                    MergeUnits(other.gameObject, true);
                }
            }
        }
    }

    public void UpdateFlag(bool spawnFlag, GameObject flagPrefab) {
        if (spawnFlag && !hasFlag) {
            hasFlag = true;
            aiHealth += 40;
            unitFlag = Instantiate(flagPrefab, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity, transform);
            unitFlag.transform.eulerAngles = new Vector3(0, -90, 0);
            ++unitLevel;
            unitFlag.GetComponentInChildren<UnityEngine.UI.Text>().text = unitLevel.ToString("0");
        } else if (hasFlag) {
            aiHealth += 40;
            ++unitLevel;
            unitFlag.GetComponentInChildren<UnityEngine.UI.Text>().text = unitLevel.ToString("0");
        }
    }

    void MergeUnits(GameObject other, bool isEnemy)
    {
        if (!isEnemy)
        {
            if (mergeTarget.transform.position.x > transform.position.x)
            {
                mergeTarget.GetComponent<AI_AutoMove>().UpdateFlag(true, playerFlag);
                mergeMelee = true;
                State = AiState.Merging;
                GetComponent<Collider>().enabled = false;
            }
        }
        else if (isEnemy)
        {
            if (mergeTarget.transform.position.x < transform.position.x)
            {
                mergeTarget.GetComponent<AI_AutoMove>().UpdateFlag(true, enemyFlag);
                mergeMelee = true;
                State = AiState.Merging;
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    #region Attack Methods
    public void MeleeAttack() {
        if (currentTarget != null)
        {
            if (currentTarget.name == "Mage Unit")
            {
                currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage;
            }
            else if (currentTarget.name == "Melee Unit" || currentTarget.name == "Ranged Unit" || currentTarget.name == "Tank Unit")
            {
                currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage * 0.5f;
            }
            else if (currentTarget != null && currentTarget.name == "Player Castle" || currentTarget.name == "Enemy Castle")
            {
                currentTarget.GetComponent<Tower>().TakeDamage(aiDamage);
            }
        }
        else
            State = AiState.Walking;
    }

    public void TankAttack()
    {
        if (currentTarget != null)
        {
            if (currentTarget.name == "Mage Unit")
            {
                currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage;
            }
            else if (currentTarget.name == "Tank Unit" || currentTarget.name == "Mage Unit" || currentTarget.name == "Ranged Unit")
            {
                currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage * 0.5f;
            }
            else if (currentTarget != null && currentTarget.name == "Player Castle" || currentTarget.name == "Enemy Castle")
            {
                currentTarget.GetComponent<Tower>().TakeDamage(aiDamage);
            }
        }
        else
            State = AiState.Walking;
    }

    public void RangedAttack() {
        if (currentTarget != null)
        {
            GameObject temp = Instantiate(arrowPrefab, transform.position + new Vector3(0, 0.4f, 0), transform.rotation, transform); // Spawn a Arrow.
            if (gameObject.tag == "EnemyUnit")
                temp.GetComponent<Projectiles>().shotFrom = "Enemy";
            else if (gameObject.tag == "PlayerUnit")
                temp.GetComponent<Projectiles>().shotFrom = "Player";
        }
        else
            State = AiState.Walking;


        //if (currentTarget.name == "Melee Unit") {
        //    currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage;
        //} else if (currentTarget.name == "Ranged Unit" || currentTarget.name == "Mage Unit" || currentTarget.name == "Tank Unit") {
        //    currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage * 0.5f;
        //} else if (currentTarget != null && currentTarget.name == "Player Castle" || currentTarget.name == "Enemy Castle") {
        //    currentTarget.GetComponent<Tower>().TakeDamage(aiDamage);
        //}
    }

    public void MageAttack() {
        if (currentTarget != null)
        {
            GameObject temp = Instantiate(magicPrefab, transform.position + new Vector3(0, 0.4f, 0), transform.rotation, transform); // Spawn a Magic attack.
            if (gameObject.tag == "EnemyUnit")
                temp.GetComponent<Projectiles>().shotFrom = "Enemy";
            else if (gameObject.tag == "PlayerUnit")
                temp.GetComponent<Projectiles>().shotFrom = "Player";
        }
        else
            State = AiState.Walking;

;
        //if (currentTarget.name == "Tank Unit") {
        //    currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage;
        //} else if (currentTarget.name == "Mage Unit" || currentTarget.name == "Ranged Unit" || currentTarget.name == "Tank Unit") {
        //    currentTarget.GetComponent<AI_AutoMove>().aiHealth -= aiDamage * 0.5f;
        //} else if (currentTarget != null && currentTarget.name == "Player Castle" || currentTarget.name == "Enemy Castle") {
        //    currentTarget.GetComponent<Tower>().TakeDamage(aiDamage);
        //}
    }
    #endregion
}
