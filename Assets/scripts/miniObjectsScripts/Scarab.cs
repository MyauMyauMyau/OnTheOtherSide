using System.Collections;
using System.Net;
using UnityEngine;

namespace Assets.scripts.miniObjectsScripts
{
	public class Scarab : MonoBehaviour
	{

		public bool IsMoving;
		// Use this for initialization
		void Start ()
		{
			EndPoint = GameField.GetVectorFromCoord(Target.X, Target.Y);
		}

		public Vector3 EndPoint { get; set; }

		// Update is called once per frame
		void Update () {

			if (transform.position.y > 4.5f)
			{
				transform.position = transform.position + new Vector3(0, -0.05f);
				return;
			}
			else
			{
				if (!IsMoving)
				{
					IsMoving = true;
					StartCoroutine(GoToTarget());
				}
			}
		}

		private IEnumerator GoToTarget()
		{
			Game.PlayerIsBlocked = true;
			var tg = (EndPoint.x - transform.position.x)/(-EndPoint.y + transform.position.y);
			var atg = Mathf.Atan(tg)*180/Mathf.PI;
			transform.Rotate(new Vector3(0,0,atg));

			var moveVector = (EndPoint - transform.position)/50;
			for (int i = 0; i < 50; i ++)
			{
				transform.localScale+=new Vector3(0.01f, 0.01f);
				transform.position += moveVector;
				yield return new WaitForSeconds(0.01f);
			}
			if (GameField.Map[Target.X, Target.Y].TypeOfMonster == MonsterType.Coocon)
			{
				GameField.Map[Target.X, Target.Y].Destroy();
				Dictionaries.MonsterCounter[MonsterType.Coocon]++;
			}
			Game.PlayerIsBlocked = false;
			Destroy(gameObject);
		}

		public Coordinate Target { get; set; }
	}
}
