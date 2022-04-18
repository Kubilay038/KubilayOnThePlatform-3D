using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Saw : MonoBehaviour
{
    GameObject[] goToDots;
    bool getTheDistanceOnceBetween = true;
    Vector3 distanceBetween;
    int distanceBetweenMeter = 0;
    bool forwardOrBack = true;
    void Start()
    {
        goToDots = new GameObject[transform.childCount];

        for (int i = 0; i < goToDots.Length; i++)
        {
            goToDots[i] = transform.GetChild(0).gameObject;
            goToDots[i].transform.SetParent(transform.parent);
        }
    }
    void FixedUpdate()
    {
        transform.Rotate(0, 0, -5);
        goToDot();
    }
    void goToDot()
    {
        if (getTheDistanceOnceBetween)
        {
            distanceBetween = (goToDots[distanceBetweenMeter].transform.position - transform.position).normalized;
            getTheDistanceOnceBetween = false;
        }
        float distance = Vector3.Distance(transform.position, goToDots[distanceBetweenMeter].transform.position);
        transform.position += distanceBetween * Time.deltaTime * 10;
        if (distance < 0.5f)
        {
            getTheDistanceOnceBetween = true;
            if (distanceBetweenMeter == goToDots.Length-1)
            {
                forwardOrBack = false;
            }
            else if(distanceBetweenMeter == 0)
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
[CustomEditor(typeof(Saw))]
[System.Serializable]
class sawEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Saw script = (Saw)target;
        if (GUILayout.Button("Add"))
	{
        GameObject newobj = new GameObject();
        newobj.transform.parent=script.transform;
        newobj.transform.position = script.transform.position;
        newobj.name=script.transform.childCount.ToString();
	}

    }
}
#endif