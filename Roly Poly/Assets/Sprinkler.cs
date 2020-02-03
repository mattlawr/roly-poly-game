using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour
{
    public float rotationSpeed;

    bool online = false;

    // Update is called once per frame
    void Update()
    {
        if (!online)
        {
            return;
        }

        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    public void On()
    {
        online = true;
        
        foreach(Transform child in transform)
        {
            if (child)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
