using UnityEngine;
using System.Collections;

public class noMove : MonoBehaviour
{

	private float StartTime;
	// Use this for initialization
	void Start ()
	{
		StartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time < StartTime + 0.8f) return;
		transform.GetChild(0).position = transform.GetChild(0).position + new Vector3(-0.1f, -0.1f);
		transform.GetChild(1).position = transform.GetChild(1).position + new Vector3(-0.1f, 0.1f);
		transform.GetChild(2).position = transform.GetChild(2).position + new Vector3(0, 0.1f);
		transform.GetChild(3).position = transform.GetChild(3).position + new Vector3(0f, -0.1f);
		transform.GetChild(4).position = transform.GetChild(4).position + new Vector3(0.1f, 0.1f);
		transform.GetChild(5).position = transform.GetChild(5).position + new Vector3(0.1f, -0.1f);

		for (int i = 0; i < 6; i++)
		{
			if (i < 3)
				transform.GetChild(i).Rotate(0,0,10);
			else
				transform.GetChild(i).Rotate(0,0, -10);
		}
		if (Time.time > StartTime + 1.8f)
			Destroy(gameObject);
	}
}
