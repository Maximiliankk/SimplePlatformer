using UnityEngine;
using CarbonInput;

/// <summary>
/// Represents the current state of a specific gamepad. 
/// The state of any button can be accessed by the attributes <see cref="A"/>, <see cref="B"/>,... or via the method <see cref="Button(CButton)"/>. <para/>
/// <see cref="Pressed(CButton)"/> returns true during the frame it was pressed. <para/>
/// <see cref="Released(CButton)"/> returns true during the frame it was released. <para/>
/// <see cref="Left"/>, <see cref="Right"/> and <see cref="DPad"/> will give you direct access to the corresponding axes. 
/// The trigger can be accessed by <see cref="LT"/> and <see cref="RT"/>.
/// </summary>
// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming
public class GamePadState {
    /// <summary>
    /// Stores the state of all buttons from the last frame.
    /// </summary>
    private bool[] LastFrameButtons = new bool[CarbonController.ButtonCount];
    /// <summary>
    /// Stores the state of all buttons from this frame.
    /// </summary>
    private bool[] Buttons = new bool[CarbonController.ButtonCount];
    /// <summary>
    /// Is true while <see cref="CButton.A"/> is pressed.
    /// </summary>
    public bool A { get { return Buttons[(int)CButton.A]; } }
    /// <summary>
    /// Is true while <see cref="CButton.B"/> is pressed.
    /// </summary>
    public bool B { get { return Buttons[(int)CButton.B]; } }
    /// <summary>
    /// Is true while <see cref="CButton.X"/> is pressed.
    /// </summary>
    public bool X { get { return Buttons[(int)CButton.X]; } }
    /// <summary>
    /// Is true while <see cref="CButton.Y"/> is pressed.
    /// </summary>
    public bool Y { get { return Buttons[(int)CButton.Y]; } }
    /// <summary>
    /// Is true while <see cref="CButton.Back"/> is pressed.
    /// </summary>
    public bool Back { get { return Buttons[(int)CButton.Back]; } }
    /// <summary>
    /// Is true while <see cref="CButton.Start"/> is pressed.
    /// </summary>
    public bool Start { get { return Buttons[(int)CButton.Start]; } }
    /// <summary>
    /// Is true while <see cref="CButton.LB"/> is pressed.
    /// </summary>
    public bool LB { get { return Buttons[(int)CButton.LB]; } }
    /// <summary>
    /// Is true while <see cref="CButton.RB"/> is pressed.
    /// </summary>
    public bool RB { get { return Buttons[(int)CButton.RB]; } }
    /// <summary>
    /// Is true while <see cref="CButton.LS"/> is pressed.
    /// </summary>
    public bool LS { get { return Buttons[(int)CButton.LS]; } }
    /// <summary>
    /// Is true while <see cref="CButton.RS"/> is pressed.
    /// </summary>
    public bool RS { get { return Buttons[(int)CButton.RS]; } }
    /// <summary>
    /// X and Y axis of the left thumbstick.
    /// </summary>
    public Vector2 Left;
    /// <summary>
    /// X and Y axis of the right thumbstick.
    /// </summary>
    public Vector2 Right;
    /// <summary>
    /// Left trigger.
    /// </summary>
    public float LT;
    /// <summary>
    /// Right trigger.
    /// </summary>
    public float RT;
    /// <summary>
    /// X and Y axis of the dpad.
    /// </summary>
    public Vector2 DPad;

    /// <summary>
    /// Defines the owner of this <see cref="GamePadState"/>.
    /// </summary>
    private readonly PlayerIndex Index;
    /// <summary>
    /// Number of the last frame, used to determine if we're in a new frame or not.
    /// </summary>
    private int LastFrame;

    /// <summary>
    /// Returns true while the button is pressed.
    /// </summary>
    /// <param name="btn"></param>
    /// <returns></returns>
    public bool Button(CButton btn) { return Buttons[(int)btn]; }
    /// <summary>
    /// Returns true during the frame the user pressed the button.
    /// </summary>
    /// <param name="btn"></param>
    /// <returns></returns>
    public bool Pressed(CButton btn) { return Buttons[(int)btn] && !LastFrameButtons[(int)btn]; }
    /// <summary>
    /// Returns true during the frame the user released the button.
    /// </summary>
    /// <param name="btn"></param>
    /// <returns></returns>
    public bool Released(CButton btn) { return !Buttons[(int)btn] && LastFrameButtons[(int)btn]; }

	/// <summary>
	/// Returns true if any button is currently pressed.
	/// </summary>
	public bool AnyButton { get; private set; }
	/// <summary>
	/// Returns true if any axis is currently not zero.
	/// </summary>
	public bool AnyAxis { get; private set; }
	/// <summary>
	/// Returns true if any button is currently pressed or if any axis is currently not zero.
	/// </summary>
	public bool AnyButtonOrAxis { get { return AnyButton || AnyAxis; } }

	/// <summary>
	/// Returns a button that is currently pressed or null if no buttons are pressed.
	/// </summary>
	/// <returns></returns>
	public CButton? GetAnyButton() {
		for(int i = 0; i < CarbonController.ButtonCount; i++)
			if(Buttons[i]) return (CButton)i;
		return null;
	}

	/// <summary>
	/// Returns an axis that is not zero or null if all axis are zero.
	/// </summary>
	/// <returns></returns>
	public CAxis? GetAnyAxis() {
		for(int i = 0; i < CarbonController.AxisCount; i++)
			if(Mathf.Abs(GamePad.GetAxis((CAxis)i, Index)) > 0.05f) return (CAxis)i;
		return null;
	}

	public GamePadState(PlayerIndex id) {
        Index = id;
    }

    /// <summary>
    /// This will update all buttons and axes of this instance.
    /// Multiple calls in the same frame won't have any effect.
    /// </summary>
    public void Update() {
        if(LastFrame == Time.frameCount) return;
        LastFrame = Time.frameCount;
        SwapArrays();
	    AnyButton = false;
        for(int i = 0; i < Buttons.Length; i++) {
			AnyButton |= (Buttons[i] = GamePad.GetButton((CButton)i, Index));
        }
        Left = GamePad.GetLeftStick(Index);
        Right = GamePad.GetRightStick(Index);
        DPad = GamePad.GetDPad(Index);
        LT = GamePad.GetLeftTrigger(Index);
        RT = GamePad.GetRightTrigger(Index);
	    AnyAxis = Left.sqrMagnitude > 0.0025f || Right.sqrMagnitude > 0.0025f || DPad.sqrMagnitude > 0.0025f || LT > 0.05f || RT > 0.05f;
    }
    
    private void SwapArrays() {
        bool[] tmp = LastFrameButtons;
        LastFrameButtons = Buttons;
        Buttons = tmp;
    }
}
