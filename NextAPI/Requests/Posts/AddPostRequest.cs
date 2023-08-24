namespace NextAPI.Requests.Posts;

public record AddPostRequest(int AuthorId, int ReceiverId, string Text);