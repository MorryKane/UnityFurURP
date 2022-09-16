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

    public float baseMoveMultiplier = 2.0f;

    public float handStandardHeight = 0.15f;

    float maxDisplacementMultiplier_x;
    float maxDisplacementMultiplier_z;

    bool isRecovering = false;
    Vector4 recoverPos;
    int recoverCount = 0;
    public float recoverDecayFactor = 1;
    public float recoverAngularFrequency = 10;

    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
        m_ColliderSize = GetComponent<BoxCollider>().size;
        handColliderSize = hand.GetComponent<BoxCollider>().size;
        handColliderCenter = hand.GetComponent<BoxCollider>().center;
    }


    void LateUpdate()
    {
        if(isColliding)
        {
            float currentDistance_x = transform.position.x - (hand.transform.position.x - handColliderCenter.x);
            float currentDistance_z = transform.position.z - (hand.transform.position.z - handColliderCenter.z);

            float handHeight = (hand.transform.position.y - handColliderCenter.y) - transform.position.y;
            float handHeightMultiplier = handStandardHeight / handHeight;

            float MoveRatio_x = (-currentDistance_x / contactPointDistance_x + 1) * Mathf.Sign(contactPointDistance_x);
            float MoveRatio_z = (-currentDistance_z / contactPointDistance_z + 1) * Mathf.Sign(contactPointDistance_z);

            Vector4 baseMoveOutput = new Vector4(MoveRatio_x * baseMoveMultiplier * handHeightMultiplier * maxDisplacementMultiplier_x, 0, MoveRatio_z * baseMoveMultiplier * handHeightMultiplier * maxDisplacementMultiplier_z, 3);

            if (!isRecovering)
            {
                m_Material.SetVector("_BaseMove", baseMoveOutput);
            }
            else
            {
                Vector4 currentRecoverPos = m_Material.GetVector("_BaseMove");
                if((currentRecoverPos.x - baseMoveOutput.x) * Mathf.Sign(contactPointDistance_x) <= 0)
                {
                    currentRecoverPos.x = baseMoveOutput.x;
                }
                if((currentRecoverPos.z - baseMoveOutput.z) * Mathf.Sign(contactPointDistance_z) <= 0)
                {
                    currentRecoverPos.z = baseMoveOutput.z;
                }

                m_Material.SetVector("_BaseMove", currentRecoverPos);

                if(currentRecoverPos == baseMoveOutput)
                {
                    recoverCount = 0;
                    isRecovering = false;
                }
            }    
        }
    }

    void Update()
    {
        if (isRecovering)
        {
            recoverCount++;
            Vector4 currentRecoverPos = new Vector4(0, 0, 0, 3);
            currentRecoverPos.x = recoverPos.x * Mathf.Exp(-recoverCount * recoverDecayFactor * Time.fixedDeltaTime) * Mathf.Cos(recoverAngularFrequency * recoverCount * Time.fixedDeltaTime);
            currentRecoverPos.z = recoverPos.z * Mathf.Exp(-recoverCount * recoverDecayFactor * Time.fixedDeltaTime) * Mathf.Cos(recoverAngularFrequency * recoverCount * Time.fixedDeltaTime);
            m_Material.SetVector("_BaseMove", currentRecoverPos);

            if (Mathf.Abs(recoverPos.x * Mathf.Exp(-recoverCount * recoverDecayFactor * Time.fixedDeltaTime)) < 0.01 && 
                Mathf.Abs(recoverPos.z * Mathf.Exp(-recoverCount * recoverDecayFactor * Time.fixedDeltaTime)) < 0.01 &&
                !isColliding)
            {
                recoverCount = 0;
                isRecovering = false;
                m_Material.SetVector("_BaseMove", new Vector4(0, 0, 0, 3));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hand")
        {
            Debug.Log("enter");
            contactPointDistance_x = transform.position.x - (hand.transform.position.x - handColliderCenter.x);
            contactPointDistance_z = transform.position.z - (hand.transform.position.z - handColliderCenter.z);

            maxDisplacementMultiplier_x = Mathf.Abs(contactPointDistance_x) / (m_ColliderSize.x * transform.localScale.x / 2.0f + handColliderSize.x * hand.transform.localScale.x / 2.0f);
            maxDisplacementMultiplier_z = Mathf.Abs(contactPointDistance_z) / (m_ColliderSize.z * transform.localScale.z / 2.0f + handColliderSize.z * hand.transform.localScale.z / 2.0f);

            isColliding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand")
        {
            Debug.Log("exit");
            isColliding = false;
            recoverPos = m_Material.GetVector("_BaseMove");
            recoverCount = 0;
            isRecovering = true;
        }
    }
}
