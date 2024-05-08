using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange = 8f;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform randomTarget;
    public Transform[] randomTargetInCam;

    public float enemyNum;

    private Camera cam;

    void Start() {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>(); 
    }

    
    void FixedUpdate()
    {
        //parameter : position, range(scanned circle radius), casting dir, casting length, target layer
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        //randomTargetInCam = GetRandomTargetInCamera();
        randomTarget = GetRandomTarget();
    }

    public Transform GetRandomTarget()
    {
        Transform result = null;

        float diff = 100f;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;

            float curDiff = Vector3.Distance(myPos, targetPos);

            //Target out of range of the spinning weapon 
            //and the furthest enemy within detection range
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
                
            }
        }

        return result;
    }

    public List<Transform> GetRandomTargetInCamera(float upgrade) 
    {
        List<Transform> result = new List<Transform>();

        foreach(RaycastHit2D target in targets)
        {
            if(target.transform.GetComponent<Enemy>().attacking == true){
                Debug.Log("pass");
                continue;
            }
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;

            float curDiff = Vector3.Distance(myPos, targetPos);

            //Target out of range of the spinning weapon 
            //and the furthest enemy within detection range
            if(curDiff > 3f) 
            {

                //judge the target is within the screen
                Vector3 onScreen = cam.WorldToViewportPoint(targetPos);
                if(onScreen.x > 0 && onScreen.x < 1 && onScreen.y > 0 && onScreen.y < 1)
                {
                    result.Add(target.transform);

                    if(result.Count >= upgrade){
                        break;
                    }
                }
            }
        }


        return result;
    }
    
}
