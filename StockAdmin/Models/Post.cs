namespace StockAdmin.Models;

public class Post
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }

    public override bool Equals(object? obj)
    {
        Post post = obj as Post;
        return name == post?.name;
    }
}