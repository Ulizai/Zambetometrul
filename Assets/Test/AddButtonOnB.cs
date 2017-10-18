using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButtonOnB : MonoBehaviour {
    [SerializeField] protected GameObject testButton;

    protected int buttonCount;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.B))
        {
            ++buttonCount;
            GameObject newButton = GameObject.Instantiate(testButton);
            newButton.transform.SetParent(transform);
            RectTransform rt = newButton.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(buttonCount*180, -70, 0);
        }
	}
}
