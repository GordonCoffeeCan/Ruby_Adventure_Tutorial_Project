using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rig;

    public int maxHealth = 5;//最大生命值
    private int currentHealth;//当前生命值
    
    public int Health {
        get {
            return currentHealth;
        }
    }

    //Ruby无敌时间
    public float timeInvincible = 2f; //无敌时间常量
    public bool isInvicible = false;
    private float invicibleTimer = 0; //计时器

    //Shoot gap
    public float shootTimeGap = 0.5f;
    private float shootTimer = 0;

    private Vector2 lookDirection = new Vector2(1, 0);
    private Animator animator;

    public GameObject projectilePrefab;

    public AudioSource audioSource;
    public AudioSource walkAudioSource;

    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkSound;

    private Vector3 respawnPosition;
    private NPCDialog taskNPC;

    public ETCJoystick joystickLeft;
    private bool onMobileAction = false;
    private bool onMobileFire = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        currentHealth = maxHealth;
        //audioSource = this.GetComponent<AudioSource>();
        respawnPosition = this.transform.position;

        shootTimer = shootTimeGap;

        taskNPC = GameObject.FindGameObjectWithTag("TaskNPC").GetComponent<NPCDialog>();
    }

    // Update is called once per frame
    void Update()
    {

        //无敌时间计算
        if (isInvicible) {
            invicibleTimer -= Time.deltaTime;
            if(invicibleTimer <= 0) {
                isInvicible = false;
            }
        }

        //发射零件修理
        if (shootTimer > 0) {
            shootTimer -= Time.deltaTime;
        } else {
            if (Input.GetKeyDown(KeyCode.Space) || onMobileFire) {
                shootTimer = shootTimeGap;
                Launch();
            }
        }

        if(DistanceToNPC() <= 1.3f) {
            if (!taskNPC.dialogBox.activeSelf) {
                taskNPC.buttonIndicator.SetActive(true);
            } else {
                taskNPC.buttonIndicator.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.F) || onMobileAction) {
                taskNPC.DisplayDialog();
            }
        } else {
            taskNPC.buttonIndicator.SetActive(false);
        }
    }

    private void FixedUpdate() {
#if UNITY_STANDALONE
        //玩家输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
#endif

#if UNITY_ANDROID
        //PlayerInput Mobile
        float horizontal = joystickLeft.axisX.axisValue;
        float vertical = joystickLeft.axisY.axisValue;
#endif

        Vector2 move = new Vector2(horizontal, vertical);

        //当前玩家输入的某个轴向值不为0
        if(!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0)) {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            walkAudioSource.clip = walkSound;
            if (!walkAudioSource.isPlaying) {
                walkAudioSource.Play();
            }
        } else {
            walkAudioSource.Stop();
        }
        //动画控制
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //移动控制
        Vector2 position = transform.position;
        //position.x += speed * horizontal * Time.deltaTime;
        //position.y += speed * vertical * Time.deltaTime;

        //Ruby位置水平方向移动、垂直方向移动
        position += speed * move * Time.deltaTime;

        //this.transform.position = position;

        rig.MovePosition(position);
    }

    public void ChangeHealth(int amount) {

        //受到伤害
        if(amount < 0) {
            if (isInvicible) {
                return;
            }
            isInvicible = true;
            animator.SetTrigger("Hit");
            PlaySound(playerHit);
            invicibleTimer = timeInvincible;
        }

        //改变生命值（增加或减少）
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        GameManager.Instance.SetValue(currentHealth / (float)maxHealth);

        if(currentHealth <= 0) {
            Respawn();
        }
    }

    private void Launch() {
        if (!GameManager.Instance.hasTask) {
            return;
        }
        GameObject projectileObject = Instantiate(projectilePrefab, rig.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
        PlaySound(attackSoundClip);
    }

    public void PlaySound(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip);
    }

    private void Respawn() {
        ChangeHealth(maxHealth);
        this.transform.position = respawnPosition;
    }

    //Measure the distance between player and task NPC;
    private float DistanceToNPC() {
        return Vector3.Distance(this.transform.position, taskNPC.transform.position);
    }

    public void MobileActionPressed() {
        onMobileAction = true;
    }

    public void MobileActionReleased() {
        onMobileAction = false;
    }

    public void MobileFirePressed() {
        onMobileFire = true;
    }

    public void MobileFireReleased() {
        onMobileFire = false;
    }
}
