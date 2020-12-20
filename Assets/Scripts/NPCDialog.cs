using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour {
    public GameObject dialogBox;
    public float displayTime = 4f;
    public GameObject questMark;
    public GameObject questionMark;
    public GameObject buttonIndicator;
    private float timerDisplay;
    public Text dialogText;
    public AudioClip QuestCompletedClip;
    public AudioSource audioSource;
    private bool hasPlayed = false;

    // Start is called before the first frame update
    void Start() {
        dialogBox.SetActive(false);
        buttonIndicator.SetActive(false);
        questionMark.SetActive(false);
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
        GameManager.Instance.hasTask = true;

        if (!GameManager.Instance.taskComplete) {
            questMark.SetActive(false);
            questionMark.SetActive(true);
        }

        if (GameManager.Instance.fixedNum >= 6) {
            dialogText.text = "My robots are all fixed up! Thank you Ruby!";
            GameManager.Instance.taskComplete = true;
            questionMark.SetActive(false);
            if (!hasPlayed) {
                audioSource.PlayOneShot(QuestCompletedClip);
                hasPlayed = true;
            }
        }
    }
}
