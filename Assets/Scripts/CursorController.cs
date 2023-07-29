using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum GameCursorState
{
    default_state,
    hovering_state,
    move_state,
    attack_state
}

public class CursorController : MonoBehaviour
{
    [SerializeField] private Sprite[] cursorSprites;
    [SerializeField] private SpriteRenderer cursorImage;

    Vector2 pos;

    public List<Clickable> clickables;
    public Clickable selectedClickable {get; private set;}

    [SerializeField] public int restingCursorState;

    private int cursorState;
    public int CursorState
    {
        get
        {
            return cursorState;
        }
        set
        {
            if(value == cursorState) return;
            cursorState = value;
            OnCursorStateChange(cursorState);
            //Debug.Log(CursorState.ToString());
        }
    }

    public delegate void OnCursorStateChangeDelegate(int state);
    public event OnCursorStateChangeDelegate OnCursorStateChange;

    public static CursorController Instance {get; private set;}

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        OnCursorStateChange += ChangeCursorImage;

        clickables = new List<Clickable>();
    }

    private void FixedUpdate() 
    {
        transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    private void ChangeCursorImage(int state)
    {
        cursorImage.sprite = cursorSprites[state];
    }

    public void AddClickable(Clickable clickable)
    {
        clickables.Add(clickable);
        clickables[0].UpdateClickableState();
    }

    public void RemoveClickable(Clickable clickable)
    {
        clickables.Remove(clickable);
        if(clickables.Count > 0) clickables[0].UpdateClickableState();
        else if(selectedClickable == null)
        {
            CursorState = restingCursorState;
        }
    }

    public void SetSelectedClickable(Clickable clickable)
    {
        selectedClickable = clickable;
        CursorState = (int) GameCursorState.default_state;
    }

    public void UnsetSelectedClickable()
    {
        selectedClickable = null;
    }
}
