using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;

namespace MauiSignalRSample;

public partial class MainPage : ContentPage
{
    private readonly HubConnection _connection;
    private bool _isConnected = false;
    private ObservableCollection<ChatMessage> messages;

    public MainPage()
    {
        InitializeComponent();

        messages = new ObservableCollection<ChatMessage>();
        MessagesCollection.ItemsSource = messages;

        _connection = new HubConnectionBuilder()
            .WithUrl("http://52.66.240.38:5296/chat")
            .Build();

        _connection.On<string>("MessageReceived", (message) =>
        {
            Dispatcher.Dispatch(() =>
            {
                if (!IsSentMessage(message))
                {
                    messages.Add(new ChatMessage { Text = message, IsSent = false });
                }
            });
        });

        _connection.Closed += async (error) =>
        {
            _isConnected = false;
            await DisplayAlert("Connection", "Disconnected from server", "OK");
        };
    }

    private async void OnConnectClicked(object sender, EventArgs e)
    {
        if (_isConnected)
        {
            await DisplayAlert("Connection", "Already connected to server", "OK");
            return;
        }

        if (passwordEntry.Text == "Prahi@2005")
        {
            try
            {
                await _connection.StartAsync();
                _isConnected = true;
                myChatMessage.IsEnabled = true;
                sendButton.IsEnabled = true;
                connectButton.IsEnabled = false;
                passwordEntry.IsVisible = false;
                connectButton.IsVisible = false;
                await DisplayAlert("Server Connection", "Server Connected", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Connection Failed", "Failed to connect to server. Try again.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Incorrect password. Server not connected.", "OK");
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        if (!_isConnected)
        {
            await DisplayAlert("Error", "Not connected to the server.", "OK");
            return;
        }

        var message = myChatMessage.Text;
        messages.Add(new ChatMessage { Text = message, IsSent = true });

        LastSentMessage = message;

        await _connection.InvokeCoreAsync("SendMessage", args: new[] { message });
        myChatMessage.Text = string.Empty;
    }

    private string LastSentMessage = string.Empty;

    private bool IsSentMessage(string message)
    {
        return message == LastSentMessage;
    }
}

public class ChatMessage
{
    public string Text { get; set; }
    public bool IsSent { get; set; }
}
