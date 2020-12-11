using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        RubyController rubyController = collision.GetComponent<RubyController>();

        //判断触发检测的物体是否有RubyController
        if (rubyController != null) {
            //判断Ruby是否满血状态
            if (rubyController.Health < rubyController.maxHealth) {
                rubyController.ChangeHealth(1);
                Destroy(this.gameObject);
            }
        }
    }
}
