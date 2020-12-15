using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image mask;
    float originalSize;

    public static UIHealthBar Instance {
        get;
        private set;
    }

    public bool hasTask = false;
    //public bool isCompleteTask = false;
    public int fixedNum = 0;

    private void Awake() {
        Instance = this;
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
}
