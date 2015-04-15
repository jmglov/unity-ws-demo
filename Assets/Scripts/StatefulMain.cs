using UnityEngine;
using WebSocketSharp;

public class StatefulMain : MonoBehaviour {
    public StateMachine stateMachine;

    private WebSocket ws;

    void Start() {
        ws = new WebSocket("ws://echo.websocket.org");

        ws.OnOpen += OnOpenHandler;
        ws.OnMessage += OnMessageHandler;
        ws.OnClose += OnCloseHandler;

        stateMachine.AddHandler(State.Running, () => {
            new Wait(this, 3, () => {
                ws.ConnectAsync();
            });
        });

        stateMachine.AddHandler(State.Connected, () => {
            stateMachine.Transition(State.Ping);
        });

        stateMachine.AddHandler(State.Ping, () => {
            new Wait(this, 3, () => {
               ws.SendAsync("This WebSockets stuff is a breeze!", OnSendComplete);
            });
        });

        stateMachine.AddHandler(State.Pong, () => {
            new Wait(this, 3, () => {
                ws.CloseAsync();
            });
        });

        stateMachine.Run();
    }

    private void OnOpenHandler(object sender, System.EventArgs e) {
        Debug.Log("WebSocket connected!");
        stateMachine.Transition(State.Connected);
    }

    private void OnMessageHandler(object sender, MessageEventArgs e) {
        Debug.Log("WebSocket server said: " + e.Data);
        stateMachine.Transition(State.Pong);
    }

    private void OnCloseHandler(object sender, CloseEventArgs e) {
        Debug.Log("WebSocket closed with reason: " + e.Reason);
        stateMachine.Transition(State.Done);
    }

    private void OnSendComplete(bool success) {
        Debug.Log("Message sent successfully? " + success);
    }
}
