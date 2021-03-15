using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0.0f, -30.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        //transform.position = player.transform.position + offset;
        transform.position = new Vector3(player.transform.position.x, 0.0f, player.transform.position.z);

    }
}
