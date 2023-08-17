using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField]
    [Serializable]
    public struct Range
    {
        public float min, max;
        public Range(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [Header("移动速度")]
    public float moveSpeed = 100;

    [Header("跳跃力度")]
    public float jumpForce = 10;

    [Header("重力")]
    public float gravity = 10f;

    [Header("鼠标灵敏度")]
    public float mouseSensitivity = 100;

    [Header("俯仰角范围")]
    public Range angleRange = new Range(-90, 90);

    [Header("平滑镜头")]
    public bool smoothCamera = false;

    [Header("镜头阻尼值")]
    [Range(0, 10)]
    public float damper = 5;

    private Vector3 direction;
    //是否在地面上
    public bool isGround;
    private float myGravity;
    //摄像机旋转角度
    private Vector2 currentViewpointAngle;
    private Vector2 targetViewpointAngle;
    //人物旋转角度
    private Vector2 currentPlayerAngle;
    private Vector2 targetPlayerAngle;
    private Rigidbody rb;
    private GameObject eyes;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        eyes = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        //PlayerMove();

        ViewpointControl();
    }
    private void LateUpdate()
    {
        

    }
    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        //bool isJump = Input.GetButton("Jump");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("空格按下!");

        }
        direction = (transform.forward * moveZ + transform.right * moveX).normalized;

        if (isGround)//接触地
        {
            myGravity = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                isGround = false;
                //myGravity += jumpForce;
                rb.AddForce(transform.up*jumpForce);
            }
        }
        else
        {
            myGravity -= gravity * Time.fixedDeltaTime;
            
        }
        //direction.y = myGravity;
        //Debug.Log(direction.y + "重力速度");
        rb.AddForce(direction*moveSpeed);

        //rb.velocity = direction * moveSpeed * Time.deltaTime;


    }

    private void ViewpointControl()
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");
        targetPlayerAngle.y += moveX * mouseSensitivity * Time.deltaTime;
        targetViewpointAngle.x -= moveY * mouseSensitivity * Time.deltaTime;
        //摄像机 上下视角范围限制。
        targetViewpointAngle.x = Mathf.Clamp(targetViewpointAngle.x, angleRange.min, angleRange.max);

        if (smoothCamera)
        {
            //平滑转动
            currentPlayerAngle = Vector2.Lerp(currentPlayerAngle, targetPlayerAngle, damper * Time.deltaTime);
            currentViewpointAngle = Vector2.Lerp(currentViewpointAngle, targetViewpointAngle, damper * Time.deltaTime);
        }
        else
        {
            //实时转动
            currentPlayerAngle = targetPlayerAngle;
            currentViewpointAngle = targetViewpointAngle;
        }
        transform.rotation = Quaternion.Euler(currentPlayerAngle);
        eyes.transform.localRotation = Quaternion.Euler(currentViewpointAngle);

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            isGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            isGround = false;
        }
    }
}

