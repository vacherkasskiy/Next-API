namespace NextAPI.Requests.Messages;

public record AddMessageRequest(int AuthorId, int ReceiverId, string Text);