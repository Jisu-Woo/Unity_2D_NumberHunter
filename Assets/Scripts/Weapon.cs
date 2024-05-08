using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public float upgrade;
    public float speed;

    public bool isDamage = false;

    public float gunTimer;
    public float lightTimer;
    public float floorTimer;
    public float meleeTimer;

    Transform floor;

    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어가 살아있는 상태에만 실행
        if (!GameManager.instance.isLive)
            return;

        //id 0 : rotating weapon
        switch (id)
        {
            //case 0: rotating weapon
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); 
                break;

            //case 1: gun, speed: AttackInterval
            case 1:
                gunTimer += Time.deltaTime;

                if (gunTimer > speed - upgrade)
                {
                    gunTimer = 0f;
                    Fire();
                }
                break;
            //case 2: Lightning, speed: AttackInterval
            case 2:
                lightTimer += Time.deltaTime;

                if (lightTimer > speed)
                {
                    //if(player.scanner.randomTargetInCam)
                    if(player.scanner.GetRandomTargetInCamera(upgrade).Count == upgrade)
                    {
                        lightTimer = 0f;
                        RandomAttack();
                            
                    }
                }
                break;

            //case 3: Damage Floor / dot damage
            case 3:
                floorTimer += Time.deltaTime;

                if (floorTimer > speed)
                {
                    isDamage = true;                   
                }
                
                if (floorTimer > speed + 0.1f)
                {
                    isDamage = false;
                    floorTimer = 0.1f;
                }
             
                break;
            case 4:
                meleeTimer += Time.deltaTime;

                if (meleeTimer > speed)
                {
                    meleeTimer = 0f;
                    FireMelee();
                }
                break;
            default:
                
                    
                break;
        }

    }

    //if level up, damage and count will upgrade 
    public void LevelUp(float damage, float upgrade)
    {
        this.damage += damage;
        this.upgrade += upgrade;

        if (id == 0)
            Batch();
        else if (id == 3)
            FloorBatch();
        else if (id == 5)
            AddExp();
        else if (id == 6)
            AddHp(upgrade);
    }

    //initialize damage and counts of weapons
    public void Init(ItemData data)
    {
        // basic set
        name = "Weapon " + data.itemID;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // property set
        id = data.itemID;
        damage = data.baseDamgae;
        upgrade = data.baseUpgrade;

        // initialize prefab id in pooling manager for independence
        for (int index=0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = -150;
                Batch();
                break;
            case 1:
                speed = 3;
                break;
            case 2:
                speed = 5;
                upgrade = 1;
                break;  
            case 3:
                speed = 1;
                FloorBatch();
                break;
            case 4:
                speed = 5;
                break;
            case 5:
                AddExp();
                break;
            case 6:
                AddHp(upgrade);
                break;
            default:
                speed = 2f;
                break;
        }
    }

    void Batch()
    {
        for (int index = 0; index < upgrade; index++) 
        {
            Transform bullet;
            
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(10).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
          
            Vector3 rotVec = Vector3.forward * 360 * index / upgrade;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(0, damage, -1, Vector3.zero);
        }
    }

    void Fire()
    {
       
            if (!player.scanner.randomTarget)
                return;

            Vector3 targetPos = player.scanner.randomTarget.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bullet = GameManager.instance.pool.Get(11).transform;
            bullet.parent = transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(1, damage, upgrade, dir);
    }
            

    void FireMelee()
    {
        if (!player.scanner.randomTarget)
            return;

        for (int index = 0; index < upgrade; index++)
        {
            Vector3 targetPos = transform.position + new Vector3(Random.Range(-1f, 1f), 1, 0);
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bullet = GameManager.instance.pool.Get(14).transform;
            bullet.parent = transform;
            bullet.position = transform.position;
            bullet.transform.Rotate(0, 0, Time.deltaTime * 100f);
            bullet.GetComponent<Bullet>().Init(4, damage, upgrade, dir);
        }
        
    }

    void RandomAttack()
    {
        List<Transform> randomTargetInCam = player.scanner.GetRandomTargetInCamera(upgrade);
        for(int i = 0; i<randomTargetInCam.Count; i++){
            Vector3 targetPos = randomTargetInCam[i].position;
            Vector3 instanPos = new Vector3(targetPos.x, targetPos.y, targetPos.z);
            Transform bullet = GameManager.instance.pool.Get(12).transform;
            bullet.parent = transform;
            bullet.position = instanPos + new Vector3(0,0,1);
            bullet.GetComponent<Bullet>().Init(2, damage, upgrade, Vector3.zero);
        }
    }

    void FloorBatch()
    {
        if (floor == null)
        {
            floor = GameManager.instance.pool.Get(13).transform;
            floor.parent = transform;
            floor.position = transform.position;
            floor.GetComponent<Bullet>().Init(3, damage, upgrade, Vector3.zero);

            floor.localPosition = Vector3.zero;
            
        }
        else if (floor != null)
            floor.localScale = new Vector3(transform.localScale.x + upgrade, transform.localScale.y + upgrade, 0);       

    }

    void AddExp()
    {
        
        GameManager.instance.AddExp(upgrade);

    }

    void AddHp(float upgrade)
    {
        
        GameManager.instance.AddHp((int)upgrade);

    }

}
