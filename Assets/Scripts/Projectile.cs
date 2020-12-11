using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rig;

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

    private void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("Hit gameobject is " + collision.gameObject);

        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController != null) {
            enemyController.Fix();
        }

        Destroy(this.gameObject);
    }
}
