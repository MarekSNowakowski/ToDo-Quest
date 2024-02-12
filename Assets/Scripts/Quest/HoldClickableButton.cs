using System;
using UnityEngine.EventSystems;
using UnityEngine;

public class HoldClickableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _holdDuration;

    public event Action OnClicked;
    public event Action OnHoldClicked;
    public Quest quest;

    private bool _isHoldingButton;
    private float _elapsedTime;
    private PointerEventData pointerEventData;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerEventData = eventData;
        ToggleHoldingButton(true);
    }
    
    private void ToggleHoldingButton(bool isPointerDown)
    {
        _isHoldingButton = isPointerDown;

        if (isPointerDown)
            _elapsedTime = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ManageButtonInteraction(eventData, true);
        ToggleHoldingButton(false);
    }

    private void ManageButtonInteraction(PointerEventData eventData, bool isPointerUp = false)
    {
        if (!_isHoldingButton)
            return;

        if (isPointerUp)
        {
            Click();
            return;
        }

        _elapsedTime += Time.deltaTime;
        var isHoldClickDurationReached = _elapsedTime > _holdDuration;

        if (isHoldClickDurationReached)
            HoldClick(eventData);
    }

    private void Click()
    {
        OnClicked?.Invoke();
    }

    private void HoldClick(PointerEventData eventData)
    {
        ToggleHoldingButton(false);
        quest.OnHold(eventData);
    }

    private void Update() => ManageButtonInteraction(pointerEventData);
}