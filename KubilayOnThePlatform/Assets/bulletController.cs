using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    enemyController enemy;
    Rigidbody2D physical;
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<enemyController>();
        physical = GetComponent<Rigidbody2D>();
        physical.AddForce(enemy.getDirection()*1000);
    }
}
