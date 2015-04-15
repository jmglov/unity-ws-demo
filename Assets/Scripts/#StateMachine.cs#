using System.Collections.Generic;
using UnityEngine;

public enum State { NotRunning, Running, Connected, Ping, Pong, Done }

public delegate void Handler();

public class StateMachine : MonoBehaviour {
    private readonly object syncLock = new object();
    private readonly Queue<State> pendingTransitions = new Queue<State>();
    private readonly Dictionary<State, Handler> handlers
        = new Dictionary<State, Handler>();

    [SerializeField]
    private State currentState = State.NotRunning;

    public void Run() {
        Transition(State.Running);
    }

    public void AddHandler(State state, Handler handler) {
        handlers.Add(state, handler);
    }

    public void Transition(State state) {
        State cur;
        lock(syncLock) {
            cur = currentState;
            pendingTransitions.Enqueue(state);
        }

        Debug.Log("Queued transition from " + cur + " to " + state);
    }

    public void Update() {
        while (pendingTransitions.Count > 0) {
            currentState = pendingTransitions.Dequeue();
            Debug.Log("Transitioned to state " + currentState);

            Handler handler;
            if (handlers.TryGetValue(currentState, out handler)) {
                handler();
            }
        }
    }
}
