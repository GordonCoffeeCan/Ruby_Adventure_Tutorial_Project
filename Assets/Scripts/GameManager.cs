using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image mask;
    float originalSize;

    public static GameManager Instance {
        get;
        private set;
    }

    public bool hasTask = false;
    public bool taskComplete = false;
    public int fixedNum = 0;

    public GameObject MobileUI;

    private void Awake() {
        Instance = this;
        Platforms();
    }

    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //设置当前UI血条显示值
    public void SetValue(float fillPercent) {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * fillPercent);
    }

    //Detect the platform of runing devices
    private void Platforms() {
#if UNITY_STANDALONE
        MobileUI.SetActive(false);
#endif

#if UNITY_EDITOR
        MobileUI.SetActive(false);
#endif

#if UNITY_ANDROID
        MobileUI.SetActive(true);
#endif
    }
}
