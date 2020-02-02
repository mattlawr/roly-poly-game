using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player rolySidePrefab;
    public Transform levelsParent;
    private Level[] levels;

    private Transform worldMap;

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        worldMap = transform.GetChild(0);// First child

        if (levelsParent)
        {
            levels = new Level[levelsParent.childCount];
            for(int i = 0; i < levelsParent.childCount; i++)
            {
                if (!levelsParent.GetChild(i).GetComponent<Level>())
                {
                    Debug.LogError("No Level class!");
                    break;
                }

                levels[i] = levelsParent.GetChild(i).GetComponent<Level>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterLevel(int id)
    {
        worldMap.gameObject.SetActive(false);

        levels[id].StartLevel(rolySidePrefab);
    }

    public void ExitLevel()
    {
        worldMap.gameObject.SetActive(true);

    }
}
