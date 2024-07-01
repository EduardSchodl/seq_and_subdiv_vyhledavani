class Class
{
    /*
     * Metoda rozhoduje, zda se predana hodnota x nachazi v predanem serazenem poli
     */
    static bool IntervalSubdivision(int[] data, int x)
    {
        int left = 0; //leva hranice intervalu
        int right = data.Length - 1; //prava hranice intervalu
        int mid = (left + right) / 2; //index uprostred intervalu [0, 1, 2, 4]
        while (data[mid] != x)
        {
            if (left >= right)
            {
                return false;
            }
            //nyni zmensime interval	
            if (data[mid] > x)
            {
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
            mid = (left + right) / 2;
        }
        return true;
    }

    /// <summary>
    /// Metoda generuje náhodný počet <paramref name="size"/> seřazených celý čísel 
    /// </summary>
    /// <param name="size">Počet čísel</param>
    /// <returns>Vrací pole seřazených celých čísel o dané velikosti</returns>
    public static int[] GenerateData(int size)
    {
        int[] data = new int[size];
        Random r = new Random();
        data[0] = 1;
        for(int i = 0; i < data.Length; i++)
        {
            data[i+1] = data[i] + r.Next(1, 6);
        }
        return data;
    }
    

    public static bool TestFinderMatch(IFinder seq, IFinder div)
    {
        int[] data = GenerateData(1_000);
        int x = 50;
        if (seq.Find(x, data) == div.Find(x, data))
        {
            return true;
        }
        return false;
    }

    public static string TestFinderSpeed(IFinder finder)
    {
        int[] data = GenerateData(10_000_000);

        double counter = 0;
        for(int i = 0; i < 1000; i++)
        {
            DateTime start = DateTime.Now;
            bool found = finder.Find(i, data);
            DateTime stop = DateTime.Now;
            counter += (stop - start).TotalSeconds;
        }

        return  counter + " seconds";
    }

    /// <summary>
    /// Metoda testuje rychlost hledání náhodného čísla <paramref name="x"/> pomocí algoritmu specifikovaného pomocí <paramref name="finder"/> na poli celých čísel
    /// <paramref name="data"/>
    /// </summary>
    /// <param name="finder">Testovaný algoritmus hledání</param>
    /// <param name="data">Pole celých čísel</param>
    /// <param name="x">Hledané číslo</param>
    /// <returns>Vrací uplynulý čas za dobu hledání</returns>
    public static double TestFinderSpeed(IFinder finder, int[] data, int x)
    {
        DateTime start = DateTime.Now;
        bool found = finder.Find(x, data);
        DateTime stop = DateTime.Now;        

        return (stop - start).TotalSeconds;
    }

    /// <summary>
    /// Metoda načte data ze souboru předané cesty k souboru <paramref name="filePath"/>
    /// </summary>
    /// <param name="filePath">Cesta k souboru</param>
    /// <returns>Vrací pole celých čísel načtených ze souboru</returns>
    public static int[] LoadDataFromTxt(string filePath)
    {
        string[] data = File.ReadAllLines(filePath);
        return Array.ConvertAll(data, num => Int32.Parse(num));
    }

    /// <summary>
    /// Metoda zkontroluje, zda je předané pole celých čísel seřazené
    /// </summary>
    /// <param name="data">Pole celých čísel</param>
    /// <returns>True, pokud je pole seřazené, jinak False</returns>
    public static bool IsSorted(int[] data)
    {
        for(int i = 0; i < data.Length-1; i++)
        {
            if (data[i] > data[i + 1])
            {
                return false;
            }
        }
        return true;
    }

    public static void Main(string[] args)
    {
        /*
        //int[] data = new int[] { 1, 2, 3 };
        int[] data = [1, 3, 5, 41, 48, 52, 63, 71];
        DateTime start = DateTime.Now;
        bool found = IntervalSubdivision(data, 53);
        DateTime stop = DateTime.Now;
        Console.WriteLine("Interval subdivision finished in " + (stop - start).TotalMilliseconds * 1000 + " ns");
        Console.WriteLine("Number found: " + found);
        */
        
        IFinder div = new IntervalSubdivisionFinder();
        IFinder seq = new SequentialFinder();
        /*
        Console.WriteLine($"Vrací stejně? - {TestFinderMatch(seq, div)}");
        
        Console.WriteLine($"Rychlost sekvenční - {TestFinderSpeed(seq)}");
        Console.WriteLine($"Rychlost půlení - {TestFinderSpeed(div)}");
        */
        Random r = new Random();
        for(int i = 1; i <= 10; i++)
        {
            string filePath = $"Exercise03/seq{i}.txt";
            int[] data = LoadDataFromTxt(filePath);
            if (IsSorted(data))
            {
                Console.WriteLine(filePath);
                Console.WriteLine($"Počet prvků v posloupnosti - {data.Length}");
                double seqTime = 0.0;
                double divTime = 0.0;
                for(int j = 0; j < 10000; j++)
                {
                    int randomNum = r.Next(data[0], data[data.Length - 1] + 1);
                    seqTime += TestFinderSpeed(seq, data, randomNum);
                    divTime += TestFinderSpeed(div, data, randomNum);
                }
                //Console.WriteLine($"Hledané číslo - {randomNum}");
                Console.WriteLine($"Rychlost sekvenční - {seqTime} sekund");
                Console.WriteLine($"Rychlost půlení - {divTime} sekund");
                Console.WriteLine($"Půlení intervalu je rychlejší o {seqTime/divTime}");
                Console.WriteLine();
            }
        }
    }
}

interface IFinder
{
    bool Find(int x, int[] data);
}

class IntervalSubdivisionFinder : IFinder
{
    /// <summary>
    /// Metoda hledá hledané celé číslo <paramref name="x"/> v poli celých čísel <paramref name="data"/> pomocí půlení intervalu hledání.
    /// </summary>
    /// <param name="x">Hledané celé číslo</param>
    /// <param name="data">Pole celých čísel</param>
    /// <returns>Vrací True pokud hledané číslo je v poli, jinak False</returns>
    public bool Find(int x, int[] data)
    {
        int left = 0; //leva hranice intervalu
        int right = data.Length - 1; //prava hranice intervalu
        int mid = (left + right) / 2; //index uprostred intervalu [0, 1, 2, 4]
        while (data[mid] != x)
        {
            if (left >= right)
            {
                return false;
            }
            //nyni zmensime interval	
            if (data[mid] > x)
            {
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
            mid = (left + right) / 2;
        }
        return true;
    }
}

class SequentialFinder : IFinder
{
    /// <summary>
    /// Metoda hledá hledané celé číslo <paramref name="x"/> v poli celých čísel <paramref name="data"/> pomocí sekvenčního hledání.
    /// </summary>
    /// <param name="x">Hledané celé číslo</param>
    /// <param name="data">Pole celých čísel</param>
    /// <returns>Vrací True pokud hledané číslo je v poli, jinak False</returns>
    public bool Find(int x, int[] data)
    {
        for(int i = 0; i < data.Length; i++)
        {
            if (data[i] == x)
            {
                return true;
            }
        }
        return false;
    }
}