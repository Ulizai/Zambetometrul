using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustComponents : MonoBehaviour {

    [SerializeField] Vector2 bufferSpace = new Vector2(20,20);
    protected List<RectTransform> collectedObjects = new List<RectTransform>();
    protected List<RectTransform> elementsInRow = new List<RectTransform>();
    protected RectTransform thisRT;

    protected float height;
    protected float heightSoFar;
    protected float widthLeft;

	// Use this for initialization
	void Start () {
        RectTransform rt = GetComponent<RectTransform>();
        height = rt.sizeDelta.y;
        thisRT = GetComponent<RectTransform>();
        widthLeft = Screen.width;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A))
        {
            AdjustContainerAndChildren();
        }
	}

    public void AdjustContainerAndChildren()
    {
        ///We assume that everything in the collectedObjects is adjusted
        for (int index = collectedObjects.Count; index<transform.childCount; ++index) /// Repeat for all new elements
        {
            RectTransform childRT = transform.GetChild(index).GetComponent<RectTransform>();

            ///Anchor top left and select pivot as upper left
            childRT.anchorMin = new Vector2(0, 1);
            childRT.anchorMax = new Vector2(0, 1);
            childRT.pivot = new Vector2(0,1);

            if (widthLeft > 2 * bufferSpace.x + childRT.sizeDelta.x) ///Can i place another element?
            {
                RectTransform previousRT;
                previousRT = (index > 0 ? collectedObjects[index - 1].GetComponent<RectTransform>() : childRT);
                
                if (widthLeft == Screen.width) ///First element on row
                {
                    if (previousRT = childRT)///If first element make sure we align to top
                    {
                        childRT.localPosition = new Vector3(childRT.sizeDelta.x / 2 + bufferSpace.x, - bufferSpace.y, 0);
                    }
                    else
                    {
                        childRT.localPosition = new Vector3(childRT.sizeDelta.x / 2 + bufferSpace.x, previousRT.localPosition.y, 0);
                    }
                }
                else
                {
                    childRT.localPosition = new Vector3(previousRT.localPosition.x + previousRT.sizeDelta.x/2 + bufferSpace.x + childRT.sizeDelta.x/2 ,previousRT.localPosition.y,0); 
                }
                widthLeft -= bufferSpace.x + childRT.sizeDelta.x;

                collectedObjects.Add(childRT);
                elementsInRow.Add(childRT);
            }else
            {
                AlignElementsOnLine();
                elementsInRow.Clear();
                widthLeft = Screen.width;
                RectTransform previousTransform = transform.GetChild(index - 1).GetComponent<RectTransform>();
                childRT.localPosition = new Vector3(bufferSpace.x + childRT.sizeDelta.x/2,previousTransform.localPosition.y - previousTransform.sizeDelta.y/2 - bufferSpace.y - childRT.sizeDelta.y/2, 0);
                collectedObjects.Add(childRT);
                elementsInRow.Add(childRT);
                widthLeft -= childRT.localPosition.x + childRT.sizeDelta.x / 2;
            }          
        }
        AlignElementsOnLine();
        RectTransform lastRT = collectedObjects[collectedObjects.Count - 1].GetComponent<RectTransform>();
        thisRT.sizeDelta = new Vector2(0, -(lastRT.localPosition.y - lastRT.sizeDelta.y - bufferSpace.y ));  
    }

    private void AlignElementsOnLine()
    {
        float totalSize = 0;
        float addedSize = 0;
        foreach(RectTransform rect in elementsInRow)
        {
            totalSize += rect.sizeDelta.x;
        }
        float buffer = (Screen.width - totalSize) / (elementsInRow.Count+1);
        for (int index=0; index<elementsInRow.Count;++index)
        {
            elementsInRow[index].localPosition = new Vector3(addedSize + buffer,elementsInRow[index].localPosition.y,0);
            addedSize += buffer + elementsInRow[index].sizeDelta.x;
        }
    }
}
