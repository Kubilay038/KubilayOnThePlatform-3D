using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class enemyController : MonoBehaviour
{
    GameObject[] goToDots;
    bool getTheDistanceOnceBetween = true;
    Vector3 distanceBetween;
    int distanceBetweenMeter = 0;
    bool forwardOrBack = true;
    GameObject character;
    RaycastHit2D ray;
    public LayerMask layerMask;
    int speed = 5;
    public Sprite frontSide;
    public Sprite backSide;
    SpriteRenderer spriteRenderer;
    public GameObject bullet;
    float fireTime = 0;
    void Start()
    {
        goToDots = new GameObject[transform.childCount];
        character = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < goToDots.Length; i++)
        {
            goToDots[i] = transform.GetChild(0).gameObject;
            goToDots[i].transform.SetParent(transform.parent);
        }
    }
    void FixedUpdate()
    {
        didTheEnemyWasSawMe();
        if (ray.collider.tag=="Player")
        {
            speed = 8;
            spriteRenderer.sprite = frontSide;
            Fire();
        }
        else
        {
            speed = 5;
            spriteRenderer.sprite = backSide;
        }

        goToDot();
    }
    void Fire()
    {
        fireTime += Time.deltaTime;
        if (fireTime > Random.Range(0.2f,1))
        {
            Instantiate(bullet,transform.position,Quaternion.identity);
            fireTime = 0;   
        }
    }
    void didTheEnemyWasSawMe()
    {
        Vector3 rayDirection = character.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position, rayDirection, 1000, layerMask);
    }
    void goToDot()
    {
        if (getTheDistanceOnceBetween)
        {
            distanceBetween = (goToDots[distanceBetweenMeter].transform.position - transform.position).normalized;
            getTheDistanceOnceBetween = false;
        }
        float distance = Vector3.Distance(transform.position, goToDots[distanceBetweenMeter].transform.position);
        transform.position += distanceBetween * Time.deltaTime * speed;
        if (distance < 0.5f)
        {
            getTheDistanceOnceBetween = true;
            if (distanceBetweenMeter == goToDots.Length - 1)
            {
                forwardOrBack = false;
            }
            else if (distanceBetweenMeter == 0)
            {
                forwardOrBack = true;
            }
            if (forwardOrBack)
            {
                distanceBetweenMeter++;
            }
            else
            {
                distanceBetweenMeter--;
            }
        }
    }
    public Vector2 getDirection()
    {
        return (character.transform.position - transform.position).normalized;
    }
#if UNITY_EDITOR
void OnDrawGizmos()
{
    for (int i = 0; i < transform.childCount; i++)
			{
                Gizmos.color=Color.red;
                Gizmos.DrawWireSphere(transform.GetChild(i).transform.position,1);
			}
            for (int i = 0; i < transform.childCount-1; i++)
			{
                Gizmos.color=Color.black;
                Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i+1).transform.position);
			}
}
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(enemyController))]
[System.Serializable]
class enemyControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        enemyController script = (enemyController)target;
        if (GUILayout.Button("Add"))
	{
        GameObject newobj = new GameObject();
        newobj.transform.parent=script.transform;
        newobj.transform.position = script.transform.position;
        newobj.name=script.transform.childCount.ToString();
	}
    EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMask"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("frontSide"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("backSide"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("bullet"));
    serializedObject.ApplyModifiedProperties();
    serializedObject.Update();
    }
}
#endif