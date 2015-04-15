using UnityEngine;
using System;
using System.Collections;

public class Wait {
	public Wait(MonoBehaviour mb, float seconds, Action a) {
		mb.StartCoroutine(RunAndWait(seconds, a));
	}

	IEnumerator RunAndWait(float seconds, Action a) {
		yield return new WaitForSeconds(seconds);
		a();
	}
}

