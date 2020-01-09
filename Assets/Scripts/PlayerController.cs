using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 20.0f;
    public int health = 100;
    private Rigidbody2D rigidBody;
    private bool right = true;
    [SerializeField] private Animator animator;
    public float horizontal;
    public float vertical;
    [SerializeField] private Text healthText;
    public EnumBreadType breadType = EnumBreadType.NORMAL;
    private bool infected = false;

    public GameObject gameOverPanel;

    public int resistCharges;

    [SerializeField] private GameObject helmetObjectArmor;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var joystickObject = GameObject.Find("Joystick");
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (joystickObject != null)
        {
            var joystick = joystickObject.GetComponent<Joystick>();
            if (joystick.m_UseX)
            {
                horizontal = joystick.horizontal;
            }

            if (joystick.m_UseY)
            {
                vertical = joystick.vertical;
            }
        }

        if (GameObject.Find("Events").GetComponent<Events>().weatherType == Events.EnumWeatherType.FOGGY)
        {
            horizontal *= -1;
            vertical *= -1;
        }

        helmetObjectArmor.SetActive(resistCharges > 0);

        if (vertical != 0f || horizontal != 0f)
        {
            Move();
            animator.SetBool("walking", true);
            transform.Find("Particle System").gameObject.SetActive(true);
        }
        else if (animator.GetBool("walking"))
        {
            animator.SetBool("walking", false);
            transform.Find("Particle System").gameObject.SetActive(false);
        }
    }

    private void Move()
    {
        var force = rigidBody.transform.position;

        force.x += horizontal * Time.deltaTime * speed;
        force.y += vertical * Time.deltaTime * speed;
        rigidBody.MovePosition(force);
        if (horizontal > 0f && !right)
        {
            Flip();
            right = true;
        }
        else if (horizontal < 0f && right)
        {
            Flip();
            right = false;
        }
    }

    private void Flip()
    {
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Damage(int damage, bool resistable)
    {
        if (damage > 0)
        {
            if (resistable && resistCharges > 0)
            {
                resistCharges--;
            }
            else
            {
                health -= damage;
            }

            if (health <= 50)
            {
                if (breadType == EnumBreadType.INFECTED)
                {
                    breadType = EnumBreadType.EATED_INFECTED;
                }
                else
                {
                    breadType = EnumBreadType.EATED;
                }
            }

            if (health <= 0f)
            {
                gameOverPanel.SetActive(true);
                Destroy(gameObject);
                Destroy(GameObject.Find("HealthPanel"));
                Destroy(GameObject.Find("TimePanel"));
            }
        }

        healthText.text = "Health: " + health + "%";
        Camera.main.GetComponent<CameraShake>().shakeDuration = 0.4f;
    }

    public void Infect()
    {
        if (breadType == EnumBreadType.INFECTED && breadType == EnumBreadType.EATED_INFECTED) return;
        breadType = breadType == EnumBreadType.EATED ? EnumBreadType.EATED_INFECTED : EnumBreadType.INFECTED;

        speed = 7f;
        Invoke("Pure", 16f);
    }

    private void Pure()
    {
        if (breadType == EnumBreadType.EATED_INFECTED)
        {
            breadType = EnumBreadType.EATED;
        }

        breadType = EnumBreadType.NORMAL;
        speed = 10f;
    }

    public enum EnumBreadType
    {
        NORMAL,
        EATED,
        INFECTED,
        EATED_INFECTED,
        DRYED,
        DRYED_EATED
    }
}