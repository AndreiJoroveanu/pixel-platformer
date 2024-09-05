using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour, IPointerEnterHandler
{
	[SerializeField] private GameObject SelectionArrow;
	[SerializeField] private int Position;
	private int change;

	// Changes the position of the selection arrow when the mouse hovers over a button
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		change = Position - SelectionArrow.GetComponent<SelectionArrow>().currentPosition;
		SelectionArrow.GetComponent<SelectionArrow>().ChangePosition(change);
	}
}
