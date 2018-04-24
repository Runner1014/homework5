/** 
 * 这个文件是实现飞碟的飞行动作 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFlyAction : SSAction
{
    private float acceleration; //重力加速度=9.8f

    private float horizontalSpeed; //飞碟水平方向的速度
    private float verticalSpeed;   //飞碟竖直方向的速度

    private Vector3 direction; //飞碟的初始飞行方向

    Rigidbody rigidbody;
    DiskData disk;

    public override void Start()
    {
        disk = gameobject.GetComponent<DiskData>();
        enable = true;
        acceleration = 9.8f;
        verticalSpeed = 0;
        horizontalSpeed = disk.speed;
        direction = disk.direction;
        rigidbody = this.gameobject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.velocity = horizontalSpeed * direction;
        }
    }

    // Update is called once per frame  
    public override void Update()
    {
        if (gameobject.activeSelf)
        {
            verticalSpeed += Time.deltaTime * acceleration;

            transform.Translate(direction * horizontalSpeed * Time.deltaTime); //水平运动

            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime); //竖直运动

            Destroy();
        }

    }

    public override void FixedUpdate()
    {

        if (gameobject.activeSelf)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        if (this.transform.position.y < -4)
        {
            this.destroy = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }
    }

    public static CCFlyAction GetCCFlyAction()
    {
        CCFlyAction action = ScriptableObject.CreateInstance<CCFlyAction>();
        return action;
    }
}