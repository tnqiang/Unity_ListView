using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NSUListView;

public class TestDiffsizeListItemView : IUListItemView {
	public Text text;
	RectTransform rectTrans = null;

	public override void SetData(object data)
	{
		int num = (int)data;
		text.text = num.ToString ();
		if (null == rectTrans) 
		{
			rectTrans = transform as RectTransform;
		}
		Vector2 rect = rectTrans.sizeDelta;
		rect.y = 100 + num;
		rectTrans.sizeDelta = rect;
	}

	public override Vector2 GetItemSize (object data)
	{
		int num = (int)data;
		Vector2 rect = new Vector2 (100, 100);
		rect.y += num;
		return rect;
	}
}