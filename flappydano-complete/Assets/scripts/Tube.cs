using UnityEngine;
using System.Collections;

public class Tube : MonoBehaviour {

	GameObject bird;

	void Start() {
		bird = GameObject.Find("Bird");
		StartCoroutine( DestroyTube() );
	}
	
	void Update() {

		if( !bird.GetComponent<Bird>().isDead ) {
			Vector3 curPos = transform.position;
			curPos.x -= 20 * Time.deltaTime;
			transform.position = curPos;
		}
	}

	IEnumerator DestroyTube() {
		yield return new WaitForSeconds( 20.0f );
		Destroy( gameObject );
	}
	
}