using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Level types? Level win condition?

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

        player = Instantiate(roly, transform.position+Vector3.up*3f, Quaternion.identity);

        return player;
    }

    public void LeaveLevel()
    {
        GameObject.Destroy(player.gameObject);

        GameManager.instance.ExitLevel();

        gameObject.SetActive(false);
    }
}
