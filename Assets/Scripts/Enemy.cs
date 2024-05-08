using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //�ӵ� ������ ���� float ����
    public float speed;

    //ü�°� �ִ� ü��
    //ü���� ���� �ʿ� �������� ���� ü��, �ִ� ü���� ���� �����Ҷ� �ο��� �ʱ� ü�� 
    public float hp;
    public float maxHp;

    public RuntimeAnimatorController[] animCon;

    //���� ���� Ÿ�� ������Ʈ(rigidbody)
    public Rigidbody2D target;

    //���� ������ �Ǵ��ϴ� bool ����
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
        //�÷��̾ ����ִ� ���¿��� ����
        if (!GameManager.instance.isLive)
            return;

        //���� ����ִ� ���¿��� ����
        if (!isLive)
            return;

        //Ÿ�� ��ġ - ���� ��ġ�� ���� ��ġ ���̸� ����
        Vector2 dirVec = target.position - rigid.position;
        //��ġ ����ȭ �� �ӵ� ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        //������Ʈ �̵�
        rigid.MovePosition(rigid.position + nextVec);
        //�����ӵ��� �̵��� ������ ���� �ʵ��� �ӵ� ����
        rigid.velocity = Vector2.zero;
    }

    //��ġ�� ���� �ٶ󺸴� ������ ����/���������� ����
    private void LateUpdate()
    {
        //�÷��̾ ����ִ� ���¿��� ����
        if (!GameManager.instance.isLive)
            return;

       // sprite.flipX = target.position.x < rigid.position.x;
    }

    //������Ʈ�� Ȱ��ȭ �� ��, �ʱ�ȭ�ؾ� �ϴ� ��ҵ��� �ʱ�ȭ�ϴ� �Լ�
    void OnEnable()
    {
        //Ȱ��ȭ �� �� �÷��̾��� ������ٵ� Ÿ������ ����
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        hp = maxHp;
    }

    //���� �����͸� �����ͼ� �����ϴ� �Լ�
    public void Init(SpawnData data)
    {
        //anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHp = data.hp;
        hp = data.hp;
    }


    //�浹�� �Ͼ�� �� �浹�� ������Ʈ�� ���� ������ �ൿ�� �����ϴ� �Լ�
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
