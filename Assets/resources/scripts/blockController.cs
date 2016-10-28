using UnityEngine;
using System.Collections;

public class blockController : MonoBehaviour
{

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
}