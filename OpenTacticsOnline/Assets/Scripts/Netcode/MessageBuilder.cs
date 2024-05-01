using System.Collections.Generic;

public class MessageBuilder
{
    public const char Separator = '|';
    
    private readonly NetworkSignifier signifier;
    private readonly List<string> values;
    
    public MessageBuilder(NetworkSignifier signifier)
    {
        this.signifier = signifier;
        values = new List<string>();
    }

    public MessageBuilder AddValue(object value)
    {
        values.Add(value.ToString());
        return this;
    }

    public string GetMessage()
    {
        string msg = $"{(int)signifier}";
        foreach (string value in values)
        {
            msg += $"{Separator}{value}";
        }
        return msg;
    }
}