using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Ű���� �Է� ���� ���� ���� ����
    public Vector2 inputVec;

    //�ӵ� ������ ���� float ����
    public float speed;

    public Scanner scanner;

    Rigidbody2D rigid;

    public Enemy enemy;

    public float healTime;
    public int healHp=10;

    public AudioSource audioSource;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); 
        scanner = GetComponent<Scanner>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        //Ű���� �Է��� �޾� ���Ϳ� ����
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        //��ġ ������ ���� ���͸� ����ȭ�ϰ� �ӵ� ���� ����
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;

        //rigidbody�� �̿��� ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);
    }

    //�÷��̾��� �ִϸ��̼� ��������Ʈ ����
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (gameObject.tag == "player" && collision.collider.CompareTag("enemy"))
        {
           GameManager.instance.hp -= 1;

            if (GameManager.instance.hp > 0)
            {
                
            }
            else if (GameManager.instance.hp <= 0)
            {
                GameManager.instance.GameOver();
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (gameObject.tag == "player" && collision.collider.CompareTag("enemy"))
        {
            audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "player" && collision.CompareTag("exp"))
        {
            collision.gameObject.SetActive(false);
            GameManager.instance.GetExp();
        }
       
    }

}
