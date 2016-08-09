
using System.Text;

public class SpeedString
{
    string string_base;
    StringBuilder string_builder;
    char[] int_parser = new char[11];

    public SpeedString(int capacity)
    {
        string_builder = new StringBuilder(capacity, capacity);
        string_base = (string)string_builder.GetType().GetField(
            "_str",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance).GetValue(string_builder);
    }

    public void Clear()
    {
        string_builder.Length = 0;
        for (int i = 0; i < string_builder.Capacity; i++)
        {
            string_builder.Append(' ');
        }
        string_builder.Length = 0;
    }

    public SpeedString Append(string value)
    {
        string_builder.Append(value);
        return this;
    }

    public SpeedString Append(int value)
    {
        int count;
        if (value >= 0)
        {
            count = ToCharArray((uint)value, int_parser, 0);
        }
        else
        {
            int_parser[0] = '-';
            count = ToCharArray((uint)-value, int_parser, 1) + 1;
        }
        for (int i = 0; i < count; i++)
        {
            string_builder.Append(int_parser[i]);
        }
        return this;
    }

    private static int ToCharArray(uint value, char[] buffer, int bufferIndex)
    {
        if (value == 0)
        {
            buffer[bufferIndex] = '0';
            return 1;
        }
        int len = 1;
        for (uint rem = value / 10; rem > 0; rem /= 10)
        {
            len++;
        }
        for (int i = len - 1; i >= 0; i--)
        {
            buffer[bufferIndex + i] = (char)('0' + (value % 10));
            value /= 10;
        }
        return len;
    }

    public override string ToString()
    {
        return string_base;
    }
}