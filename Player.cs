using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //移动速度
    public float speed = 3;
    //最大血量
    public float maxHp = 20;
    //变量，输入方向
    private Vector3 input;
    //是否死亡
    private bool dead = false;
    //血量
    private float hp;

    Weapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        weapon = GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        //将键盘的横向、纵向输入，保存在input中
        input =new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Debug.Log(input);
        bool fireKeyDown = Input.GetKeyDown(KeyCode.J);
        Debug.Log(fireKeyDown);
        bool fireKeyPressed = Input.GetKey(KeyCode.J);
        bool changeWeapon = Input.GetKeyDown(KeyCode.Q);

        if (!dead)
        {
            Move();
            weapon.Fire(fireKeyDown,fireKeyPressed);
            if (changeWeapon)
            {
                ChangeWeapon();
            }
        }
    }

    private void Move()
    {
        input = input.normalized;
        transform.position+= input * speed * Time.deltaTime;
        //令角色前方与移动方向一致
        if (input.magnitude>0.1f)
        {
            transform.forward=input;
        }
        //以上移动方式没有考虑阻挡，因此使用以下代码限制移动范围
        Vector3 temp = transform.position;
        const float BORDER = 20;
        if (temp.x < -BORDER)
        {
            temp.x = -BORDER;
        }
        if (temp.x > BORDER)
        {
            temp.x = BORDER;
        }

        if (temp.z<-BORDER)
        {
            temp.z=-BORDER;
        }
        if (temp.z > BORDER)
        {
            temp.z = BORDER;
        }
        transform.position = temp;
    }
    private void ChangeWeapon()
    {
        int w=weapon.Change();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            if (hp <= 0)
            {
                return;
            }

            hp--;
            if (hp<=0)
            {
                dead = true;
            }
        }
    }
}
