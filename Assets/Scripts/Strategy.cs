using UnityEngine;
using UnityEngine.InputSystem;

public class Strategy : MonoBehaviour
{
    InputAction actionMove;
    Move _move;
    Look _look;

    enum StrategyCode {
        None,
        Move,
        Look
    }
    StrategyCode currentStarategy;

    void Start() {
        actionMove = InputSystem.actions.FindAction("NeedMove");
        _move = this.GetComponent<Move>();
        _look = this.GetComponent<Look>();
        currentStarategy = StrategyCode.None;
    }

    void Update() {
        var newStarategy = StrategyCode.None;
        if (actionMove.IsPressed())
            newStarategy = StrategyCode.Move;
        else
            newStarategy = StrategyCode.Look;

        if (newStarategy != currentStarategy) {
            switch (newStarategy) {
                case StrategyCode.None:
                    _move.Deactivate();
                    _look.Deactivate();
                    break;

                case StrategyCode.Move:
                    _move.Activate();
                    _look.Deactivate();
                    break;

                case StrategyCode.Look:
                    _move.Deactivate();
                    _look.Activate();
                    break;
            }
            currentStarategy = newStarategy;
        }
    }
}
