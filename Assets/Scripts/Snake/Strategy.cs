// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;
using UnityEngine.InputSystem;

public class Strategy : MonoBehaviour
{
    InputAction action;
    Meander meander;

    enum StrategyCode {
        None,
        Meander,
    }
    StrategyCode currentStarategy;


    void Start() {
        action = InputSystem.actions.FindAction("Move");
        meander = this.GetComponent<Meander>();
        currentStarategy = StrategyCode.None;
    }


    void Update() {
        var newStarategy = StrategyCode.None;
        if (action.IsPressed())
            newStarategy = StrategyCode.Meander;
        else
            newStarategy = StrategyCode.None;

        if (newStarategy != currentStarategy) {
            switch (newStarategy) {
                case StrategyCode.None:
                    meander.Deactivate();
                    break;

                case StrategyCode.Meander:
                    meander.Activate();
                    break;
            }
            currentStarategy = newStarategy;
        }
    }
}
