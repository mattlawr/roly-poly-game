using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float duration = 1f;
    float timer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration && duration > 0)
        {
            Destruct();
        }
    }

    public void setTo(float dur)
    {
        timer = 0;
        duration = dur;
    }

    /// <summary>
    /// Main self destruct function
    /// </summary>
    public void Destruct()
    {
        Destroy(gameObject);
    }
}
