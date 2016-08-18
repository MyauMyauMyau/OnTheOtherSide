using UnityEngine;
using System.Collections;
using Assets.scripts;

public class SwipeController : MonoBehaviour {

	// Use this for initialization
	float startTime;
	Vector2 startPos;
	bool couldBeSwipe;
	float comfortZone = 1000;
	float minSwipeDistance = 0f;
	float maxSwipeTime = 5.0f;
	
	private void MakeSwipe(Vector2 currentSwipe)
	{
		if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f && GameField.ClickedObject.Value.Y < Game.MAP_SIZE - 1)
		{
			if (GameField.Map[GameField.ClickedObject.Value.X, GameField.ClickedObject.Value.Y + 1] != null)
				GameField.Map[GameField.ClickedObject.Value.X, GameField.ClickedObject.Value.Y + 1].Click();
		}
		//swipe down
		if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f && GameField.ClickedObject.Value.Y > 0)
		{
			if (GameField.Map[GameField.ClickedObject.Value.X, GameField.ClickedObject.Value.Y - 1] != null)
				GameField.Map[GameField.ClickedObject.Value.X, GameField.ClickedObject.Value.Y - 1].Click();
		}
		//swipe left
		if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && GameField.ClickedObject.Value.X > 0)
		{
			if (GameField.Map[GameField.ClickedObject.Value.X - 1, GameField.ClickedObject.Value.Y] != null)
				GameField.Map[GameField.ClickedObject.Value.X - 1, GameField.ClickedObject.Value.Y].Click();
		}
		//swipe right
		if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && GameField.ClickedObject.Value.X < Game.MAP_SIZE - 1)
		{
			if (GameField.Map[GameField.ClickedObject.Value.X + 1, GameField.ClickedObject.Value.Y] != null)
				GameField.Map[GameField.ClickedObject.Value.X + 1, GameField.ClickedObject.Value.Y].Click();
		}
	}

	void Update() 
	{
		if (Input.touchCount == 0)
		{
			if (Input.GetMouseButtonDown(0))
			{
				//save began touch 2d point
				startPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			}
			if (Input.GetMouseButtonUp(0) && GameField.ClickedObject != null)
			{
				//save ended touch 2d point
				var secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

				//create vector from the two points
				var currentSwipe = new Vector2(secondPressPos.x - startPos.x, secondPressPos.y - startPos.y);

				//normalize the 2d vector
				currentSwipe.Normalize();
				MakeSwipe(currentSwipe);
			}
		}
		if (Input.touchCount > 0)
		{
			var touch = Input.touches[0];

			switch (touch.phase)
			{
				case TouchPhase.Began:
					couldBeSwipe = true;
					startPos = touch.position;
					startTime = Time.time;
					break;
				//case TouchPhase.Stationary:
					//couldBeSwipe = false;
					//break;
				case TouchPhase.Ended:
					if (couldBeSwipe && GameField.ClickedObject != null)
					{
						//save ended touch 2d point
						var secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

						//create vector from the two points
						var currentSwipe = new Vector2(secondPressPos.x - startPos.x, secondPressPos.y - startPos.y);

						//normalize the 2d vector
						currentSwipe.Normalize();
						MakeSwipe(currentSwipe);
					}
					break;
			}
		}
	}
}
