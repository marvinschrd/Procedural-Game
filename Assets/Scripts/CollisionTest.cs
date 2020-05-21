using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{

    Collider2D collider;
    List<ContactPoint2D> contacts;
    List<Vector2> inBetweenPoints;

    float timer = 3f;
    bool getContacts = true;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        contacts = new List<ContactPoint2D>();
        inBetweenPoints = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer<=0&&getContacts)
        {
            collider.GetContacts(contacts);
            Vector2 test = (new Vector2(-1.45f, 0.7f) + new Vector2(-1.7f, 0.7f))/2;
            Vector2 test2 = (new Vector2(-0.9f, 0.7f).normalized + new Vector2(-0.7f, 0.7f).normalized).normalized;
            Debug.Log("TEST =" + test);
            Debug.Log("TEST2 =" + test2);
            Debug.Log(contacts.Count);
            //Debug.Log(contacts[0].collider.transform.name);
            //Debug.Log(contacts[1].collider.transform.name);
            Debug.Log(contacts[2].collider.transform.name + "x =" + contacts[2].point.x + "y=" + contacts[2].point.y);
            Debug.Log(contacts[3].collider.transform.name + "x =" + contacts[3].point.x + "y=" + contacts[3].point.y);
            //Debug.Log(contacts[3].collider.transform.name);

            while (contacts.Count > 0)
            {
                Debug.Log(contacts[0].collider.transform.name);
                Vector2 firstPointPosition = new Vector2(contacts[0].point.x, contacts[0].point.y);
                Debug.Log("first point position = " + firstPointPosition);
                //Debug.Log(contacts[1].point.x + contacts[1].point.y);
                Vector2 secondPointPosition = new Vector2(contacts[1].point.x, contacts[1].point.y);
                Debug.Log("second point position =" + secondPointPosition);
               // Vector2 inBetweenPointPosition = (firstPointPosition.normalized+ secondPointPosition.normalized).normalized;
                Vector2 inBetweenPointPosition = (firstPointPosition + secondPointPosition)/2;
                inBetweenPoints.Add(inBetweenPointPosition);
                contacts.RemoveAt(0);
                contacts.RemoveAt(0);
                //Debug.Log("POINT 0" +contacts[0].point.x + contacts[0].point.y);
                //Debug.Log("POINT 1"+contacts[1].point.x + contacts[1].point.y);
                //Debug.Log("inbetween point =" +inBetweenPoints[0]);
                //maintenant, utiliser la liste de points entre deux pour dessiner des gizmos;
            }
            getContacts = false;
            DrawCircles();
            WallOpening();
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
       
    //      collider.GetContacts(contacts);

    //        Debug.Log(contacts.Count);
    //    //Debug.Log(contacts[0].collider.transform.name);
    //    //Debug.Log(contacts[1].collider.transform.name);
    //    //Debug.Log(contacts[2].collider.transform.name);
    //    //Debug.Log(contacts[3].collider.transform.name);

    //    while (contacts.Count > 0)
    //    {
    //        Debug.Log(contacts[0].collider.transform.name);
    //        Vector2 firstPointPosition = new Vector2(contacts[0].point.x, contacts[0].point.y);
    //        Debug.Log("first point position = " +firstPointPosition);
    //        Vector2 secondPointPosition = new Vector2(contacts[1].point.x, contacts[1].point.y);
    //        Debug.Log("second point position =" +secondPointPosition);
    //        Vector2 inBetweenPointPosition = (firstPointPosition.normalized + secondPointPosition.normalized).normalized;
    //        inBetweenPoints.Add(inBetweenPointPosition);
    //        contacts.RemoveAt(0);
    //        contacts.RemoveAt(0);
    //        //Debug.Log("inbetween point =" +inBetweenPoints[0]);
    //        //maintenant, utiliser la liste de points entre deux pour dessiner des gizmos;
    //    }

    //    //for (int i = 0; i < inBetweenPoints.Count; i++)
    //    //{
    //    //    Debug.Log(inBetweenPoints[i]);
    //    //}
    //     DrawCircles();

    //    //for(int i =0; i<contacts.Count;i++)
    //    //{
    //    //    Debug.Log(contacts[0].collider.transform.name);

    //    //    Vector2 pointPosition = new Vector2(contacts[0].point.)
    //    //}
    //}

    void DrawCircles()
    { 
       
        for (int i = 0; i < inBetweenPoints.Count; i++)
        {
            //Gizmos.DrawWireSphere(inBetweenPoints[i], 0.5f);
            //Gizmos.color = Color.black;
            Debug.Log("inbetween point" + inBetweenPoints[i]);
        }
    }

    void WallOpening()
    {
        for(int i = 0; i <inBetweenPoints.Count; i++)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(inBetweenPoints[i],0.1f, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != gameObject.GetComponent<BoxCollider2D>())
                {
                    Destroy(hit.collider.transform.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (inBetweenPoints != null)
        {
            for (int i = 0; i < inBetweenPoints.Count; i++)
            {
                Gizmos.DrawWireSphere(inBetweenPoints[i], 0.5f);
                Gizmos.color = Color.black;
            }
        }
    }
}
