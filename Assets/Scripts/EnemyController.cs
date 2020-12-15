using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speed = 3f;
    public bool vertical;
    private int direction = 1;

    public float changeTime = 3f;
    private float timer;

    public ParticleSystem smokeEffect;

    private Rigidbody2D rig;

    private Animator animator;

    private AudioSource audioSource;
    public AudioClip fixedSound;
    public AudioClip[] hitSounds;
    public GameObject hitEffectParticle;

    //机器人是否故障
    private bool broken = true;

    // Start is called before the first frame update
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        timer = changeTime;
        //animator.SetFloat("MoveX", direction);
        //animator.SetBool("Vertical", vertical);
        PlayMoveAnimation();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken) {
            //已修好，不再移动
            return;
        }

        timer -= Time.deltaTime;
        if(timer < 0) {
            direction *= -1;
            //animator.SetFloat("MoveX", direction);
            PlayMoveAnimation();
            timer = changeTime;
        }
    }

    private void FixedUpdate() {
        Vector2 position = rig.position;

        if (vertical) {
            position.y += speed * Time.deltaTime * direction;
        } else {
            position.x += speed * Time.deltaTime * direction;
        }

        rig.MovePosition(position);
    }

    //碰撞检测
    private void OnCollisionEnter2D(Collision2D collision) {
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if(rubyController != null) {
            rubyController.ChangeHealth(-1);
        }
    }

    //控制移动动画的方法
    private void PlayMoveAnimation() {
        if (vertical) {//垂直轴向动画控制
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        } else {//水平轴向动画控制
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
    }

    //修复机器人的方法
    public void Fix() {
        Instantiate(hitEffectParticle, this.transform.position, Quaternion.identity);
        broken = false;
        rig.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        int randomNum = Random.Range(0, 2);
        audioSource.Stop();
        audioSource.volume = 1;
        audioSource.PlayOneShot(hitSounds[randomNum]);
        Invoke("PlayFixedSound", 0.5f);
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    private void PlayFixedSound() {
        audioSource.PlayOneShot(fixedSound);
    }
}
