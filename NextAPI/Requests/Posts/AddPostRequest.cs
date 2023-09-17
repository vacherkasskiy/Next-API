namespace NextAPI.Requests.Posts;

public record AddPostRequest(int ReceiverId, string Text);