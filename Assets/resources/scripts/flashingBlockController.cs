using UnityEngine;
using System.Collections;

public class flashingBlockController : MonoBehaviour {

	// Use this for initialization
	void Start()
	{
		InvokeRepeating ("FlashColors", 0.3f, 0.3f);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "ballPrimary(Clone)")
		{
			Destroy (gameObject);

		}
		else if (collision.gameObject.name == "ballSecondary(Clone)")
		{
			Destroy (gameObject);
		}
	}

	void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
	void FlashColors(){
		if (GetComponent<SpriteRenderer> ().sprite == gameController.sprites [1] [gameController.primaryColor]) 
		{
			GetComponent<SpriteRenderer> ().sprite = gameController.sprites [1] [gameController.secondaryColor];
		}
		else if (GetComponent<SpriteRenderer> ().sprite == gameController.sprites [1] [gameController.secondaryColor]) 
		{
			GetComponent<SpriteRenderer> ().sprite = gameController.sprites [1] [gameController.primaryColor];
		}
	}
}
