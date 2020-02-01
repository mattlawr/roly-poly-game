using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public SpriteRenderer sprite;

    public int health = 10;

    const bool _FACELEFT = false;    // Which direction sprite images face

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        return (sprite.flipX == !_FACELEFT);
    }
}
