public class CoolMessage { public string content; }

public interface Queueble {
    void Add(CoolMessage message);
    CoolMessage Poll();
}

public class CoolQueue(int a) : Queueble {
    private Queue<CoolMessage> q = new();
    private int size = a;
    private object monitor = new object();

    public void Add(CoolMessage message) {
        lock (monitor) {
            while (q.Count >= size)
                Monitor.Wait(monitor);

            q.Enqueue(message);
            Monitor.PulseAll(monitor);
        }
    }

    public CoolMessage Poll() {
        lock (monitor) {
            while (q.Count == 0)
                Monitor.Wait(monitor);

            CoolMessage msg = q.Dequeue();
            Monitor.PulseAll(monitor);
            return msg;
        }
    }
}

class Program {
    static void Main() {
        CoolQueue q = new(1);

        new Thread(() => {
            var msg = q.Poll();
            Console.WriteLine($"hi from {msg.content}");
        }).Start();

        Thread.Sleep(333); 

        new Thread(() => { q.Add(new CoolMessage { content = "the future" }); }).Start();
    }
}