using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    
    public GameObject player;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");

        if (horz != 0)
        {
            player.transform.Translate(horz / 5, 0, 0);
        }
        if (vert != 0)
        {
            player.transform.Translate(0, 0, vert / 5);
        }
    }
}
