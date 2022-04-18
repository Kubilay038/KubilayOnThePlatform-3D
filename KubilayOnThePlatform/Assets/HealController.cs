using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealController : MonoBehaviour
{
    public Sprite []keyFrames;
    int keyFramesMeter = 0;
    SpriteRenderer spriteRenderer;
    float time = 0;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.09f)
        {
            spriteRenderer.sprite = keyFrames[keyFramesMeter++];
            if (keyFrames.Length == keyFramesMeter)
            {
                keyFramesMeter = keyFrames.Length-1;
            }
            time = 0;
        }
    }
}
