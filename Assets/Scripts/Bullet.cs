using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //������ �� ������
    public float damage;

    public float per;

    Rigidbody2D rigid;

    int id;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Dead();
    }


    //� ����� ���� �÷��̾�� �Ÿ��� �־����� ��Ȱ��ȭ
    void Dead()
    {
        Transform target = GameManager.instance.player.transform;
        Vector3 targetPos = target.position;
        float dir = Vector3.Distance(targetPos, transform.position);
        if (dir > 20f)
            this.gameObject.SetActive(false);
    }


    //������ �ʱ�ȭ�ϴ� �Լ�
    public void Init(int id, float damage, float per, Vector3 dir)
    {
        this.id = id;
        this.damage = damage;
        this.per = per;

        switch (id)
        {
            case 0:
                break;
            case 1:
                rigid.velocity = dir * 10f;
                break;
            case 2:
                //GetComponent<BoxCollider2D>().enabled = false;
                rigid.velocity = dir * 10f;
                break; 
            case 3:
                break;
            case 4:
                rigid.velocity = dir * 10f;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( id == 1 && (collision.CompareTag("enemy")|| collision.CompareTag("boss")))
        {
                rigid.velocity = Vector2.zero;
                gameObject.SetActive(false);
        }
       
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if ( id == 2 && (collision.CompareTag("enemy")|| collision.CompareTag("boss")))
        {
            transform.position = collision.transform.position;
        }
    }

}
