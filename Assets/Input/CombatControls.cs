//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Input/CombatControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @CombatControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CombatControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CombatControls"",
    ""maps"": [
        {
            ""name"": ""Combat"",
            ""id"": ""c75aad76-46a9-450f-9149-542cb5f29a58"",
            ""actions"": [
                {
                    ""name"": ""StopCombat"",
                    ""type"": ""Button"",
                    ""id"": ""1909d215-7b36-4d4f-a53a-b4f3c7b28f57"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d98d3a37-a15b-4d0e-b340-99b093762e58"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopCombat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Combat
        m_Combat = asset.FindActionMap("Combat", throwIfNotFound: true);
        m_Combat_StopCombat = m_Combat.FindAction("StopCombat", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Combat
    private readonly InputActionMap m_Combat;
    private ICombatActions m_CombatActionsCallbackInterface;
    private readonly InputAction m_Combat_StopCombat;
    public struct CombatActions
    {
        private @CombatControls m_Wrapper;
        public CombatActions(@CombatControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @StopCombat => m_Wrapper.m_Combat_StopCombat;
        public InputActionMap Get() { return m_Wrapper.m_Combat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CombatActions set) { return set.Get(); }
        public void SetCallbacks(ICombatActions instance)
        {
            if (m_Wrapper.m_CombatActionsCallbackInterface != null)
            {
                @StopCombat.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnStopCombat;
                @StopCombat.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnStopCombat;
                @StopCombat.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnStopCombat;
            }
            m_Wrapper.m_CombatActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StopCombat.started += instance.OnStopCombat;
                @StopCombat.performed += instance.OnStopCombat;
                @StopCombat.canceled += instance.OnStopCombat;
            }
        }
    }
    public CombatActions @Combat => new CombatActions(this);
    public interface ICombatActions
    {
        void OnStopCombat(InputAction.CallbackContext context);
    }
}
