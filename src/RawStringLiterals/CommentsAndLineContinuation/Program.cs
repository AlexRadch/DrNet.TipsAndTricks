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
    var literal = $"""
        Line 1
        {""

        //Сomment 1
        //Comment 2
       }Line 2{""   /* Comment 3*/  }
        Line 3 {""
       }continuation of line 3
        {""
        
        //Empty Line Comment
       }
        """;
    Console.WriteLine(literal);
}
{
    var literal = $$$"""
        Line 1
        {{{""

       //Сomment 1
       //Comment 2
     }}}Line 2{{{""   /* Comment 3*/  }}}
        Line 3 {{{""
     }}}continuation of line 3
        {{{""

       //Empty Line Comment
     }}}
        """;
    Console.WriteLine(literal);
}
{
    var literal = $$$"""
        
        """;
    Console.Write(literal);
}
