using UnityEngine;

public class BezierCurveScriptTest : MonoBehaviour
{
    public Transform point1, point2, point3, point4;
    [Range(0, 1)]
    public float time;

    private void Update()
    {
        Vector3 ab = Vector3.Lerp(point1.position, point2.position, (Time.time / 3) % 1);
        Vector3 bc = Vector3.Lerp(point2.position, point3.position, (Time.time / 3) % 1);
        Vector3 cd = Vector3.Lerp(point3.position, point4.position, (Time.time / 3) % 1);
        Vector3 abc = Vector3.Lerp(ab, bc, (Time.time / 3) % 1);
        Vector3 bcd = Vector3.Lerp(bc, cd, (Time.time / 3) % 1);
        Vector3 endPos = Vector3.Lerp(abc, bcd, (Time.time / 3) % 1);

        Debug.DrawLine(point1.position, point2.position);
        Debug.DrawLine(point2.position, point3.position);
        Debug.DrawLine(point3.position, point4.position);
        Debug.DrawLine(ab, bc, Color.green);
        Debug.DrawLine(bc, cd, Color.green);
        Debug.DrawLine(abc, bcd, Color.red);


        transform.position = endPos;
    }
}
