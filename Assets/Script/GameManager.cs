using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool isGamePaused = false;
    public int playerScore;

    public static string currentLevel;

    public List<GameObject> KeyCollection;
    public Transform durationGUI;
    public UnityEngine.UI.Text playerScoreGUI;
    public UnityEngine.UI.Text gameTimeGUI;

    private int AiSpawnGacha;
    public GameObject[] aiUnit;
    [SerializeField] private Transform enemySpawnTransform;
    [SerializeField] private PatternGenerator patternGenerator;

    void Start() {
        ChooseLevel(currentLevel);
        UpdateKeys(currentLevel);
    }

    void Update()
    {
        gameTimeGUI.text = "Time in\nGame :\n" + Time.time.ToString("0000");
    }

    public IEnumerator SpawnEnemyUnit()
    {
        while (!isGamePaused)
        {
            AiSpawnGacha = Random.Range(1, 19);

            if (AiSpawnGacha % 5 == 0)
            {
                int randomSpawn = Random.Range(0, aiUnit.Length);
                switch (randomSpawn)
                {
                    case 0:
                        GameObject temp = Instantiate(aiUnit[0], enemySpawnTransform.position, enemySpawnTransform.rotation);
                        temp.GetComponent<UnitControl>().SetTeam(0);
                        temp.name = "Melee Unit";
                        break;
                    case 1:
                        GameObject temp1 = Instantiate(aiUnit[1], enemySpawnTransform.position, enemySpawnTransform.rotation);
                        temp1.GetComponent<UnitControl>().SetTeam(0);
                        temp1.name = "Tank Unit";
                        break;
                    case 2:
                        GameObject temp2 = Instantiate(aiUnit[2], enemySpawnTransform.position, enemySpawnTransform.rotation);
                        temp2.GetComponent<UnitControl>().SetTeam(0);
                        temp2.name = "Mage Unit";
                        break;
                    case 3:
                        GameObject temp3 = Instantiate(aiUnit[3], enemySpawnTransform.position, enemySpawnTransform.rotation);
                        temp3.GetComponent<UnitControl>().SetTeam(0);
                        temp3.name = "Ranged Unit";
                        break;
                }

                //int randomSpawn2 = Random.Range(0, patternGenerator.playerUnit.Length);
                //switch (randomSpawn2)
                //{
                //    case 0:
                //        GameObject temp = Instantiate(aiUnit[0], patternGenerator.spawnTransform.position, patternGenerator.spawnTransform.rotation);
                //        temp.GetComponent<UnitControl>().SetTeam(1);
                //        temp.name = "Melee Unit";
                //        break;
                //    case 1:
                //        GameObject temp1 = Instantiate(aiUnit[1], patternGenerator.spawnTransform.position, patternGenerator.spawnTransform.rotation);
                //        temp1.GetComponent<UnitControl>().SetTeam(1);
                //        temp1.name = "Tank Unit";
                //        break;
                //    case 2:
                //        GameObject temp2 = Instantiate(aiUnit[2], patternGenerator.spawnTransform.position, patternGenerator.spawnTransform.rotation);
                //        temp2.GetComponent<UnitControl>().SetTeam(1);
                //        temp2.name = "Mage Unit";
                //        break;
                //    case 3:
                //        GameObject temp3 = Instantiate(aiUnit[3], patternGenerator.spawnTransform.position, patternGenerator.spawnTransform.rotation);
                //        temp3.GetComponent<UnitControl>().SetTeam(1);
                //        temp3.name = "Ranged Unit";
                //        break;
                //}
            }
            yield return new WaitForSeconds(1f);
        }
    }

    #region ChooseLevel
    public void ChooseLevel(string level)
    {
        switch (level)
        {
            case "1":
                AddToList(patternGenerator.Key, KeyCollection, 4); // A
                break;
            case "2":
                AddToList(patternGenerator.Key, KeyCollection, 9); // KA
                break;
            case "3":
                AddToList(patternGenerator.Key, KeyCollection, 14); // SA
                break;
            case "4":
                AddToList(patternGenerator.Key, KeyCollection, 19); // TA
                break;
            case "5":
                AddToList(patternGenerator.Key, KeyCollection, 24); // NA
                break;
            case "6":
                AddToList(patternGenerator.Key, KeyCollection, 29); // HA
                break;
            case "7":
                AddToList(patternGenerator.Key, KeyCollection, 34); // MA
                break;
            case "8":
                AddToList(patternGenerator.Key, KeyCollection, 37); // YA
                break;
            case "9":
                AddToList(patternGenerator.Key, KeyCollection, 42); // RA
                break;
            case "10":
                AddToList(patternGenerator.Key, KeyCollection, 45); // WA
                break;
        }
    }

    public void UpdateKeys(string level)
    {
        switch (level)
        {
            case "1":
                patternGenerator.spawnableKey = new List<int>() // A
                { 0, 1, 2, 3, 4 };
                break;
            case "2":
                patternGenerator.spawnableKey = new List<int>() // KA
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                break;
            case "3":
                patternGenerator.spawnableKey = new List<int>() // SA
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                break;
            case "4":
                patternGenerator.spawnableKey = new List<int>() // TA
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,16,17,18,19};
                break;
            case "5":
                patternGenerator.spawnableKey = new List<int>() // NA
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19,20,21,22,23,24 };
                break;
            case "6":
                patternGenerator.spawnableKey = new List<int>() // HA
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,25,26,27,28,29 };
                break;
            case "7":
                patternGenerator.spawnableKey = new List<int>() // MA
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,25,26,27,28,29,30,31,32,33,34 };
                break;
            case "8":
                patternGenerator.spawnableKey = new List<int>() // YA
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,25,26,27,28,29,30,31,32,33,34,35,36,37 };
                break;
            case "9":
                patternGenerator.spawnableKey = new List<int>() // RA
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42};
                break;
            case "10":
                patternGenerator.spawnableKey = new List<int>() // WA
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45 };
                break;

        }
    }
    #endregion

    void AddToList(List<GameObject> AddTo, List<GameObject> RefFrom, int forLoopLimit)
    {
        for (int i = 0; i <= forLoopLimit; i++)
        {
            AddTo.Add(RefFrom[i]);
        }
    }
}
