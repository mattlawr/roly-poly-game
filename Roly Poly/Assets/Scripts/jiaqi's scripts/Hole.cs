using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hole : MonoBehaviour
{
    public GameObject Button;

    public bool open = false;

    [Header("Events")]
    [Space]

    public UnityEvent Select;  // For Entering

    private void Update()
    {
        if(Button.activeSelf && Input.GetButtonDown("Jump"))
        {
            Select.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (open && collision.tag == "Player")
        {
            Button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (open && collision.tag == "Player")
        {
            Button.SetActive(false);
        }
    }
}
