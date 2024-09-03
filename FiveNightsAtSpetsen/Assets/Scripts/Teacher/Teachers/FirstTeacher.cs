using System.Collections.Generic;

public class FirstTeacher : Teacher
{
    void Start()
    {
        delays = new() {
            ("Start", 2f),
            ("wawa", 0.5f)
        };
    }
}