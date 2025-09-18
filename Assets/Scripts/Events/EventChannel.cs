using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Scriptable object for event channel. Use RaiseEvent to Invoke the context
/// </summary>
/// <typeparam name="T">EventContext: Custom context to invoke</typeparam>
public abstract class BaseEventChannelSO<T> : ScriptableObject where T : EventContext
{
    public UnityAction<T> OnEventRaised;
    public void RaiseEvent(T context)
    {
        OnEventRaised?.Invoke(context);
    }
}

/// <summary>
/// Listener to listen to a certain event
/// </summary>
/// <typeparam name="TEventChannel">Event Channel SO</typeparam>
/// <typeparam name="TEventContext">Event Context</typeparam>
public abstract class BaseEventChannelListener<TEventChannel, TEventContext> : MonoBehaviour
    where TEventChannel : BaseEventChannelSO<TEventContext> where TEventContext : EventContext
{
    [SerializeField] protected TEventChannel eventChannel;
    [SerializeField] protected UnityEvent<TEventContext> onResponse;
    
    protected virtual void OnEnable()
    {
        if(eventChannel == null) return;
        eventChannel.OnEventRaised += OnEventRaised;
    }

    protected virtual void OnDisable()
    {
        if(eventChannel == null) return;
        eventChannel.OnEventRaised -= OnEventRaised;
    }

    protected virtual void OnEventRaised(TEventContext ctx)
    {
        onResponse?.Invoke(ctx);
    }
}

/// <summary>
/// Custom event context.
/// Inherit from this class to create a custom event for event channel
/// </summary>
public abstract class EventContext { }
