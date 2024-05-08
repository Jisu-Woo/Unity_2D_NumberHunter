using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //키보드 입력 값을 받을 벡터 변수
    public Vector2 inputVec;

    //속도 조절을 위한 float 변수
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
        //키보드 입력을 받아 벡터에 저장
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        //위치 보정을 위해 벡터를 정규화하고 속도 값을 조정
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;

        //rigidbody를 이용한 위치 이동
        rigid.MovePosition(rigid.position + nextVec);
    }

    //플레이어의 애니메이션 스프라이트 제어
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
