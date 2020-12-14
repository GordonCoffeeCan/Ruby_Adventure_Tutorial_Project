using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour {
    public GameObject dialogBox;
    public float displayTime = 4f;
    private float timerDisplay;

    // Start is called before the first frame update
    void Start() {
        dialogBox.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if(timerDisplay > 0) {
            timerDisplay -= Time.deltaTime;
        } else {
            dialogBox.SetActive(false);
        }
    }

    public void DisplayDialog() {
        dialogBox.SetActive(true);
        timerDisplay = displayTime;
    }
}
