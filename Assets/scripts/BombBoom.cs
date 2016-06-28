using UnityEngine;
using System.Collections;
using Assets.scripts;
public class BombBoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(Boom());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator Boom()
	{
		Game.PlayerIsBlocked = true;
		
		var scale = gameObject.transform.localScale;
		for (int i = 0; i < 20; i++)
		{
			scale.x += 0.03f;
			scale.y += 0.03f;
			transform.localScale = scale;
			yield return new WaitForSeconds(0.005f);
		}
		Game.PlayerIsBlocked = false;
		DestroyObject(gameObject);		
	}
}
