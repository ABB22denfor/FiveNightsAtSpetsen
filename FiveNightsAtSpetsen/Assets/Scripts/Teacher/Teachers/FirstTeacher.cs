using System.Collections.Generic;

public class FirstTeacher : Teacher
{
    protected override void Init()
    {
        delays = new() {
            ("Start", 2f),
            ("wawa", 0.5f)
        };
    }
}