using System.Collections.Generic;

namespace StockAdmin.Models;

public class PersonWithPosts : Person
{
    public List<Post> posts { get; set; }
}