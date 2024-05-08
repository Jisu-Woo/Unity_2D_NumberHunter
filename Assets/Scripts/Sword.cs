using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Vector2 inputVec;
    Vector3 oriPos;
    Vector3 oriRot;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        oriPos = transform.localPosition;
        oriRot = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {

        if(inputVec.y < 0)
        {
            transform.localPosition = new Vector3(0, -0.5f, 0);
            transform.eulerAngles = new Vector3(0, 0, 90f);

        }
        else if(inputVec.y > 0)
        {
            transform.localPosition = new Vector3(0, 0.5f, 0);
            transform.eulerAngles = new Vector3(0, 0, -90f);

        }
        else if(inputVec.x < 0)
        {
            transform.localPosition = new Vector3(-0.5f, 0, 0);
            transform.eulerAngles = new Vector3(0, 0, 0);

        }
        else if(inputVec.x > 0)
        {
            transform.localPosition = new Vector3(0.5f, 0, 0);
            transform.eulerAngles = new Vector3(0, 0, 180f);
        }
        else 
        {
            transform.localPosition = oriPos;
            transform.eulerAngles = oriRot;
        }
    }
}
