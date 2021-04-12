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
    public UnityEngine.UI.Button addButton;

    void Start() {
        // SubmitEvent event = new InputField.SubmitEvent();
        // event.AddListener(AddPlayer);
        // playerInput.onSubmit = event;
    }

    public void AddPlayer() {
        List<Player> players = PlayerManager.Instance.players;
        string name = playerInput.text;
        if (players.Count < PlayerManager.Instance.MAX_PLAYER_NUM && name.Length > 0) {
            playerInput.text = "";
            PlayerManager.Instance.AddNewPlayer(name);
            int idx = players.Count - 1;

            if (idx < displayContainer.transform.childCount) {
                Player player = players[idx];
                Transform nameContainer = displayContainer.transform.GetChild(idx);
                TMP_Text text = nameContainer.GetComponent<TMP_Text>();

                text.text = player.player_name;
                text.color = player.player_color;
                nameContainer.gameObject.SetActive(true);
            }
        }

        if (players.Count >= PlayerManager.Instance.MIN_PLAYER_NUM) {
            startButton.interactable = true;
        }
        if (players.Count == PlayerManager.Instance.MAX_PLAYER_NUM) {
            addButton.interactable = false;
        }
    }

    public void LoadNextScene() {
        List<Player> players = PlayerManager.Instance.players;
        if (players.Count >= PlayerManager.Instance.MIN_PLAYER_NUM) {
            int currSceneNum = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currSceneNum + 1);
        }
    }
}
