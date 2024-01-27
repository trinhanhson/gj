using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public Joystick inputController;
    public float speed;
    public float strafeSpeed;
    public float jumpForce;

    public Rigidbody hips;
    public bool isGrounded;
    private float inputX;
    private float inputY;
    private bool buttonAttackClicked = false;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        hips = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // inputX = Input.GetAxis("Horizontal");
        // inputY = Input.GetAxis("Vertical");
        inputX = inputController.Horizontal;
        inputY = inputController.Vertical;
        
        Vector3 direction = new Vector3(inputX, 0, inputY);

        if (direction.magnitude > 0.1f)
        {
            hips.AddForce(direction * speed);
        }

        if (buttonAttackClicked)
        {
            if (isGrounded)
            {
                hips.AddForce(new Vector3(direction.x ,jumpForce, direction.z));
                buttonAttackClicked = false;
                isGrounded = false;
            }
        }
    }
    
    public void OnClickAttack()
    {
        buttonAttackClicked = true;
    }
    
}
