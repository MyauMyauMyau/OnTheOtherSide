using UnityEngine;
using System.Collections;

public class BigPlateController : MonoBehaviour {

	public bool IsActivated { get; set; }
	public float TimeToRemoveBlock;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (IsActivated)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Deactivate();
		}
		if (TimeToRemoveBlock > 0.1f && TimeToRemoveBlock < Time.time)
			MainMenu.Instance.IsPaused = false;
	}

	public void Activate()
	{
		if (IsActivated)
			return;
		IsActivated = true;
		GetComponent<Animation>().Play("activatingGrave");
		MainMenu.Instance.IsPaused = true;
		TimeToRemoveBlock = 0;
	}

	public void Deactivate()
	{
		IsActivated = false;
		GetComponent<Animation>().Play("deactivatingGrave");
		TimeToRemoveBlock = Time.time + 0.5f;
	}
}
