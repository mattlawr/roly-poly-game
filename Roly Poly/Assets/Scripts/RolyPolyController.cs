using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RolyPolyController : MonoBehaviour
{
    public float speed = 2f;
    public float rollStrength = 10f;
    [Range(0, .3f)] public float smoothing = .05f;
    [Range(0, 1f)] public float airControl = 0.5f;
    //public LayerMask groundLayers;

    public ParticleSystem dust;
    public TrailRenderer trail;

    private Player player;
    private Rigidbody2D rb;
    private Vector2 upAnchor = Vector3.up;
    private Vector3 velocity = Vector3.zero;

    bool anchored = false;

    [System.NonSerialized]
    public bool rolling = false;
    float rollCharge = 0f;

    private Vector2 lastMove = Vector3.zero;
    private bool correctCrawl = false;

    private float gravity = 0f;

    [System.NonSerialized]
    public bool stunned = false;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;  // For FX

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        gravity = rb.gravityScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        rollCharge -= Time.fixedDeltaTime;

        if (stunned)
        {
            return;
        }

        if (!rolling && !anchored)
        {
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position - transform.up*0.4f, 0.12f);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    //print("anchor! STAY");
                    SetAnchor(transform.up, transform.position);
                    return;
                }
            }

            if(rb.velocity.magnitude > 0.1f)
            {
                return;
            }

            colliders = Physics2D.OverlapCircleAll(transform.position - Vector3.up * 0.2f, 0.3f);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    //print("anchor! Ground");
                    SetAnchor(Vector3.up, transform.position);
                    return;
                }
            }
        }

        // check wall
        if (anchored)
        {
            Vector3 dir = player.GetForward();

            Ray r = new Ray(transform.position-(transform.up*0.1f), dir.normalized);
            Debug.DrawRay(r.origin, r.direction, Color.red);

            RaycastHit2D rc = Physics2D.Raycast(r.origin, r.direction, 0.3f, LayerMask.GetMask("Default"));

            if (rc)
            {
                SetAnchor(rc.normal, rc.point);
                player.anim.SetTrigger("corner");
                //print("corner!");
            }
        }

        player.anim.SetFloat("speed", rb.velocity.magnitude);
    }

    // Check for wall climbing
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (stunned)
        {
            return;
        }

        if(collision.collider.tag == "Untagged" && collision.gameObject.layer == 0 && !rolling)
        {
            //print("anchor!");
            SetAnchor(collision.contacts[0].normal, collision.contacts[0].point);
        }

        float diff = Mathf.Abs(Vector3.Angle(-collision.contacts[0].normal, player.GetForward()));

        if (rolling && (rollCharge > 0f || rb.velocity.magnitude > 1f) && (diff < 15f || collision.collider.tag == "Enemy") && collision.collider.GetComponent<Entity>())
        {
            print("PASS");

            collision.collider.GetComponent<Entity>().TakeDamage(1);
        }
    }

    //TODO: Corrections for controller climbing!
    public void Move(Vector2 move, bool roll, bool release)
    {
        Vector3 targetVelocity = Vector3.zero;

        //only control here if grounded
        if (anchored)
        {
            int trueVertical = (transform.up.x < 0.1f) ? 1 : -1;

            // If on ground/ceiling
            int vertical = (transform.up.y > -0.1f) ? 1 : -1;
            float moveC = move.x;   // Correction for continuous crawl
            // If on side...
            if(transform.up.x > 0.5f || transform.up.x < -0.5f)
            {
                vertical = (transform.up.x > -0.1f) ? 1 : -1;
                moveC = move.y * -vertical;
            }

            // Move the character by finding the target velocity using (correction)
            //targetVelocity = Quaternion.Euler(transform.rotation.eulerAngles) * new Vector2(moveC * speed * 100f, 0);

            // Override with real directions NOT after player changes inputs
            if (true || lastMove != move || correctCrawl)
            {
                targetVelocity = (move * speed * 100f) * transform.right * trueVertical;
                correctCrawl = true;
            }

            // Check if going off edge
            if (true)
            {
                Vector3 dir = player.GetForward() - transform.up * 1.5f;

                Ray r = new Ray(transform.position, dir.normalized);
                Debug.DrawRay(r.origin, r.direction, Color.red);

                if (!Physics2D.Raycast(r.origin, r.direction, 1f, LayerMask.GetMask("Default")))
                {
                    targetVelocity = Vector2.zero;
                }
            }


            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothing);

            if(rb.velocity.magnitude > 0.1f)
            {
                dust.Emit(1);
            }

            SetDirection(moveC);
        }

        // Air movement
        if(!anchored && airControl > 0f)
        {
            targetVelocity = new Vector2 ((move.x * speed * airControl * 10f), 0f);

            rb.velocity = rb.velocity + (Vector2)targetVelocity;
        }

        // Release
        if (release)
        {
            rolling = false;
            trail.emitting = false;

            player.anim.SetTrigger("release");
            //print("RELEASE");
        }

        // Roll
        if (anchored && roll)
        {
            rolling = true;
            rollCharge = 0.3f;

            player.anim.SetTrigger("roll");
            player.anim.ResetTrigger("release");
            GameManager.instance.PlaySingle("powerup");
            trail.emitting = true;

            UnAnchor();

            // Add a force to the player.
            //m_Grounded = false;
            rb.velocity = (targetVelocity + player.GetForward() / 5f).normalized * rollStrength;
        }

        lastMove = move;
    }

    // Sets controller to either stick to a wall or unstick
    void SetAnchor(Vector2 norm, Vector2 point)
    {
        Quaternion q = Quaternion.FromToRotation(Vector2.up, norm);

        rb.velocity = Vector3.zero;
        rb.gravityScale = 0f;

        if (!CheckVector(norm))
        {
            Quaternion.FromToRotation(Vector2.up, Vector2.up);
        }

        player.anim.SetTrigger("anchor");

        transform.rotation = q;
        //print(q + ", " + norm);

        float px = transform.position.x;

        if(!Physics2D.Raycast(transform.position, -transform.up, 0.6f, LayerMask.GetMask("Default")))
        {
            px = point.x;
            if(norm.y < 0.5f && norm.y > -0.5f && !anchored)
            {
                px += norm.x * 0.5f;
            }
        }

        if((Vector2)transform.position != point && transform.up.y > 0.1f)
        {
            transform.position = new Vector2(px, point.y + 0.5f);
        }

        correctCrawl = false;
        anchored = true;
    }

    public void Stun(bool a)
    {
        stunned = a;
        if (a)
        {
            UnAnchor();
        }
    }

    bool CheckVector(Vector2 c)
    {
        if (c.x < 0.8f && c.x > -0.8f)
        {
            if(c.y < 0.8f && c.y > -0.8f)
            {
                return false;
            }
        }

        return true;
    }

    void UnAnchor()
    {
        rb.gravityScale = gravity;

        anchored = false;
    }

    void SetDirection(float direction)
    {
        player.SetSpriteDirection(direction);
    }

}
