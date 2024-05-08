using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingPlayer : MonoBehaviour
{

    RectTransform rect;

    // Start is called before the first frame update
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = GameManager.instance.player.transform.position;
        rect.position = Camera.main.WorldToScreenPoint(new Vector3(pos.x, pos.y-0.6f, pos.z));
    }
}
