using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    public int hearts = 10;

    private SpriteRenderer[] heartSprites;

    // Start is called before the first frame update
    void Start()
    {
        if (true)
        {
            heartSprites = new SpriteRenderer[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).GetComponent<SpriteRenderer>())
                {
                    Debug.LogError("No hearts!");
                    break;
                }

                heartSprites[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            }
        }

        UpdateHearts(0);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateHearts(0);
    }

    public void UpdateHearts(int d)
    {
        hearts += d;
        if(hearts > heartSprites.Length) { hearts = heartSprites.Length; }

        for (int i = 0; i < heartSprites.Length; i++)
        {
            if(i < hearts)
            {
                heartSprites[i].color = Color.white;
            } else
            {
                heartSprites[i].color = Color.black;
            }
        }
    }
}
