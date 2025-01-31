//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/KWS/Input/HardwareInputActions.inputactions
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

public partial class @HardwareInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @HardwareInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""HardwareInputActions"",
    ""maps"": [
        {
            ""name"": ""Hardware"",
            ""id"": ""d0323fde-259a-490f-b056-fab89204294c"",
            ""actions"": [
                {
                    ""name"": ""Interactions"",
                    ""type"": ""Button"",
                    ""id"": ""cf4d268c-0d73-45e4-a1a0-b033f39ee5e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a1f8be09-a362-460f-a82f-3973340849d6"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interactions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Hardware
        m_Hardware = asset.FindActionMap("Hardware", throwIfNotFound: true);
        m_Hardware_Interactions = m_Hardware.FindAction("Interactions", throwIfNotFound: true);
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

    // Hardware
    private readonly InputActionMap m_Hardware;
    private List<IHardwareActions> m_HardwareActionsCallbackInterfaces = new List<IHardwareActions>();
    private readonly InputAction m_Hardware_Interactions;
    public struct HardwareActions
    {
        private @HardwareInputActions m_Wrapper;
        public HardwareActions(@HardwareInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Interactions => m_Wrapper.m_Hardware_Interactions;
        public InputActionMap Get() { return m_Wrapper.m_Hardware; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HardwareActions set) { return set.Get(); }
        public void AddCallbacks(IHardwareActions instance)
        {
            if (instance == null || m_Wrapper.m_HardwareActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_HardwareActionsCallbackInterfaces.Add(instance);
            @Interactions.started += instance.OnInteractions;
            @Interactions.performed += instance.OnInteractions;
            @Interactions.canceled += instance.OnInteractions;
        }

        private void UnregisterCallbacks(IHardwareActions instance)
        {
            @Interactions.started -= instance.OnInteractions;
            @Interactions.performed -= instance.OnInteractions;
            @Interactions.canceled -= instance.OnInteractions;
        }

        public void RemoveCallbacks(IHardwareActions instance)
        {
            if (m_Wrapper.m_HardwareActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IHardwareActions instance)
        {
            foreach (var item in m_Wrapper.m_HardwareActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_HardwareActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public HardwareActions @Hardware => new HardwareActions(this);
    public interface IHardwareActions
    {
        void OnInteractions(InputAction.CallbackContext context);
    }
}
