using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player rolySidePrefab;
    private Player roly;

    public Transform levelsParent;
    private Level[] levels;

    private Transform worldMap;
    private Vector3 firstPos;

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

        firstPos = transform.position;

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
        if (roly)
        {
            float diff = roly.transform.position.y - transform.position.y;

            Vector3 follow = transform.position;

            if(diff > 3f || diff < -2f)
            {
                follow += new Vector3(0, diff/10f);
            }

            transform.position = follow;
        }
    }

    void ResetCamera()
    {
        transform.position = firstPos;
    }

    public void EnterLevel(int id)
    {
        worldMap.gameObject.SetActive(false);

        ResetCamera();

        roly = levels[id].StartLevel(rolySidePrefab);
    }

    public void ExitLevel()
    {
        ResetCamera();

        worldMap.gameObject.SetActive(true);

    }
}
