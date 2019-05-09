using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

using Object = UnityEngine.Object;

namespace UnityMVVM.EventSystems
{
    /// <summary>
    /// <para>Receives events from the EventSystem and calls regostered VM methods.</para>
    /// </summary>
    [AddComponentMenu("MVVM/Event Trigger")]
    public class MVVMEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
    {
        [FormerlySerializedAs("delegates")]
        [SerializeField]
        private List<Entry> m_Delegates;

        protected MVVMEventTrigger()
        {
        }

        /// <summary>
        ///   <para>All the functions registered in this EventTrigger.</para>
        /// </summary>
        public List<Entry> triggers
        {
            get
            {
                if (this.m_Delegates == null)
                    this.m_Delegates = new List<Entry>();
                return this.m_Delegates;
            }
            set
            {
                this.m_Delegates = value;
            }
        }

        private void Execute(EventTriggerType id, BaseEventData eventData)
        {
            int index = 0;
            for (int count = this.triggers.Count; index < count; ++index)
            {
                Entry trigger = this.triggers[index];
                if (trigger.eventID == id && trigger.callback != null)
                    trigger.callback.Invoke(eventData);
            }
        }

        /// <summary>
        ///   <para>Called by the EventSystem when the pointer enters the object associated with this EventTrigger.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerEnter, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when the pointer exits the object associated with this EventTrigger.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerExit, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem every time the pointer is moved during dragging.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnDrag(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.Drag, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when an object accepts a drop.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnDrop(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.Drop, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a PointerDown event occurs.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerDown, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a PointerUp event occurs.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerUp, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a Click event occurs.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerClick, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a Select event occurs.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnSelect(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.Select, eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a new object is being selected.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnDeselect(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.Deselect, eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a Scroll event occurs.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnScroll(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.Scroll, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a Move event occurs.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnMove(AxisEventData eventData)
        {
            this.Execute(EventTriggerType.Move, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when the object associated with this EventTrigger is updated.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnUpdateSelected(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.UpdateSelected, eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a drag has been found, but before it is valid to begin the drag.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.InitializePotentialDrag, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called before a drag is started.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.BeginDrag, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem once dragging ends.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.EndDrag, (BaseEventData)eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a Submit event occurs.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnSubmit(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.Submit, eventData);
        }

        /// <summary>
        ///   <para>Called by the EventSystem when a Cancel event occurs.</para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public virtual void OnCancel(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.Cancel, eventData);
        }

        /// <summary>
        ///   <para>UnityEvent class for Triggers.</para>
        /// </summary>
        //[Serializable]
        //public class TriggerEvent : UnityEvent<BaseEventData>
        //{
        //    private string _vmName;
        //}
        [Serializable]
        public class TriggerEvent : ISerializationCallbackReceiver
        {
            [SerializeField]
            private List<TriggerCall> _calls;
            [SerializeField]
            private string _typeName;
            [SerializeField]
            private EventTriggerType _eventId;

            [SerializeField]
            private bool _callsDirty = true;

            private List<Action<BaseEventData>> _invokableList = new List<Action<BaseEventData>>();

            public TriggerEvent()
            {
                _calls = new List<TriggerCall>();
            }

            private void SetCallsDirty()
            {
                _callsDirty = true;
            }

            public void Invoke(BaseEventData e)
            {
                var prepareInvoke = PrepareInvoke();
                foreach (var action in prepareInvoke)
                {
                    action.Invoke(e);
                }
            }

            #region Serialization events
            public void OnBeforeSerialize()
            {
            }

            public void OnAfterDeserialize()
            {
                _callsDirty = true;
            }
            #endregion

            private List<Action<BaseEventData>> PrepareInvoke()
            {
                ReBuildCallsIfNeeded();
                // var invokeList = new List<Action<BaseEventData>>();
                return _invokableList;
            }

            private void ReBuildCallsIfNeeded()
            {
                if (!_callsDirty) return;
                InitializeCalls();
                _callsDirty = false;
            }

            private void InitializeCalls()
            {
                _invokableList.Clear();
                foreach (var call in _calls)
                {
                    if (!call.IsValid()) continue;
                    var runtimeCall = call.GetRuntimeCall(this);
                    if (runtimeCall != null)
                        _invokableList.Add(runtimeCall);
                }
            }

            private MethodInfo FindMethod(TriggerCall call)
            {
                var argType = typeof(Object);
                if (!string.IsNullOrEmpty(call.Arguments.unityObjectArgumentAssemblyTypeName))
                    argType = Type.GetType(call.Arguments.unityObjectArgumentAssemblyTypeName, false) ?? typeof(Object);
                return FindMethod(call.MethodName, call.Target, call.Mode, argType);
            }

            private MethodInfo FindMethod(string name, object listener, PersistentListenerMode mode, Type argumentType)
            {
                switch (mode)
                {
                    case PersistentListenerMode.EventDefined:
                        return FindMethod(name, listener);
                    case PersistentListenerMode.Void:
                        return GetValidMethodInfo(listener, name, new Type[0]);
                    case PersistentListenerMode.Object:
                        return GetValidMethodInfo(listener, name, new Type[1]
                                {
                                    argumentType ?? typeof(Object)
                                });
                    case PersistentListenerMode.Int:
                        return GetValidMethodInfo(listener, name, new Type[1]
                                {
                                    typeof(int)
                                });
                    case PersistentListenerMode.Float:
                        return GetValidMethodInfo(listener, name, new Type[1]
                                {
                                    typeof(float)
                                });
                    case PersistentListenerMode.String:
                        return GetValidMethodInfo(listener, name, new Type[1]
                                {
                                    typeof(string)
                                });
                    case PersistentListenerMode.Bool:
                        return GetValidMethodInfo(listener, name, new Type[1]
                                {
                                    typeof(bool)
                                });
                    default:
                        return null;
                }
            }

            private MethodInfo FindMethod(string name, object listener)
            {
                return GetValidMethodInfo(listener, name, EventDefinedMethodArgs(_eventId));
            }

            private static Type[] EventDefinedMethodArgs(EventTriggerType eventId)
            {
                switch (eventId)
                {
                    case EventTriggerType.PointerEnter:
                        return new Type[1]
                        {
                            typeof(PointerEventData)
                        };
                    case EventTriggerType.PointerExit:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.PointerDown:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.PointerUp:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.PointerClick:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.Drag:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.Drop:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.Scroll:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.UpdateSelected:
                        return new Type[1]
                        {
                            typeof(BaseEventData)
                        };
                    case EventTriggerType.Select:
                        goto case EventTriggerType.UpdateSelected;
                    case EventTriggerType.Deselect:
                        goto case EventTriggerType.UpdateSelected;
                    case EventTriggerType.Move:
                        return new Type[1]
                        {
                            typeof(AxisEventData)
                        };
                    case EventTriggerType.InitializePotentialDrag:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.BeginDrag:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.EndDrag:
                        goto case EventTriggerType.PointerEnter;
                    case EventTriggerType.Submit:
                        goto case EventTriggerType.UpdateSelected;
                    case EventTriggerType.Cancel:
                        goto case EventTriggerType.UpdateSelected;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(eventId), eventId, null);
                }
            }

            private MethodInfo GetValidMethodInfo(object obj, string funcName, Type[] argumentTypes)
            {
                // TODO use Commands instead of methods here!
                for (System.Type type = obj.GetType(); type != typeof(object) && type != null; type = type.BaseType)
                {
                    // MethodInfo method = type.GetMethod(funcName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder)null, argumentTypes, (ParameterModifier[])null);
                    var command = type.GetField(funcName);
                    var isReactiveCommand = command.FieldType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReactiveCommand<>));
                    if(!isReactiveCommand) continue;

                    var method = command.FieldType.GetMethod("Execute", argumentTypes);
                    if (method == null) continue;
                    var parameters = method.GetParameters();
                    var flag = true;
                    var index = 0;
                    foreach (var parameterInfo in parameters)
                    {
                        flag = argumentTypes[index].IsPrimitive == parameterInfo.ParameterType.IsPrimitive;
                        if (flag)
                            ++index;
                        else
                            break;
                    }
                    if (flag)
                        return method;
                }
                return (MethodInfo)null;
            }

