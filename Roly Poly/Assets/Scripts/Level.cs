using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Sprinkler worldSprinkler;

    public AudioClip song;

    public Transform startObj;

    private Player player;

    const float _MAX_X = 6.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player && (player.transform.position.x > _MAX_X || player.transform.position.x < -_MAX_X))
        {
            LeaveLevel();
        }
    }

    public Player StartLevel(Player roly)
    {

        gameObject.SetActive(true);// enable level

        Vector3 startPos = transform.position + Vector3.up * 3f;

        if (startObj)
        {
            startPos = startObj.position;
        }

        player = Instantiate(roly, startPos, Quaternion.identity);

        return player;
    }

    public void LeaveLevel()
    {
        GameObject.Destroy(player.gameObject);

        GameManager.instance.ExitLevel();

        gameObject.SetActive(false);
    }

    public void CompleteLevel()
    {
        //GameObject.Destroy(player.gameObject);
        player.controller.Stun(true);

        GameManager.instance.StartCoroutine("StopLevel");

        if (!worldSprinkler)
        {
            return;
        }

        worldSprinkler.On();
    }
}
