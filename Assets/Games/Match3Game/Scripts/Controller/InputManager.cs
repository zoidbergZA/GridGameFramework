using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GridGame;
using Match3;
using System;

public class InputManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Match3Game game;
	public float dragDistance = 5f;

	private FieldView selectedFieldView;
	private Vector2 startPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
		if (game.GameState != GameStates.Running)
			return;

		for (int i = 0; i < eventData.hovered.Count; i++)
		{
			if (eventData.hovered[i] == null)
				continue;

			FieldView fieldView = eventData.hovered[i].GetComponent<FieldView>();
			
			if (fieldView)
			{
				if (fieldView.Field.Gem != null)
					SelectView(fieldView, eventData.position);
			}
		}
    }

    public void OnDrag(PointerEventData eventData)
    {        
		if (game.GameState != GameStates.Running)
			return;

		var delta = eventData.position - startPos;
		
		if (delta.magnitude >= dragDistance)
		{
			HandleMove(delta);
			CancelSelect();
			eventData.pointerDrag = null;
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CancelSelect();
    }

	private void SelectView(FieldView view, Vector2 start)
	{
		// Debug.Log("select fieldView: " + view.name + ", " + view.Field.position);
		selectedFieldView = view;
		startPos = start;
	}

	private void CancelSelect()
	{
		selectedFieldView = null;
	}

	private void HandleMove(Vector2 dragInput)
	{
		if (selectedFieldView == null)
			return;

		SwapInput swapInput = new SwapInput();
		Vec2 swapDirection = Vec2.invalid;

		if (Mathf.Abs(dragInput.x) > Mathf.Abs(dragInput.y))
		{
			if (dragInput.x < 0)
				swapDirection = Vector2.left;
			else
				swapDirection = Vec2.right;
		}
		else
		{
			if (dragInput.y < 0)
				swapDirection = Vector2.down;
			else
				swapDirection = Vec2.up;
		}

		swapInput.from = selectedFieldView.Field.position;
		swapInput.to = swapInput.from + swapDirection;

		game.HandleInput(swapInput);
	}
}