            [Serializable]
            private class TriggerCall
            {
                [SerializeField]
                private PersistentListenerMode _mode = PersistentListenerMode.EventDefined;
                [SerializeField]
                private MVVMArgumentCache _arguments = new MVVMArgumentCache();
                [SerializeField]
                private UnityEventCallState _callState = UnityEventCallState.RuntimeOnly;
                [SerializeField]
                private UnityEngine.Object _target;
                [SerializeField]
                private string _methodName;
                [SerializeField]
                private string _vmName;

                public PersistentListenerMode Mode => _mode;

                public MVVMArgumentCache Arguments => _arguments;

                public ViewModel.ViewModelBase Target => Util.ViewModelProvider.Instance.GetViewModelBehaviour(_vmName);

                public string MethodName => _methodName;

                private object TargetCommand => Target.GetType().GetField(MethodName).GetValue(Target);

                public Action<BaseEventData> GetRuntimeCall(TriggerEvent te)
                {
                    if (_callState == UnityEventCallState.RuntimeOnly && !Application.isPlaying || (_callState == UnityEventCallState.Off))
                        return null;
                    var method = te.FindMethod(this);
                    if (method == null)
                        return null;
                    // TODO complete this switch case
                    return CreateDelegate(TargetCommand, method, Arguments, Mode);
                }

                public bool IsValid()
                {
                    // return /* _target != null */ Util.ViewModelProvider. && !string.IsNullOrEmpty(_methodName);
                    return Util.ViewModelProvider.IsViewModelTypeNameValid(_vmName) && !string.IsNullOrEmpty(_methodName);
                }

                private static Action<BaseEventData> CreateDelegate(object target, MethodInfo method, MVVMArgumentCache arguments, PersistentListenerMode mode)
                {
                    Action<BaseEventData> @delegate;
                    if (mode == PersistentListenerMode.EventDefined)
                        @delegate = (e) => method.Invoke(target, new object[] {e});
                    else
                    {
                        var callParam = arguments.GetCallParam(mode);
                        @delegate = (e) => method.Invoke(target, callParam);
                    }
                    return @delegate;
                }
            }
        }

        /// <summary>
        ///   <para>An Entry in the EventSystem delegates list.</para>
        /// </summary>
        [Serializable]
        public class Entry
        {
            /// <summary>
            ///   <para>What type of event is the associated callback listening for.</para>
            /// </summary>
            public EventTriggerType eventID = EventTriggerType.PointerClick;
            /// <summary>
            ///   <para>The desired TriggerEvent to be Invoked.</para>
            /// </summary>
            public TriggerEvent callback = new TriggerEvent();
            public string nanaynay = "rain rain go away";
        }
    }
}
