using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player rolySidePrefab;
    private Player roly;
    public GameObject soundPrefab;

    public Transform levelsParent;
    private Level[] levels;
    int currLevel = 0;
    public AudioClip mainSong;

    public HeartBar hearts;

    private Transform worldMap;
    private Vector3 firstPos;

    public float cameraSpeed = 10f;

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

        PlaySong(mainSong);
    }

    // Update is called once per frame
    void Update()
    {
        if (roly)
        {
            float diff = roly.transform.position.y - transform.position.y;

            Vector3 follow = transform.position;

            if(diff > 3f || diff < 4f)
            {
                follow += new Vector3(0, diff/10f);
            }

            transform.position = Vector3.Lerp(transform.position, follow, cameraSpeed * Time.deltaTime);
        }
    }

    public void PlaySong(AudioClip clip)
    {
        if (!clip) { return; }

        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

    void ResetCamera()
    {
        transform.position = firstPos;
    }

    public void EnterLevel(int id)
    {
        worldMap.gameObject.SetActive(false);

        ResetCamera();

        currLevel = id;

        PlaySong(levels[id].song);
        roly = levels[id].StartLevel(rolySidePrefab);
    }

    public void ExitLevel()
    {
        ResetCamera();

        HeartsAdd(10);

        PlaySong(mainSong);
        worldMap.gameObject.SetActive(true);

    }

    public void HeartsAdd(int d)
    {
        hearts.UpdateHearts(d);
    }

    public IEnumerator StopLevel()
    {
        yield return new WaitForSeconds(3f);

        levels[currLevel].LeaveLevel();

        yield return null;
    }

    //******** SOUND

    public void PlaySong()
    {

    }

    public void PlaySingle(string soundName)
    {
        if (soundName == "") { return; }
        GameObject fxObj = (GameObject)Instantiate(soundPrefab, Vector3.zero, Quaternion.identity);
        if (GameObject.Find("_effects")) { fxObj.transform.parent = GameObject.Find("_effects").transform; }

        AudioSource asource = fxObj.GetComponent<AudioSource>();
        AudioClip a = (AudioClip)Resources.Load(soundName);
        asource.clip = a;
        fxObj.GetComponent<SelfDestruct>().duration = asource.clip.length;
        asource.spatialBlend = 0f;

        asource.volume = 1f * 0.7f;

        asource.Play();
    }

}
