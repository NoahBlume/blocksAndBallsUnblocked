using UnityEngine;
using System.Collections;

public class ballController : MonoBehaviour {

    public static ballController Instance;
    public gameController GameController;
	public Rigidbody2D thisBall;
	public Vector3 ballSpawnLocation = new Vector3(0, 0, 11);
	public static int ballSpeedAndOrDirection = 0;
	public bool ballIsDead = false;
	public static bool killBall = false;
	public static bool swapBallColor = false;
	public bool launched = false;
	// Use this for initialization
	void Start () {
		thisBall = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		if (ballSpeedAndOrDirection == 1) 
		{
			BallGoesUp ();
		}
		else if (ballSpeedAndOrDirection == 2) 
		{
			BallGoesDown ();
		}
		if (killBall) 
		{
			Destroy (this.gameObject);
			killBall = false;
		}
		if (Application.loadedLevelName == "g") 
		{
			Destroy (this.gameObject);
		}
		if (swapBallColor && GetComponent<SpriteRenderer>().sprite == gameController.sprites [0] [gameController.primaryColor]) 
		{
			GetComponent<SpriteRenderer>().sprite = gameController.sprites [0] [gameController.secondaryColor];
			swapBallColor = false;
		}
		if (swapBallColor && GetComponent<SpriteRenderer>().sprite == gameController.sprites [0] [gameController.secondaryColor]) 
		{
			GetComponent<SpriteRenderer>().sprite = gameController.sprites [0] [gameController.primaryColor];
			swapBallColor = false;
		}
    }
    
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
		gameController.ballShouldSpawn = true;
		if (!ballIsDead) 
		{
			gameController.ballsLeft--;
		}
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
		Debug.Log ("HEY THE BALL COLLIDED WITH " + collision.gameObject.name);
		if (collision.gameObject.name == "blockPrimary(Clone)" && GetComponent<SpriteRenderer>().sprite == gameController.sprites [0] [gameController.primaryColor])
        {
			gameController.HitRightColor();
			ballIsDead = true;
			Destroy (this.gameObject);
			gameController.ballShouldSpawn = true;

        }
		else if (collision.gameObject.name == "blockSecondary(Clone)" && GetComponent<SpriteRenderer>().sprite == gameController.sprites [0] [gameController.primaryColor])
        {
			gameController.HitWrongColor();
			ballIsDead = true;
			Destroy (this.gameObject);
			gameController.ballShouldSpawn = true;
        }

		if (collision.gameObject.name == "blockPrimary(Clone)" && GetComponent<SpriteRenderer>().sprite == gameController.sprites [0] [gameController.secondaryColor])
		{
			gameController.HitWrongColor();
			ballIsDead = true;
			Destroy (this.gameObject);
			gameController.ballShouldSpawn = true;

		}
		else if (collision.gameObject.name == "blockSecondary(Clone)" && GetComponent<SpriteRenderer>().sprite == gameController.sprites [0] [gameController.secondaryColor])
		{
			gameController.HitRightColor();
			ballIsDead = true;
			Destroy (this.gameObject);
			gameController.ballShouldSpawn = true;
		}
		if (collision.gameObject.name == "blockFlashing(Clone)")
		{
			gameController.HitFlashing();
			ballIsDead = true;
			Destroy (this.gameObject);
			gameController.ballShouldSpawn = true;

		}
    }

	public void BallGoesUp()
	{
		if (!launched) 
		{
			thisBall.velocity = new Vector2 (0, 12);
			launched = true;
		}
		ballController.ballSpeedAndOrDirection = 0;
	}

	public void BallGoesDown()
	{
		if (!launched) 
		{
			thisBall.velocity = new Vector2 (0, -12);
			launched = true;
		}
		ballController.ballSpeedAndOrDirection = 0;
	}
		
}
