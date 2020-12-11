﻿using System.Collections;
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
    public float timeInvincible = 2f; //无敌时间常亮
    public bool isInvicible = false;
    private float invicibleTimer = 0; //计时器

    private Vector2 lookDirection = new Vector2(1, 0);
    private Animator animator;

    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvicible) {
            invicibleTimer -= Time.deltaTime;
            if(invicibleTimer <= 0) {
                isInvicible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Launch();
        }
    }

    private void FixedUpdate() {
        //玩家输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        //当前玩家输入的某个轴向值不为0
        if(!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0)) {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

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
            invicibleTimer = timeInvincible;
        }

        //改变生命值（增加或减少）
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    private void Launch() {
        GameObject projectileObject = Instantiate(projectilePrefab, rig.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
    }
}