using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour {

    public TMP_InputField playerInput;
    public GameObject displayContainer;

    public UnityEngine.UI.Button startButton;
    private List<string> playerNames;

    public int MAX_PLAYER_NUM = 6;

    void Start() {
        // SubmitEvent event = new InputField.SubmitEvent();
        // event.AddListener(AddPlayer);
        // playerInput.onSubmit = event;

        playerNames = new List<string>();
    }

    public void AddPlayer() {
        if (playerNames.Count < MAX_PLAYER_NUM) {
            string name = playerInput.text;
            playerInput.text = "";
            
            playerNames.Add(name);
            int idx = playerNames.Count - 1;
            if (idx < displayContainer.transform.childCount) {
                Transform nameContainer = displayContainer.transform.GetChild(idx);
                nameContainer.GetComponent<TMP_Text>().text = name;
                nameContainer.gameObject.SetActive(true);
            }
        }

        if (playerNames.Count == MAX_PLAYER_NUM) {
            startButton.interactable = true;
        }
    }

    public void StartGame() {
        if (playerNames.Count == MAX_PLAYER_NUM) {
            int currSceneNum = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currSceneNum + 1);
        }
    }
}
