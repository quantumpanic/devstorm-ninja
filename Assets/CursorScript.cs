using UnityEngine;
using System.Collections;

public class CursorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        if (isFollowing)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.1f)
                transform.position = (Vector3.Lerp(transform.position, target.position, panSpeed * Time.deltaTime));
        }
    }

    public float panSpeed = 1;
    bool isPanning = false;

    void PanTo(Vector3 point)
    {
        isPanning = true;
        StartCoroutine(PanningTo(point));
    }

    IEnumerator PanningTo(Vector3 point)
    {
        if (Vector3.Distance(transform.position, point) > 0.1f)
        {
            transform.position = (Vector3.Lerp(transform.position, point, panSpeed * Time.deltaTime));
        }
        else isPanning = false;

        if (isPanning) yield return new WaitForFixedUpdate();
    }

    void PanTo(Transform entity)
    {
        PanTo(entity.position);
    }

    public Transform target;
    public bool isFollowing;

    void Follow(Transform entity, bool follow = true)
    {
        isFollowing = follow;
        target = entity;
    }

    void StopFollow()
    {
        isFollowing = false;
        target = null;
    }
}
