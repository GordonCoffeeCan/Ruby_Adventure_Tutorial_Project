using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour {
    public GameObject dialogBox;
    public float displayTime = 4f;
    private float timerDisplay;
    public Text dialogText;
    public AudioClip QuestCompletedClip;
    public AudioSource audioSource;
    private bool hasPlayed = false;

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
        UIHealthBar.Instance.hasTask = true;
        if (UIHealthBar.Instance.fixedNum >= 6) {
            dialogText.text = "谢谢你Ruby，你真是太棒了！";
            if (!hasPlayed) {
                audioSource.PlayOneShot(QuestCompletedClip);
                hasPlayed = true;
            }
        }
    }
}
