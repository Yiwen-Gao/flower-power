using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoreManager : MonoBehaviour {
    private int idx;
    public GameObject displayContainer;
    public UnityEngine.UI.Button nextButton;

    void Start() {
        idx = 0;
    }

    public void DisplayNextFaction() {
        Transform currFactionContainer = displayContainer.transform.GetChild(idx++);
        currFactionContainer.gameObject.SetActive(false);
        
        if (idx < displayContainer.transform.childCount) {
            Transform nextFactionContainer = displayContainer.transform.GetChild(idx);
            nextFactionContainer.gameObject.SetActive(true);
            if (idx == displayContainer.transform.childCount - 1) {
                nextButton.interactable = false;
            }
        }
    }

    public void LoadNextScene() {
        int currSceneNum = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneNum + 1);
    }
}
