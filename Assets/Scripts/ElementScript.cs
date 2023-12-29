using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementScript : MonoBehaviour
{
    public string Name;
    public bool Discovered;
    public float CircleTransitionSpeed = 15f;
    public Vector3 CircleTargetSize = new(1.34f, 1.34f, 1.34f);

    private bool backgroundEnabled = false;
    private Transform circle;

    void Awake()
    {
        
    }

    void Start()
    {
        circle = transform.Find("SelectionCircle(Clone)");
    }

    void Update()
    {
        ScaleCircle();
    }

    private void ScaleCircle()
    {
        if (backgroundEnabled && circle.localScale != CircleTargetSize)
        {
            circle.localScale = Vector3.MoveTowards(circle.localScale, CircleTargetSize, Time.deltaTime * CircleTransitionSpeed);
        }
        else if (!backgroundEnabled && circle.localScale.x > 0)
        {
            circle.localScale = Vector3.MoveTowards(circle.localScale, Vector3.zero, Time.deltaTime * CircleTransitionSpeed);
        }
    }

    private void EnableBackground()
    {
        backgroundEnabled = true;
    }

    private void DisableBackground()
    {
        backgroundEnabled = false;
    }
}
