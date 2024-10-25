using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private Water water;

    private void Start()
    {
        water = GameObject.Find("water").GetComponent<Water>();
    }

    private void Update()
    {
        Vector3[] nearestVerts = FindNearestVerts();
        Vector3 normal = Vector3.Cross(nearestVerts[1] - nearestVerts[0], nearestVerts[3] - nearestVerts[0]);
        Plane plane = new Plane(normal, nearestVerts[0]);
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 100, transform.position.z), -transform.up);
        float enter = 0f;
        if (plane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = hitPoint;
        }
    }

    Vector3[] FindNearestVerts()
    {
        Vector3[] vts = water.GetVerts();
        Vector3[] nearestVerts = new Vector3[4];
        Vector2 correctedPosition = new Vector2(transform.position.x - water.gameObject.transform.position.x, transform.position.z - water.gameObject.transform.position.z) * 1/water.GetSpacing();
        int flooredX = Mathf.FloorToInt(correctedPosition.x);
        int flooredY = Mathf.FloorToInt(correctedPosition.y);

        nearestVerts[0] = vts[flooredX + flooredY * (water.GetLength() + 1)] + water.transform.position;
        nearestVerts[1] = vts[flooredX + (flooredY + 1) * (water.GetLength() + 1)] + water.transform.position;
        nearestVerts[2] = vts[flooredX + 1 + (flooredY + 1) * (water.GetLength() + 1)] + water.transform.position;
        nearestVerts[3] = vts[flooredX + 1 + flooredY * (water.GetLength() + 1)] + water.transform.position;

        return nearestVerts;
    }

    /*private void OnDrawGizmos()
    {
        try
        {
            foreach (Vector3 v in FindNearestVerts())
            {
                Gizmos.DrawSphere(v, 0.5f);
            }
        }
        catch
        {

        }
    }*/
}
