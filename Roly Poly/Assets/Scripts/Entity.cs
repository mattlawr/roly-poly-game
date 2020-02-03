using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject fxPrefab;
    public Animator anim;
    public int health = 10;

    public AudioClip[] sounds = new AudioClip[0];

    const bool _FACELEFT = true;    // Which direction sprite images face

    [Header("Events")]
    [Space]

    public UnityEvent OnHit;  // For TakeDamage

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Can be overridden
    public void TakeDamage(int dmg)
    {
        health -= dmg;

        OnHit.Invoke();
    }
    public virtual void TakeDamage(int dmg, Vector2 force)
    {
        // Push this
        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }

        if(sounds.Length > 0)
        {
            GameManager.instance.PlaySingle(sounds[Random.Range(0, sounds.Length)].name);
        }

        TakeDamage(dmg);
    }

    // -1 = left, +1 = right
    public void SetSpriteDirection(float d)
    {
        if (!sprite) { return; }

        bool unflipped = sprite.flipX != _FACELEFT;

        if (d > 0 && unflipped)
        {
            sprite.flipX = _FACELEFT;
        }
        else if (d < 0 && !unflipped)
        {
            sprite.flipX = !_FACELEFT;
        }
    }

    public bool FacingRight()
    {
        return (sprite.flipX == _FACELEFT);
    }

    public Vector3 GetForward()
    {
        return (FacingRight() ? transform.right : -transform.right);
    }

    public void SpawnFX()
    {
        if (!fxPrefab) { return; }

        GameObject fxObj = (GameObject)Instantiate(fxPrefab, transform.position, Quaternion.identity);
    }
}
