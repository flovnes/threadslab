public interface Interface {
    void Exec(Action r);
}

public class SmartFlag : Interface {
    int flag = 0;

    public void Exec(Action r) {
        if (Interlocked.Exchange(ref flag, 1) == 0)
            r();
    }
}

class Program {
    static void Main() {
        SmartFlag once = new();
        for (int i = 0; i < 9999; i++)
            new Thread(() => once.Exec(() => Console.WriteLine("!"))).Start();
    }
}