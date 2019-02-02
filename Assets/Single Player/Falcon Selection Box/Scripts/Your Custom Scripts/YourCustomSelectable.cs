using UnityEngine;
using System.Collections;
using Falcon;
/*
    Convinient script that allows to have your own Selectable class. Easy to modify.
*/
public class YourCustomSelectable : Selectable
{
    private Renderer myRenderer = null;
    private Vector3 rectStartPosition = Vector3.zero;
    private Rect screenCollisionRect = new Rect();
    public Collider myCollider = null;
    public Collider2D myCollider2D = null;

    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        if (myCollider == null)
        {
            myCollider = GetComponent<Collider>();
        }
        if (myCollider2D == null)
        {
            myCollider2D = GetComponent<Collider2D>();
        }
    }

    public override void Select()
    {
        base.Select();
        //Customized behavior of the select method.
        Debug.Log("Put your custom code here.");
    }

    public override void Unselect()
    {
        base.Unselect();
        //Customized behavior of the unselect method.
        Debug.Log("Put your custom code here.");
    }

    public override Bounds GetBounds()
    {
        Bounds b = new Bounds();
        if (myCollider != null)
        {
            b = myCollider.bounds;
        }
        else if (myCollider2D != null)
        {
            b = myCollider2D.bounds;
        }
        return b;
    }

    public override Rect GetBoundsScreenCoordinates()
    {
        rectStartPosition = Camera.main.WorldToScreenPoint(myRenderer.bounds.min);
        screenCollisionRect.x = rectStartPosition.x;
        screenCollisionRect.y = rectStartPosition.y;
        screenCollisionRect.width = Camera.main.WorldToScreenPoint(myRenderer.bounds.max).x - Camera.main.WorldToScreenPoint(myRenderer.bounds.min).x;
        screenCollisionRect.height = Camera.main.WorldToScreenPoint(myRenderer.bounds.max).y - Camera.main.WorldToScreenPoint(myRenderer.bounds.min).y;
        return screenCollisionRect;
    }
}
