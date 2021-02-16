using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Solution : MonoBehaviour
{
    Vector3 target;
    float speed;
    //public Rigidbody rb;
    public Vector3 destination;
    public float duration;

    //void Update()
    //{
    //    float distance = Vector3.Distance(target, transform.position);
    //    if (distance > 1)
    //    {
    //        Vector3 direction = target - transform.position;
    //        transform.position += direction * speed * Time.deltaTime;
    //    }
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        DrawRay();
    //}

   private void DrawRay()
    {
        //Ray ray = new Ray(this.transform.position, transform.forward);
        //if(Physics.Raycast(ray, out RaycastHit raycastHit))
        //{
        //    Resources.Load();
        //}
        //Debug.DrawRay(this.transform.position, transform.forward * 20, Color.red);

    }


    //private void FixedUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        rb.AddForce(new Vector3(0, 30, 0) * 5);
    //}

    private void Start()
    {
        //StartCoroutine(Movement(this.gameObject, destination, duration));
        Debug.Log(Math.Round(6.5f));
        Debug.Log(Math.Round(11.5f));
    }

    private IEnumerator Movement(GameObject gameObject, Vector3 destination, float duration)
    {
        Vector3 startPos = gameObject.transform.position;
        float elapsedTime = 0;

        while(elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, destination, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            Debug.Log("elapsedTime: " + elapsedTime);
            yield return null;
        }

    }
}
