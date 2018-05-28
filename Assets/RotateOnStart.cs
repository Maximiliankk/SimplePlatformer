using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnStart : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        if(Random.Range(0,2) == 1)
        {
            RotateLevel();
        }
    }

    private void RotateLevel()
    {
        var childTransforms = this.GetComponentsInChildren<Transform>();
        foreach (var c in childTransforms)
        {
            if (c.gameObject.tag != "DoNotRotate")
            {
                c.Rotate(Vector3.up, 180);
            }
        }
    }
}
