using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace NSUListView
{
	[RequireComponent(typeof(ScrollRect))]
	public abstract class IUListView : MonoBehaviour
	{
		[Tooltip("List Item Object, must set")]
		public GameObject 			item;
		public Vector2				pad;

		protected ScrollRect 		scrollRect;
		protected bool				initialized = false;
		protected RectTransform		content;
		protected Vector2			scrollRectSize;
		protected Vector2			itemSize;
		protected int				lastStartInex = 0;
		protected List<object>		lstData;
		protected int				maxShowNum;

		public virtual void Init()
		{
			if (initialized)
				return;

			if (null == item) 
			{
				Debug.LogError("set an listitem first");
				return;
			}

			// set attributes of scrollrect
			scrollRect = GetComponent<ScrollRect> ();
			scrollRect.onValueChanged.AddListener (OnValueChanged);

			// add a scrollrect content
			GameObject go = new GameObject ();
			go.name = "content";
			content = go.AddComponent (typeof(RectTransform)) as RectTransform;
			content.SetParent (transform);
			content.pivot = new Vector2 (0, 1);
			content.anchorMin = content.anchorMax = content.pivot;
			content.anchoredPosition = Vector2.zero;
			scrollRect.content = content;

			// record some sizes
			RectTransform scrollRectTransform = scrollRect.transform as RectTransform;
			scrollRectSize = scrollRectTransform.sizeDelta;
			RectTransform itemRectTransform = item.transform as RectTransform;
			itemSize = itemRectTransform.sizeDelta;

			// record the max shown item num
			maxShowNum = GetMaxShowItemNum();

			initialized = true;
		}

		public void  SetData(List<object> lstData)
		{
			Init ();
			this.lstData = lstData;
			RefreshListView ();
		}

		private void OnValueChanged(Vector2 pos)
		{
			int startIndex = GetStartIndex ();
			if (startIndex != lastStartInex 
			    && startIndex >= 0)
			{
				RefreshListView();
				lastStartInex = startIndex;
			}
		}

		protected void RefreshListView()
		{
			// set the content size
			Vector2 size = GetContentSize ();
			RectTransform contentRectTransform = content.transform as RectTransform;
			contentRectTransform.sizeDelta = size;

			// set the item postion and data
			int startIndex = GetStartIndex ();
			for (int i=0; i<maxShowNum && startIndex + i < lstData.Count; ++i)
			{
				GameObject go = GetItemGameObject(i);
				RectTransform trans = go.transform as RectTransform;
				trans.SetParent(content);
				trans.pivot = trans.anchorMin = trans.anchorMax = new Vector2(0.5f, 0.5f);
				trans.anchoredPosition = GetItemAnchorPos(startIndex + i);
				IUListItemView itemView = go.GetComponent<IUListItemView>();
				itemView.SetData(lstData[startIndex + i]);
			}
		}

		/// <summary>
		/// Gets the max show item number.
		/// </summary>
		/// <returns>The max show item number.</returns>
		public abstract int			GetMaxShowItemNum();
		/// <summary>
		/// Gets the rect tranform size of the content of the ScrollRect
		/// </summary>
		/// <returns>The content size.</returns>
		public abstract Vector2 	GetContentSize();
		/// <summary>
		/// Gets the anchor position of the item indexed index
		/// Assume that anchorMin = anchorMax = pivot = new Vector(0.5f, 0.5f)
		/// </summary>
		/// <returns>The item anchor position.</returns>
		/// <param name="index">index of item</param>
		public abstract Vector2 	GetItemAnchorPos(int index);
		/// <summary>
		/// Gets the start index of the item to be shown
		/// </summary>
		/// <returns>The start index(start from 0)</returns>
		public abstract int 		GetStartIndex();
		/// <summary>
		/// Gets the item game object.
		/// </summary>
		/// <returns>The item game object.</returns>
		/// <param name="index">Index.</param>
		public abstract GameObject	GetItemGameObject(int index);
	}
}