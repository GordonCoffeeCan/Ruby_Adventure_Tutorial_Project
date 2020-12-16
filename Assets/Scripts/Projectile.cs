using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rig;

    public GameObject hitEffect;

    private void Awake() {
        rig = this.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update() {
        if(this.transform.position.magnitude > 100) {
            Destroy(this.gameObject);
        }
    }

    public void Launch(Vector2 direction, float force) {
        rig.AddForce(direction * force);
    }

    //Projectile touches any obstacle destory itself;
    private void OnCollisionEnter2D(Collision2D collision) {
        Instantiate(hitEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
