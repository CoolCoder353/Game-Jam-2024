using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private readonly float moveSpeed = 5f;
    public float destRange = 5f;
    public Vector2 dest;
    private float timeOut = 3f;
    private bool atDest = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dest = new Vector2(
            rb.position.x + Random.Range(-destRange, destRange),
            rb.position.y + Random.Range(-destRange, destRange)
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (atDest) timeOut -= Time.deltaTime;
        if (timeOut <= 0)
        {
            atDest = false;
            timeOut = 3f;
            dest = new Vector2(
                rb.position.x + Random.Range(-destRange, destRange),
                rb.position.y + Random.Range(-destRange, destRange)
            );
        }
    }

    // FixedUpdate is called on a fixed-interval, indpenedent of frame rate
    void FixedUpdate()
    {
        Vector2 norm = (dest - rb.position).normalized;
        float ang = Mathf.Atan2(norm.y, norm.x);
        Vector2 newPos = rb.position + norm * moveSpeed * Time.fixedDeltaTime;
        // Quadrant 1 (x +ve, y +ve)
        if (ang >= 0 && ang <= Mathf.PI/2)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            rb.MovePosition(new Vector2(
                Mathf.Clamp(newPos.x, rb.position.x, dest.x),
                Mathf.Clamp(newPos.y, rb.position.y, dest.y)
            ));
        }
        // Quadrant 2 (x -ve, y +ve)
        else if (ang >= Mathf.PI/2 && ang <= Mathf.PI)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rb.MovePosition(new Vector2(
                Mathf.Clamp(newPos.x, dest.x, rb.position.x),
                Mathf.Clamp(newPos.y, rb.position.y, dest.y)
            ));
        }
        // Quadrant 3 (x -ve, y -ve)
        else if (ang >= -Mathf.PI && ang <= -Mathf.PI/2)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rb.MovePosition(new Vector2(
                Mathf.Clamp(newPos.x, dest.x, rb.position.x),
                Mathf.Clamp(newPos.y, dest.y, rb.position.y)
            ));
        }
        // Quadrant 4 (x +ve, y -ve)
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            rb.MovePosition(new Vector2(
                Mathf.Clamp(newPos.x, rb.position.x, dest.x),
                Mathf.Clamp(newPos.y, dest.y, rb.position.y)
            ));
        }

        // Check if at destination
        if (rb.position == dest) atDest = true;
    }
}
