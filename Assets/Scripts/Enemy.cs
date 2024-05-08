using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //속도 조정을 위한 float 변수
    public float speed;

    //체력과 최대 체력
    //체력은 적이 맵에 있을때의 현재 체력, 최대 체력은 적을 스폰할때 부여할 초기 체력 
    public float hp;
    public float maxHp;

    public RuntimeAnimatorController[] animCon;

    //적이 따라갈 타겟 오브젝트(rigidbody)
    public Rigidbody2D target;

    //생존 유무를 판단하는 bool 변수
    bool isLive;
    public bool attacking;
    public bool isDamage;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer sprite;
    AudioSource audioSource;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attacking = false;
        audioSource = GameManager.instance.pool.GetComponent<AudioSource>();
    }
    
    void FixedUpdate()
    {
        //플레이어가 살아있는 상태에만 실행
        if (!GameManager.instance.isLive)
            return;

        //적이 살아있는 상태에만 실행
        if (!isLive)
            return;

        //타겟 위치 - 나의 위치를 통해 위치 차이를 구함
        Vector2 dirVec = target.position - rigid.position;
        //위치 정규화 및 속도 조정
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        //오브젝트 이동
        rigid.MovePosition(rigid.position + nextVec);
        //물리속도가 이동에 영향을 주지 않도록 속도 제거
        rigid.velocity = Vector2.zero;
    }

    //위치에 따라 바라보는 방향을 왼쪽/오른쪽으로 변경
    private void LateUpdate()
    {
        //플레이어가 살아있는 상태에만 실행
        if (!GameManager.instance.isLive)
            return;

       // sprite.flipX = target.position.x < rigid.position.x;
    }

    //오브젝트가 활성화 될 때, 초기화해야 하는 요소들을 초기화하는 함수
    void OnEnable()
    {
        //활성화 될 때 플레이어의 리지드바디를 타겟으로 삼음
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        hp = maxHp;
    }

    //스폰 데이터를 가져와서 적용하는 함수
    public void Init(SpawnData data)
    {
        //anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHp = data.hp;
        hp = data.hp;
    }


    //충돌이 일어났을 때 충돌한 오브젝트에 따라 정해진 행동을 수행하는 함수
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("sword") ||
        (collision.CompareTag("bullet") && collision.gameObject.transform.parent.GetComponent<Weapon>().id != 2))
            audioSource.Play();
        
        
        if(!collision.CompareTag("bullet") || !isLive || collision.gameObject.transform.parent.GetComponent<Weapon>().id == 3)
            return;
            
        if (attacking == true)
            return;

        else if(collision.gameObject.transform.parent.GetComponent<Weapon>().id == 2)
        {
            StartCoroutine(AttackedByRandom(collision.gameObject));
        }
        else 
        {
            Debug.Log(collision.GetComponent<Bullet>().damage);
            hp -= collision.GetComponent<Bullet>().damage;
            if (hp > 0)
            {
                Attacked();
            }
            else if (hp <= 0)
            {                  
                Dead();         
            }
        }

    }

    private void Attacked() {
        StartCoroutine(AttackedEffect());
    }
    IEnumerator AttackedEffect()
    {
        gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    IEnumerator AttackedByRandom(GameObject collision)
    {
        attacking = true;
        yield return new WaitForSeconds(1f);
        collision.SetActive(false);
        hp -= collision.GetComponent<Bullet>().damage;
        audioSource.Play();
        Debug.Log("damage enemy by weapon2");
        if (hp > 0)
        {
            Attacked();
        }
        else if (hp <= 0)
        {                  
            Dead();         
        }
        attacking = false;

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (attacking == true)
            return;

         if (collision.CompareTag("sword")) 
        {
            hp -= collision.GetComponent<Sword>().damage;

            if (hp > 0)
            {
                Attacked();
            }
            else if (hp <= 0)
            {
                Dead();
            }
            
            return;
        }

        if (collision.CompareTag("exp") || collision.gameObject.transform.parent.GetComponent<Weapon>().id != 3) 
            return;

        if (collision.gameObject.transform.parent.GetComponent<Weapon>().isDamage && isDamage )
        {
            hp -= collision.GetComponent<Bullet>().damage;

            isDamage = false;

            Debug.Log(collision.GetComponent<Bullet>().damage);

            if (hp > 0)
            {
                Attacked();
            }
            else if (hp <= 0)
            {
                Dead();
            }
        }
        else if (!collision.gameObject.transform.parent.GetComponent<Weapon>().isDamage)
        {
            isDamage = true;
        }
    }

    public void Dead()
    {        
        isLive = false;
        //coll.enabled = false;
        //rigid.simulated = false;
        //spriter.sortingOrder = 1;
        //anim.SetBool("Dead", true);
        
        Transform exp = GameManager.instance.pool.Get(8).transform;
        exp.position = transform.position;
        gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.SetActive(false);
        GameManager.instance.kill++;

        if(gameObject.CompareTag("boss"))
            GameManager.instance.GameClear();
    }
}
