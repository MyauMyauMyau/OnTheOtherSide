using UnityEngine;
using Random = System.Random;

namespace Assets.scripts.miniObjectsScripts
{
	public class brokenWeb : MonoBehaviour
	{

		public Vector3 TargetPosition;
		public Vector3 StartPosition;
		private float startTime;
		private float angle;
		// Use this for initialization
		void Start ()
		{
			startTime = Time.time;
			angle = (float)Monster.Rnd.NextDouble()*2 - 1;
			angle *= 60;
			StartPosition = transform.position;
			TargetPosition = StartPosition - new Vector3((float)Monster.Rnd.NextDouble()*2 - 1, 10);

		}
	
		// Update is called once per frame
		void Update () {
			
			var curPos = Vector3.Lerp(
				StartPosition, TargetPosition,
				(Time.time - startTime) / 2);
			curPos.y += 4*Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / 2) * Mathf.PI);
			transform.position = curPos;
			transform.Rotate(new Vector3(0,0, angle) * Time.deltaTime);
			if (transform.position.y < - 10f)
				Destroy(gameObject);
		}
	}
}
