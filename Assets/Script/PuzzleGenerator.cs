using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGenerator : MonoBehaviour {

    [Header("Input String")]
    public string inputString;
    [Header("Player Stats")]
    public int playerScore;
    public float powerUpCounter;

    [Header("Misc")]
    private bool hasStarted = false;
    private bool startGenerator = true;
    private int randomKey;

    public List<GameObject> currentActiveKey;

    public List<int> spawnableKey;
    public List<GameObject> KeyCollection;

    [SerializeField] private Text inputFieldGUI;
    [SerializeField] private Text playerScoreGUI;
    [SerializeField] private Text gameTimeGUI;
    [SerializeField] private GameObject powerUpGUI;

    private AudioSource keyboardSource;

    void Start()
    {
        spawnableKey = new List<int>()
        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45 };

        keyboardSource = GetComponent<AudioSource>();
    }

    void Update()
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
                if (inputString == "restart")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("LevelMenu");
                }
                else if (inputString == "start" && !hasStarted)
                {
                    hasStarted = true;
                    startGenerator = true;
                    StartCoroutine(StartGame(.5f)); // Start Generator
                }
                else if (inputString == "power" && hasStarted)
                {
                    if (powerUpCounter == 100)
                        UsePower();
                    else
                        print("Not Enough PowerUp");
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
                if (inputString.Length < 10)
                {
                    PlayKeyboardAudio();
                    inputString += alphabet;
                    inputFieldGUI.text = "Input : " + inputString; // Input GUI
                }
            }
        }
        #endregion

        #region Lose Statement
        if (currentActiveKey.Count == 46)
        {
            currentActiveKey.Clear();
            StartCoroutine(YouLost());
        }
        #endregion
    }

    #region Use Power
    void UsePower()
    {
        powerUpCounter = 0;
        startGenerator = false;

        for (int i = 0; i < currentActiveKey.Count; i++)
        {
            HideKey(currentActiveKey[i].transform);
        }
        currentActiveKey.Clear();
        startGenerator = true;
    }
    #endregion

    #region ConfirmKey
    IEnumerator ConfirmKey(int i)
    {
        inputString = "";
        inputFieldGUI.text = "Input : " + inputString; // Input GUI

        playerScore += 10;
        playerScoreGUI.text = "Score :\n" + playerScore;

        if (powerUpCounter < 100)
        {
            powerUpCounter += 1;
            powerUpGUI.transform.localScale = new Vector3(1, powerUpCounter / 100, 1);
        }

        HideKey(currentActiveKey[i].transform);
        currentActiveKey[i].GetComponent<AudioSource>().Play();
        currentActiveKey.Remove(currentActiveKey[i]);
        yield return null;
    }
    #endregion

    #region Show & Hide Key
    void ShowKey(Transform key)
    {
        key.GetComponent<TurnTile>().isActive = true;
        key.GetComponent<TurnTile>().ShowWord = true;
    }

    void HideKey(Transform key)
    {
        key.GetComponent<TurnTile>().isActive = false;
        key.GetComponent<TurnTile>().ShowWord = false;
    }
    #endregion

    #region Keyboard Audio
    void PlayKeyboardAudio()
    {
        keyboardSource.pitch = Random.Range(1, 2);
        keyboardSource.PlayOneShot(keyboardSource.clip);
    }
    #endregion

    #region Start Game
    IEnumerator StartGame(float _interval)
    {
        float interval = _interval;
        while (startGenerator)
        {
            _interval = interval;

            randomKey = spawnableKey[Random.Range(0, KeyCollection.Count)];

            if (!KeyCollection[randomKey].GetComponent<TurnTile>().isActive)
            {
                ShowKey(KeyCollection[randomKey].transform);
                currentActiveKey.Add(KeyCollection[randomKey]);
            }
            else
            {
                _interval = 0;
            }

            yield return new WaitForSeconds(_interval);
        }
    }
    #endregion

    #region You Lost
    IEnumerator YouLost()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelMenu");
    }
    #endregion
}
