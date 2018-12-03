using UnityEngine;

public class InputEntry
{
    float _last;

    string _buttonKey;
    float _delay;

    public InputEntry(string key, float delay)
    {
        _buttonKey = key;
        _delay = delay;
        _last = -1;
    }

    public bool Read()
    {
        if ((_last < 0 || Time.time - _last >= _delay) && Input.GetButton(_buttonKey))
        {
            _last = Time.time;
            return true;
        }
        return false;
    }
}

public class GameInput
{
    const int kNumInputs = 3;
    float _moveInputDelay;

    public int xAxis;
    public int yAxis;
    public bool idleTurn;
    public bool[] numbersPressed;

    InputEntry leftInput;
    InputEntry rightInput;
    InputEntry upInput;
    InputEntry downInput;
    InputEntry idleInput;

    InputEntry[] numInputs;

    public GameInput(float inputDelay)
    {
        _moveInputDelay = inputDelay;
        leftInput = new InputEntry("left", _moveInputDelay);
        rightInput = new InputEntry("right", _moveInputDelay);
        upInput = new InputEntry("up", _moveInputDelay);
        downInput = new InputEntry("down", _moveInputDelay);
        idleInput = new InputEntry("idle", _moveInputDelay);

        numInputs = new InputEntry[kNumInputs];
        numbersPressed = new bool[kNumInputs];
        for(int i = 0; i < kNumInputs; ++i)
        {
            numInputs[i] = new InputEntry("num" + (i+1).ToString(), _moveInputDelay);
            numbersPressed[i] = false;
        }
    }

    public void Read()
    {
        xAxis = leftInput.Read() ? -1 : rightInput.Read() ? 1 : 0;
        yAxis = downInput.Read() ? -1 : upInput.Read() ? 1 : 0;
        idleTurn = idleInput.Read();
        for(int i = 0; i < kNumInputs; ++i)
        {
            numbersPressed[i] = numInputs[i].Read();
        }
    }
}
