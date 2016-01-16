using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NSUListView;

public class TestListItemView : IUListItemView {
	public Text text;
	public override void SetData(object data)
	{
		int num = (int)data;
		text.text = num.ToString ();
	}
}
