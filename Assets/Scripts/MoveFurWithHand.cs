using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFurWithHand : MonoBehaviour
{
    Material m_Material;
    Vector3 m_ColliderSize;

    public GameObject hand;
    Vector3 handColliderSize;
    Vector3 handColliderCenter;

    bool isColliding = false;
    float contactPointDistance_x;
    float contactPointDistance_z;
    float baseMoveMultiplier = 2.0f;

    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
        m_ColliderSize = GetComponent<BoxCollider>().size;
        handColliderSize = hand.GetComponent<BoxCollider>().size;
        handColliderCenter = hand.GetComponent<BoxCollider>().center;
    }


    void Update()
    {
        if(isColliding)
        {
            float currentDistance_x = transform.position.x - (hand.transform.position.x - handColliderCenter.x);
            float currentDistance_z = transform.position.z - (hand.transform.position.z - handColliderCenter.z);

            float MoveRatio_x = (-currentDistance_x / contactPointDistance_x + 1) * Mathf.Sign(contactPointDistance_x);
            float MoveRatio_z = (-currentDistance_z / contactPointDistance_z + 1) * Mathf.Sign(contactPointDistance_z);

            m_Material.SetVector("_BaseMove", new Vector4(MoveRatio_x * baseMoveMultiplier, 0, MoveRatio_z * baseMoveMultiplier, 3));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hand")
        {
            Debug.Log("enter");
            m_Material.SetVector("_WindFreq", new Vector4(0, 0, 0, 1));
            contactPointDistance_x = transform.position.x - (hand.transform.position.x - handColliderCenter.x);
            contactPointDistance_z = transform.position.z - (hand.transform.position.z - handColliderCenter.z);
            isColliding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand")
        {
            Debug.Log("exit");
            isColliding = false;
            m_Material.SetVector("_BaseMove", new Vector4(0, 0, 0, 3));
            m_Material.SetVector("_WindFreq", new Vector4(0.2f, 0.05f, 0.2f, 1));
        }
    }
}
