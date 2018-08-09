using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    private int lives = 3;
    [SerializeField]
    private float speed = 3.0F;
    [SerializeField]
    private float jumpForce = 8.0F;
    [SerializeField]
    private GameObject bullet;
    private float BulletSpeed = 7;
    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }


new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private bool IsGrounded;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        

    }

    private void FixedUpdate()
    {      
        CheckGround();
    }

    private void Update()
    {
        if (IsGrounded) State = CharState.Idle;
        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Run();
        if (IsGrounded && Input.GetButtonDown("Jump")) Jump();
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        sprite.flipX = direction.x < 0.0F;
        if (IsGrounded) State = CharState.Run;
    } 

    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        State = CharState.Jump;
    }

    private void Faint()
    {
        if (lives <= 0) State = CharState.Faint;

    }

    private void Shoot()
    { // ЧТО ТУТ ПРОИСХОДИТ ВТФ? 
        Vector3 position = new Vector3(transform.position.x + 0.8F, transform.position.y + 0.65F, transform.position.z);
        var newBullet = Instantiate(bullet, position, bullet.transform.rotation);
    
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);

    }

    private T Instantiate<T>(GameObject bullet, Vector3 position, Quaternion rotation)
    {
        throw new NotImplementedException();
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);
        IsGrounded = colliders.Length > 1;
        if (!IsGrounded) State = CharState.Jump;
    }


}

enum CharState
{
    Idle,
    Run,
    Jump, 
    Dizzy,
    Faint
}
