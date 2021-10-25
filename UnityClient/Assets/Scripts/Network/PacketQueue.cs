using Google.Protobuf;
using System.Collections.Generic;

public class Packet
{
    public ushort Id { get; set; }
    public IMessage Message { get; set; }
}
public class PacketQueue
{
    private static PacketQueue instance = new PacketQueue();
    public static PacketQueue Instance { get { return instance; } }

    Queue<Packet> packetQueue = new Queue<Packet>();

    public void Push(ushort id, IMessage message)
    {
        packetQueue.Enqueue(new Packet() { Id = id, Message = message });
    }

    public Packet Pop()
    {
        if (packetQueue.Count > 0)
        {
            return packetQueue.Dequeue();
        }

        return null;
    }

    public List<Packet> PopAll()
    {
        List<Packet> list = new List<Packet>();

        while (packetQueue.Count > 0)
        {
            list.Add(packetQueue.Dequeue());
        }

        return list;
    }
}
