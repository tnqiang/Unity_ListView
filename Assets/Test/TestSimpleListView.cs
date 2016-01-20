using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NSUListView;

public class TestSimpleListView : MonoBehaviour
{
	public USimpleListView listView;
	
	void Start()
	{
		List<object> lstData = 	new List<object> ();
		for (int i=0; i<100; ++i) {
			lstData.Add (i);
		}
		listView.SetData (lstData);
		StartCoroutine (DecreateCoroutine ());
		StartCoroutine (DecreateCoroutine2 ());
	}
	
	IEnumerator DecreateCoroutine()
	{
		yield return new WaitForSeconds(5);
		List<object> lstData = new List<object> ();
		for (int i=0; i<2; ++i) {
			lstData.Add (i);
		}
		listView.SetData (lstData);
	}

	IEnumerator DecreateCoroutine2()
	{
		yield return new WaitForSeconds(15);
		List<object> lstData = new List<object> ();
		for (int i=0; i<20; ++i) {
			lstData.Add (i);
		}
		listView.SetData (lstData);
	}
}