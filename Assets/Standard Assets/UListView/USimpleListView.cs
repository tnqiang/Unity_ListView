using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NSUListView
{
	public enum Layout
	{
		Vertical,
		Horizontal
	}
	
	public enum Alignment
	{
		Left,
		Mid,
		Right,
		Top,
		Bottom
	}

	public class USimpleListView : IUListView
	{
		[Tooltip("List Item Object, must set")]
		public GameObject			itemPrefab;
		public Layout 				layout;
		public Alignment 			alignment;
		private List<GameObject>	lstItems;
		private Vector2				itemSize;

		public override void Init ()
		{
			base.Init ();
			switch (layout) 
			{
			case Layout.Horizontal:
				scrollRect.horizontal = true;
				scrollRect.vertical = false;
				break;
			case Layout.Vertical:
				scrollRect.horizontal = false;
				scrollRect.vertical = true;
				break;
			}

			// record the item size
			IUListItemView itemView = itemPrefab.GetComponent<IUListItemView> ();
			itemSize = itemView.GetItemSize (0);

			// spawn pool for listitems
			lstItems = new List<GameObject> ();
		}

		public override int	GetMaxShowItemNum()
		{
			int max = 0;
			// calculate the max show nums
			switch (layout) 
			{
			case Layout.Horizontal:
				max = (int)(scrollRectSize.x / itemSize.x) + 2;
				break;
			case Layout.Vertical:
				max = (int)(scrollRectSize.y / itemSize.y) + 2;
				break;
			}
			return max;
		}

		public override int GetStartIndex()
		{
			Vector2 anchorPosition = content.anchoredPosition;
			anchorPosition.x *= -1;
			int index = 0;

			switch (layout)
			{
			case Layout.Vertical:
				index = (int)(anchorPosition.y / (itemSize.y + spacing.y));
				break;
			case Layout.Horizontal:
				index = (int)(anchorPosition.x / (itemSize.x + spacing.x));
				break;
			}

			return index;
		}

		public override Vector2 GetItemAnchorPos(int index)
		{
			Vector2 basePos = Vector2.zero;
			Vector2 offset = Vector2.zero;
			RectTransform contentRectTransform = content.transform as RectTransform;
			Vector2 contentRectSize = contentRectTransform.rect.size;

			if (layout == Layout.Horizontal) 
			{
				basePos.x = -contentRectSize.x / 2 + itemSize.x / 2;
				offset.x = index * (itemSize.x + spacing.x);
				switch(alignment)
				{
				case Alignment.Top:
					offset.y = -(contentRectSize.y - itemSize.y)/2;
					break;
				case Alignment.Bottom:
					offset.y = (contentRectSize.y - itemSize.y)/2;
					break;
				}
			} 
			else 
			{
				basePos.y = contentRectSize.y / 2 - itemSize.y / 2;
				offset.y = -index * (itemSize.y + spacing.y);
				switch(alignment)
				{
				case Alignment.Left:
					offset.x = -(contentRectSize.x - itemSize.x)/2;
					break;
				case Alignment.Right:
					offset.x = (contentRectSize.x - itemSize.x)/2;
					break;
				}
			}

			return basePos + offset;
		}

		public override Vector2 GetContentSize()
		{
			Vector2 size = scrollRectSize;
			int count = lstData.Count;
			switch (layout) 
			{
			case Layout.Horizontal:
				size.x = itemSize.x * count + spacing.x *( count > 0 ? count -1 : count );
				break;
			case Layout.Vertical:
				size.y = itemSize.y * count + spacing.y * ( count > 0 ? count - 1 : count );
				break;
			}
			return size;
		}

		public override GameObject GetItemGameObject(int index)
		{
			if(index < lstItems.Count)
			{
				GameObject go = lstItems[index];
				if(false == go.activeSelf)
				{
					go.SetActive(true);
				}
				return lstItems [index];
			}
			else 
			{
				GameObject go = GameObject.Instantiate(itemPrefab) as GameObject;
				lstItems.Add (go);
				return go;
			}
		}

		public override void HideNonuseableItems ()
		{
			for (int i = lstData.Count; lstItems != null && i < lstItems.Count; ++i) 
			{
				if(lstItems[i].activeSelf)
				{
					lstItems[i].SetActive(false);
				}
			}
		}
	}
}