using UnityEngine;

public class Dove : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField] private float timeForRandomMovement = 2f;
    public int phase;
    [SerializeField] private float speedPush = 20f;
    private bool right = true;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        InvokeRepeating("RandomMovement", 0f, timeForRandomMovement);
    }

    private void RandomMovement()
    {
        if (phase != 0)
        {
            return;
        }

        var side = Random.Range(0, 100);
        var force = new Vector2(-speedPush, 200f);
        if (side < 50)
        {
            force = new Vector2(speedPush, 200f);
            if (!right)
            {
                Flip();
                right = true;
            }
        }
        else if (right)
        {
            Flip();
            right = false;
        }

        rigidBody.AddForce(force);
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
    }
}