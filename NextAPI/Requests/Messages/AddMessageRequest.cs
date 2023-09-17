namespace NextAPI.Requests.Messages;

public record AddMessageRequest(int ReceiverId, string Text);