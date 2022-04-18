using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    public Sprite[] waitAnim;
    public Sprite[] jumpAnim;
    public Sprite[] walkAnim;
    public Text healText;
    public Text coinText;
    int heal = 1000;

    SpriteRenderer spriteRenderer;
    int waitAnimMeter = 0;
    int walkAnimMeter = 0;
    int coinMeter = 0;
    Rigidbody2D physical;
    Vector3 vec;
    Vector3 cameraLastPos;
    Vector3 cameraFirstPos;
    bool justoncejump = true;

    float horizontal = 0;
    float waitAnimTime = 0;
    float walkAnimTime = 0;

    GameObject camera;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        physical = GetComponent<Rigidbody2D>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraFirstPos = camera.transform.position - transform.position;
        healText.text = "Heal  " + heal;
        coinText.text = "20  " + coinMeter;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (justoncejump)
            {
                physical.AddForce(new Vector2(0, 300));
                justoncejump = false;
            }
            
        }
    }
    void FixedUpdate()
    {
        CharacterMove();
        Animation();
    }
    void LateUpdate()
    {
        CameraControl();
    }
    void CharacterMove()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vec = new Vector3(horizontal * 10, physical.velocity.y, 0);
        physical.velocity = vec;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        justoncejump = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag=="Bullet")
        {
            heal--;
            healText.text = "Heal   " + heal;
        }
        if (col.gameObject.tag == "Enemy")
        {
            heal-=10;
            healText.text = "Heal   " + heal;
        }
        if (col.gameObject.tag == "Saw")
        {
            heal -= 10;
            healText.text = "Heal   " + heal;
        }
        if (col.gameObject.tag == "takeHeal")
        {
            heal += 10;
            col.GetComponent<BoxCollider2D>().enabled = false;
            col.GetComponent<HealController>().enabled = true;
            healText.text = "Heal   " + heal;
            Destroy(col.gameObject,3);
        }
        if (col.gameObject.tag == "Coin")
        {
            coinMeter++;
            coinText.text = "20 - " + coinMeter;
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "Water")
        {
            heal = 0;
            healText.text = "Heal   " + heal;
        }
    }
    void CameraControl()
    {
        cameraLastPos = cameraFirstPos + transform.position;
        camera.transform.position = Vector3.Lerp(camera.transform.position, cameraLastPos, 0.07f);
    }
    void Animation()
    {
        if (justoncejump)
        {
            if (horizontal == 0)
            {
                waitAnimTime += Time.deltaTime;
                if (waitAnimTime > 0.3f)
                {
                    spriteRenderer.sprite = waitAnim[waitAnimMeter++];
                    if (waitAnimMeter == waitAnim.Length)
                    {
                        waitAnimMeter = 0;
                    }
                    waitAnimTime = 0;
                }

            }
            else if (horizontal > 0)
            {
                walkAnimTime += Time.deltaTime;
                if (walkAnimTime > 0.03f)
                {
                    spriteRenderer.sprite = walkAnim[walkAnimMeter++];
                    if (walkAnimMeter == walkAnim.Length)
                    {
                        walkAnimMeter = 0;
                    }
                    walkAnimTime = 0;
                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                walkAnimTime += Time.deltaTime;
                if (walkAnimTime > 0.03f)
                {
                    spriteRenderer.sprite = walkAnim[walkAnimMeter++];
                    if (walkAnimMeter == walkAnim.Length)
                    {
                        walkAnimMeter = 0;
                    }
                    walkAnimTime = 0;
                }
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        else
        {
            if (physical.velocity.y > 0)
            {
                spriteRenderer.sprite = jumpAnim[0];
            }
            else
            {
                spriteRenderer.sprite = jumpAnim[1];
            }
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
