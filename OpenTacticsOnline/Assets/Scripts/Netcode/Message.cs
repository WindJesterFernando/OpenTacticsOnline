public class Message
{
    public readonly NetworkSignifier signifier;
    public readonly string[] values;
   
    public Message(string message)
    {
         string[] csv = message.Split(MessageBuilder.Separator); 
         signifier = (NetworkSignifier)int.Parse(csv[0]);
         values = csv[1..];
    }
}