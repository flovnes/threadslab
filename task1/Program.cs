public class Warehouse {
    public enum ItemStatus { AVAILABLE, PRE_ORDER, UNAVAILABLE }

    public ItemStatus FetchBookStatus() {
        Thread.Sleep(2000); 
        return (ItemStatus)new Random().Next(0, 3);
    }
}

public class Bookstore {
    public static void Main() {
        var warehouse = new Warehouse();
        var titles = new List<string> {
            "rustbook part 1",
            "rustbook part 2",
            "rustbook part 3",
            "rustbook part 4",
            "rustbook part 5",
            "rustbook part 6"
        };

        long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        using (var latch = new CountdownEvent(titles.Count)) {
            foreach (var title in titles) {
                string currentTitle = title;

                Thread t = new(() => {
                    Console.WriteLine($"{currentTitle}: {warehouse.FetchBookStatus()}");

                    latch.Signal();
                });

                t.Start();
            }

            latch.Wait();
        }

        long end = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        Console.WriteLine($"{end - start} ms");
    }
}