using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 GrapplePoint;
    public LayerMask WhatIsGrappable;
    public Transform gunTip, Camera, Player;
    private float maxDistance = 100f;
    private SpringJoint joint;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {   

        if(Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if(Input.GetMouseButtonUp(0)) 
        {
            StopGrapple();
        }

    }


    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(origin: Camera.position, direction: Camera.forward, out hit, maxDistance))
        {
            GrapplePoint = hit.point;
            joint = Player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplePoint;

            float distanceFromPoint = Vector3.Distance(a: Player.position, b: GrapplePoint);
            

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;


            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

    private Vector3 currentGrapplePosition;

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope()
    {
        if (!joint) return;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, GrapplePoint, Time.deltaTime * 8f);
        lr.SetPosition(index: 0, gunTip.position);
        lr.SetPosition(index: 1, currentGrapplePosition);

    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return GrapplePoint;
    }

}
