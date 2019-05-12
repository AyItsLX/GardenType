using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternGenerator : MonoBehaviour {

    public int spawnLimit = 6;
    public List<GameObject> Key;
    public List<GameObject> currentActiveKey;
    public List<int> spawnableKey = new List<int>() { 0, 1, 2, 3, 4 };

    private int randomized;
    private int randomKey;
    private bool flipKey = true;
    public bool useInputField = false;
    [SerializeField] private AudioSource keyboardSource;
    [SerializeField] private string inputString;
    [SerializeField] private UnityEngine.UI.Text inputFieldGUI;

    public GameManager gameManager;

    public static bool hasStarted;
    public Transform spawnTransform;
    public GameObject[] playerUnit;

    public bool startTime;
    public float maxTime = 15;
    public float minTime;

    void Start()
    {
        keyboardSource = GetComponent<AudioSource>();
        hasStarted = true; // REMEMBER TO DELETE THIS.
        minTime = maxTime;

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        gameManager.StartCoroutine(gameManager.SpawnEnemyUnit());
        hasStarted = true;
        randomized = Random.Range(1, 6);
        StartCoroutine(FlipKey(0.1f)); // Start Generator
    }

    void Update () {
        if (useInputField)
        {
            #region Input Field
            foreach (char alphabet in Input.inputString)
            {
                if (alphabet == '\b') // Has Backspace or Delete been pressed?
                {
                    if (inputString.Length != 0)
                    {
                        PlayKeyboardAudio();
                        inputString = inputString.Substring(0, inputString.Length - 1);
                        inputFieldGUI.text = "Input : " + inputString; // Input GUI
                    }
                }
                else if ((alphabet == '\n') || (alphabet == '\r')) // Has Enter or Return been pressed?
                {
                    PlayKeyboardAudio();

                    //if (inputString == "restart")
                    //{
                    //    hasStarted = false;
                    //    UnityEngine.SceneManagement.SceneManager.LoadScene("LevelMenu");
                    //}
                    //else if (inputString == "reset")
                    //{
                    //    StartCoroutine(ResetKey(true));
                    //}
                    //else if (inputString == "start")
                    //{
                    //    //gameManager.StartCoroutine(gameManager.SpawnEnemyUnit());
                    //    //hasStarted = true;
                    //    //randomized = Random.Range(1, 6);
                    //    //StartCoroutine(FlipKey(0.1f)); // Start Generator
                    //}
                    if (inputString == "level")
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelMenu");
                    }
                    else
                    {
                        for (int i = currentActiveKey.Count - 1; i >= 0; i--)
                        {
                            if (inputString.ToUpper() == currentActiveKey[i].name) // Checks for correct answer
                            {
                                StartCoroutine(ConfirmKey(i));
                            }
                        }
                    }
                    inputString = "";
                    inputFieldGUI.text = "Input : " + inputString; // Input GUI
                }
                else
                {
                    if (inputString.Length < 10) {
                        PlayKeyboardAudio();
                        inputString += alphabet;
                        inputFieldGUI.text = "Input : " + inputString; // Input GUI
                    }
                }
            }
            #endregion
        }

        if (startTime)
        {
            minTime -= Time.deltaTime;
            Vector3 tempVec = gameManager.durationGUI.localScale;
            gameManager.durationGUI.localScale = new Vector3(tempVec.x, minTime / maxTime, tempVec.z);

            if (minTime <= 0 || !startTime)
            {
                print("Summon DUDE");
                minTime = 0;
                gameManager.durationGUI.localScale = new Vector3(1, 1, 1);
                StartCoroutine(ResetKey(true));
            }
        }
    }

    #region Keyboard Audio
    void PlayKeyboardAudio()
    {
        keyboardSource.pitch = Random.Range(1, 2);
        keyboardSource.PlayOneShot(keyboardSource.clip);
    }
    #endregion

    IEnumerator ConfirmKey(int i)
    {
        inputString = "";
        inputFieldGUI.text = "Input : " + inputString; // Input GUI
        gameManager.playerScore += 10;
        gameManager.playerScoreGUI.text = "Score :\n" + gameManager.playerScore;
        HideKey(currentActiveKey[i].transform);
        currentActiveKey[i].GetComponent<AudioSource>().Play();
        currentActiveKey.Remove(currentActiveKey[i]);

        if (hasStarted && currentActiveKey.Count == 0)
        {
            startTime = false;
            gameManager.durationGUI.localScale = new Vector3(1, 1, 1);
            SummonPlayerUnit();
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ResetKey(false));
        }
    }

    #region Summon Player Unit
    void SummonPlayerUnit()
    {
        int randomSpawn2 = Random.Range(0, gameManager.aiUnit.Length);
        switch (randomSpawn2)
        {
            case 0:
                GameObject temp = Instantiate(gameManager.aiUnit[0], spawnTransform.position, spawnTransform.rotation);
                temp.GetComponent<UnitControl>().SetTeam(1);
                temp.name = "Melee Unit";
                break;
            case 1:
                GameObject temp1 = Instantiate(gameManager.aiUnit[1], spawnTransform.position, spawnTransform.rotation);
                temp1.GetComponent<UnitControl>().SetTeam(1);
                temp1.name = "Tank Unit";
                break;
            case 2:
                GameObject temp2 = Instantiate(gameManager.aiUnit[2], spawnTransform.position, spawnTransform.rotation);
                temp2.GetComponent<UnitControl>().SetTeam(1);
                temp2.name = "Mage Unit";
                break;
            case 3:
                GameObject temp3 = Instantiate(gameManager.aiUnit[3], spawnTransform.position, spawnTransform.rotation);
                temp3.GetComponent<UnitControl>().SetTeam(1);
                temp3.name = "Ranged Unit";
                break;
        }
    }
    #endregion

    #region Reset Key
    IEnumerator ResetKey(bool clearKeys)
    {
        if (clearKeys)
        {
            for (int i = currentActiveKey.Count - 1; i >= 0; i--)
            {
                HideKey(currentActiveKey[i].transform);
            }
            currentActiveKey.Clear();
        }
        flipKey = true;
        gameManager.UpdateKeys(GameManager.currentLevel);
        randomized = Random.Range(1, 6);
        StartCoroutine(FlipKey(0.1f)); // Start Generator
        yield return null;
    }
    #endregion

    #region Flip Key
    IEnumerator FlipKey(float interval)
    {
        while (flipKey)
        {
            --randomized;
            RandomizedKey();
            if (randomized == 0)
            {
                flipKey = false;
                minTime = maxTime;
                startTime = true;
            }
            yield return new WaitForSeconds(interval);
        }
    }
    #endregion

    void RandomizedKey()
    {
        randomKey = spawnableKey[Random.Range(0, spawnableKey.Count)];
        ShowKey(Key[randomKey].transform); // ShowKey Method
        spawnableKey.Remove(randomKey);
        currentActiveKey.Add(Key[randomKey]);
    }

    void ShowKey(Transform key)
    {
        key.GetComponent<Key>().ShowWord = true;
    }

    void HideKey(Transform key)
    {
        key.GetComponent<Key>().ShowWord = false;
    }
}
