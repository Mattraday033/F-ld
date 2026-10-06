using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraglessScrollRect : ScrollRect
{

    //the event system makes the scroll rect the drag target of any press on its children, and once the pointer moves past the
    //drag threshold it cancels the click of whatever was pressed. With no drag target the press stays a click
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.pointerDrag = null;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    { 
    
    }

    public override void OnDrag(PointerEventData eventData)
    { 

    }

    public override void OnEndDrag(PointerEventData eventData)
    { 
        
    }

}
