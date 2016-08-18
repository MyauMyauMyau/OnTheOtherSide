using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlyingText : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.position+= new Vector3(0, Time.deltaTime*30);
		var color = GetComponent<Text>().color -= new Color(0,0,0,Time.deltaTime*0.25f);
		if (color.a < 0)
			Destroy(gameObject);
	}
}
