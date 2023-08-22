using UnityEngine;

public class StarProjectileScript : MonoBehaviour
{
    private Vector3 point1, point2, point3, point4;
    private float beforeSin;
    private float time;
    private Camera cam;
    private int offset;

    public float bulletSpeed;
    public GameObject starProjectileDeathParticle;
    public GameObject starProjectileSpawnParticle;

    float EaseIn(float t)
    {
        return t * t;
    }

    float EaseOut(float t)
    {
        return -Mathf.Pow(t - 1, 2) + 1;
    }

    float EaseInOut(float t)
    {
        return Mathf.Lerp(EaseIn(t), EaseOut(t), t);
    }

    void Awake()
    {
        if (Random.Range(0, 100) <= 50) offset = 7;
        else offset = -7;
        cam = GameManager.instance.mainCamera;
        transform.position = GameManager.instance.players.transform.position - transform.right * 2 + new Vector3(Random.Range(0.5f, -0.5f), Random.Range(0.5f, -0.5f), 0);
        Instantiate(starProjectileSpawnParticle, new Vector3(transform.position.x, transform.position.y, -1), transform.rotation);
        point1 = transform.position;
        point2 = GameManager.instance.players.transform.position - transform.up * offset + new Vector3(Random.Range(2, -2), Random.Range(2, -2), 0);
        point4 = cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(Random.Range(0.5f, -0.5f), Random.Range(0.5f, -0.5f), 0);
        point3 = Vector3.Lerp(GameManager.instance.players.transform.position, point4, 0.1f);
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (beforeSin <= 0.99f)
        {
            //Bézier Curve
            Vector3 ab = Vector3.Lerp(point1, point2, time);
            Vector3 bc = Vector3.Lerp(point2, point3, time);
            Vector3 cd = Vector3.Lerp(point3, point4, time);
            Vector3 abc = Vector3.Lerp(ab, bc, time);
            Vector3 bcd = Vector3.Lerp(bc, cd, time);
            transform.position = Vector3.Lerp(abc, bcd, time);
            time = EaseInOut(beforeSin);
            beforeSin += Time.deltaTime * bulletSpeed;

            /*Debug.DrawLine(point1, point2);
            Debug.DrawLine(point2, point3);
            Debug.DrawLine(point3, point4);
            Debug.DrawLine(ab, bc);
            Debug.DrawLine(bc, cd);
            Debug.DrawLine(abc, bcd);*/
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        Instantiate(starProjectileDeathParticle, transform.position, transform.rotation);
    }

}
