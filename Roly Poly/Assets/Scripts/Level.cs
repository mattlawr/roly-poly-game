using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Level types? Level win condition?

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(Player roly)
    {

        gameObject.SetActive(true);// enable level

        player = Instantiate(roly, transform.position, Quaternion.identity);
    }

    public void CompleteLevel()
    {
        GameObject.Destroy(player.gameObject);
    }
}
