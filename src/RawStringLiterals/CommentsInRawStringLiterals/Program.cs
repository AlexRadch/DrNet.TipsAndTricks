{
    var literal = $"""
        Line 1
        Сomment 1
        Comment 2
        Line 2 Comment 3
        Line 3
        continuation of line 3
        Empty Line Comment

        """;
    Console.WriteLine(literal);
}
{
    // Use `Ctrl+K, Ctrl+C` to comment a some text in IDE
    var literal = $"""
        Line 1
        //Сomment 1
        //Comment 2
        Line 2/* Comment 3*/
        Line 3
        continuation of line 3
        //Empty Line Comment

        """;
    Console.WriteLine(literal);
}
{
    // Use interpolation with empty string `{"" /* comment is here*/ }` around comments to remove them from result string
    var literal = $"""
        Line 1
        {""

        //Сomment 1
        //Comment 2
       }Line 2{""   /* Comment 3*/  }
        Line 3
        continuation of line 3
        {""
        
        //Empty Line Comment
       }
        """;
    Console.WriteLine(literal);
}
{
    // Use interpolation with empty string `{"" }` between lines to concatinate next line to privious without line break
    var literal = $"""
        Line 1
        {""

        //Сomment 1
        //Comment 2
       }Line 2{""   /* Comment 3*/  }
        Line 3 {"" // interpolation with empty string to concatinate next line without line break
       }continuation of line 3
        {""
        
        //Empty Line Comment
       }
        """;
    Console.WriteLine(literal);
}
{
    // When interpolation use many curly brackets `{{{` then comments looks ugly 
    var literal = $$$"""
        Line 1
        {{{""

       //Сomment 1
       //Comment 2
     }}}Line 2{{{""   /* Comment 3*/  }}}
        Line 3 {{{"" // interpolation with empty string to concatinate next line without line break
     }}}continuation of line 3
        {{{""

       //Empty Line Comment
     }}}
        """;
    Console.WriteLine(literal);
}

Console.WriteLine("Bye bye");
