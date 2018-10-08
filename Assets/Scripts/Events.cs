using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Events : MonoBehaviour
{
    public EnumWeatherType weatherType = EnumWeatherType.CLOUDY;

    public GameObject rainObject;
    public GameObject spawnZone;

    [SerializeField] private GameObject pitPrefab;
    [SerializeField] private GameObject poolPrefab;

    public GameObject attackZone;
    public GameObject secondAttackZone;
    public GameObject secondAttackZoneDamage;

    public float timeBetweenFly = 20f;
    public Animator doveAnimator;

    public float pitChance = 10f;

    public GameObject fogObject;

    public GameObject mealPrefab;
    public GameObject helmetPrefab;

    private bool attacking;

    private void Start()
    {
        InvokeRepeating("RandomEvent", 15f, 30f);
        InvokeRepeating("RandomObject", 0f, 10f);
        InvokeRepeating("RandomAttack", 2f, 4f);
        /*	Invoke ("Fly", timeBetweenFly);
            Invoke ("SecondAttack", 12f);*/
    }

    private void RandomAttack()
    {
        if (attacking) return;
        var chance = Random.Range(0, 100);

        if (chance <= 45)
        {
            SecondAttack();
        }

        Invoke("Fly", timeBetweenFly);
    }

    private void Fly()
    {
        GameObject doveObject = GameObject.Find("DoveBoss");
        GameObject playerObject = GameObject.Find("Player");
        doveObject.GetComponent<Dove>().phase = 1;
        var x = playerObject.transform.position.x;

        Vector3 newPos = playerObject.transform.position;
        newPos.Set(x, doveObject.transform.position.y, doveObject.transform.position.z);
        doveObject.transform.position = newPos;
        doveAnimator.SetBool("attacking", true);
        /*defaultDove.SetActive (false);
        flyingDove.SetActive (true);*/

        newPos.Set(x, attackZone.transform.position.y, attackZone.transform.position.z);
        attackZone.transform.position = newPos;
        attacking = true;
        Invoke("Attack", 1.5f);
    }

    private void Attack()
    {
        /*flyingDove.SetActive (false);
        attackingDove.SetActive (true);*/
        /*Vector3 position = RandomPointInBox (spawnZone.transform.position, spawnZone.GetComponent<Collider2D> ().transform.localScale);
        position.z = 100;*/

        attackZone.SetActive(true);
        Invoke("Land", 0.7f);
    }

    private void Land()
    {
        /*attackZone.SetActive (false);
        attackingDove.SetActive (false);
        defaultDove.SetActive (true);*/
        attackZone.SetActive(false);
        doveAnimator.SetBool("attacking", false);

        GameObject.Find("DoveBoss").GetComponent<Dove>().phase = 0;

        if (timeBetweenFly > 1.5f)
        {
            timeBetweenFly -= 0.3f;
        }

        attacking = false;
        //Invoke ("Fly", timeBetweenFly);
    }

    private void SecondAttack()
    {
        var doveObject = GameObject.Find("DoveBoss");
        doveObject.GetComponent<Dove>().phase = 1;
        var newPos = Vector3.zero;
        newPos.z = 101f;
        doveObject.transform.position = newPos;

        var num = Random.Range(0, 3);
        doveAnimator.SetBool("secondAttack", true);
        Vector3 pos = secondAttackZone.transform.position;
        pos.y -= num * 3;
        secondAttackZone.transform.position = pos;
        /*pos = doveBomb.transform.position;
        pos.y -= num * 3;
        doveBomb.transform.position = pos;*/

        secondAttackZone.SetActive(true);
        attacking = true;
        Invoke("SpawnSecondAttackZone", 0.7f);
    }

    private void SpawnSecondAttackZone()
    {
        secondAttackZoneDamage.SetActive(true);
        Invoke("DespawnSecondAttackZone", 0.1f);
    }

    void DespawnSecondAttackZone()
    {
        secondAttackZoneDamage.SetActive(false);
        secondAttackZone.SetActive(false);

        Vector3 pos = secondAttackZone.transform.position;
        pos.y = -7f;
        secondAttackZone.transform.position = pos;

        doveAnimator.SetBool("secondAttack", false);

        GameObject doveObject = GameObject.Find("DoveBoss");
        doveObject.GetComponent<Dove>().phase = 0;

        Invoke("SecondAttack", 10f);
        attacking = false;
    }

    void RandomEvent()
    {
        rainObject.SetActive(false);
        fogObject.SetActive(false);

        var chance = Random.Range(0, 100);
        if (chance < 35)
        {
            weatherType = EnumWeatherType.RAIN;
            rainObject.SetActive(true);
        }

        if (chance >= 35 && chance < 60)
        {
            weatherType = EnumWeatherType.CLOUDY;
        }

        if (chance >= 60)
        {
            weatherType = EnumWeatherType.FOGGY;
            fogObject.SetActive(true);
        }

        Debug.Log("current weather: " + weatherType);

        chance = Random.Range(0, 100);

        var position = RandomPointInBox(spawnZone.transform.position,
            spawnZone.GetComponent<Collider2D>().transform.localScale);
        if (chance <= 60)
        {
            GameObject mealObject = Instantiate(mealPrefab);
            mealObject.transform.position = position;
        }
        else
        {
            GameObject mealObject = Instantiate(helmetPrefab);
            mealObject.transform.position = position;
        }
    }

    private void RandomObject()
    {
        Vector3 position;
        switch (weatherType)
        {
            case EnumWeatherType.CLOUDY:
            case EnumWeatherType.FOGGY:
                var chance = Random.Range(0, 100);
                if (chance <= pitChance)
                {
                    position = RandomPointInBox(spawnZone.transform.position,
                        spawnZone.GetComponent<Collider2D>().transform.localScale);

                    GameObject spawnedPit = Instantiate(pitPrefab);
                    if (GameObject.Find("Player").GetComponent<Collider2D>()
                        .IsTouching(spawnedPit.GetComponent<Collider2D>()))
                    {
                        Destroy(spawnedPit);
                    }

                    position.z = 100;
                    spawnedPit.transform.position = position;
                    pitChance = 10f;
                    Camera.main.GetComponent<CameraShake>().shakeDuration = 2f;
                }
                else
                {
                    pitChance += 10f;
                }

                break;
            case EnumWeatherType.RAIN:
                position = RandomPointInBox(spawnZone.transform.position,
                    spawnZone.GetComponent<Collider2D>().transform.localScale);
                GameObject spawnedPool = Instantiate(poolPrefab);
                position.z = 100;
                spawnedPool.transform.position = position;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static Vector3 RandomPointInBox(Vector3 center, Vector3 size)
    {
        return center + new Vector3(
                   (Random.value - 0.5f) * size.x,
                   (Random.value - 0.5f) * size.y,
                   (Random.value - 0.5f) * size.z
               );
    }

    public enum EnumWeatherType
    {
        RAIN,
        CLOUDY,
        FOGGY
    }
}