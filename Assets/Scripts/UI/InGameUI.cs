using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour {



	RawImage fadeImage;
	public bool isFading;

	void Awake (){
		fadeImage = GetComponent<RawImage> ();
		isFading = false;
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void FadeToBlack(bool toBlack, float fadingDuration){
		StartCoroutine (FadeToBlackCoroutine (toBlack, fadingDuration));
	}

	public IEnumerator FadeToBlackCoroutine(bool toBlack, float fadingDuration){

		isFading = true;

		for (float i = 0; i < 1; i += Time.deltaTime / fadingDuration) {

			if (toBlack) {
				fadeImage.color = new Color (0, 0, 0, i);
			} else {
				float j = 1 - i;
				fadeImage.color = new Color (0, 0, 0, j);
			}
			yield return null;
		}
		//Debug.Log ("FadingIn Coroutine Fini");
		isFading = false;
	}









}
