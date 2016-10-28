// Developers: Dr4g0nbyt3
// Date: December 1st, 2015
// Github: dr4g0nbyt3
// Website: dr4g0nbyt3.github.io
// Email: dr4g0nbyt3@gmail.com
// Twitter: @dr4g0nbyt3
// Youtube: dr4g0nbyt3

using UnityEngine;
using System;
using System.Collections;

public class flashingLightsController : MonoBehaviour {


    

	// Use this for initialization
	void Start ()
    {
        Invoke("Delete", 1.99f);
	}

    void Delete()
    {
        Destroy(gameObject);
    }
		
}
