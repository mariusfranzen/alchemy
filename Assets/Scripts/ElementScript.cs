using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementScript : MonoBehaviour
{
    public string Name;
    public bool Discovered;
    public Animator Animator;
    public GameMasterScript GameMaster;
    [HideInInspector]
    public ElementScript ElementTryingToCombine = null;

    private Transform circle;
    private new SpriteRenderer renderer;

    private bool isDragging = false;
    private bool isHovering = false;
    private bool hasWaited = false;

    void Awake()
    {
        renderer = transform.GetComponent<SpriteRenderer>();
        circle = transform.Find("SelectionCircle(Clone)");
        Animator = circle.GetComponent<Animator>();
        GameMaster = FindFirstObjectByType<GameMasterScript>();
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        renderer.sortingOrder += 1;
    }

    void OnMouseUp()
    {
        isDragging = false;
        renderer.sortingOrder -= 1;
        if (ElementTryingToCombine)
        {
            TryToCombine();
        }
    }

    public void Disappear()
    {
        Animator.SetTrigger("DisappearTrigger");
    }

    public void TryToCombine()
    {
        var combination = GameMaster.CombineElements(this, ElementTryingToCombine);
        if (combination)
        {
            Debug.Log(combination);
        }
        else
        {
            Debug.Log("no");
            ElementTryingToCombine.SetHover(false);
        }
    }

    /// <summary>
    /// When another element is hovering over this one
    /// </summary>
    public void SetHover(bool hover)
    {
        isHovering = hover;
        if (hover)
        {
            StartCoroutine(WaitForHoverEffect());
        } 
        else
        {
            Animator.SetBool("IsHovering", false);
            Animator.SetTrigger("HoverEnd");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDragging is false)
        {
            collision.transform.GetComponent<ElementScript>().ElementTryingToCombine = this;
            SetHover(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (isDragging is false)
        {
            collision.transform.GetComponent<ElementScript>().ElementTryingToCombine = null;
            SetHover(false);
        }
    }

    IEnumerator WaitForHoverEffect()
    {
        if (hasWaited is false)
        {
            hasWaited = true;
            yield return new WaitForSeconds(0.8f);
        }

        hasWaited = false;

        if (isHovering)
        {
            Animator.SetBool("IsHovering", true);
            Animator.SetTrigger("HoverStart");
        }
    }
}
