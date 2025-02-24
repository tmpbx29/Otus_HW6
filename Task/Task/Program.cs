using System.Diagnostics;
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Укажите директорию с файлами:");
        var directory = Console.ReadLine();

        // Пример для первого задания: 3 файла
        var stopwatch = Stopwatch.StartNew();
        int totalSpaces = await ProcessThreeFilesAsync(
                directory + "\\file1.txt",
                directory + "\\file2.txt",
                directory + "\\file3.txt");
        stopwatch.Stop();
        
        Console.WriteLine($"Общее количество пробелов: {totalSpaces}");
        Console.WriteLine($"Время выполнения (3 файла): {stopwatch.ElapsedMilliseconds} мс\n");

        // Пример для второго задания: все файлы в папке
        stopwatch.Restart();
        int totalSpacesInDir = CountSpacesInDirectory(directory);
        stopwatch.Stop();
        
        Console.WriteLine($"Общее количество пробелов в папке: {totalSpacesInDir}");
        Console.WriteLine($"Время выполнения (папка): {stopwatch.ElapsedMilliseconds} мс");
    }

    // Метод для обработки трёх файлов
    static async Task<int> ProcessThreeFilesAsync(string file1, string file2, string file3)
    {
        var task1 = Task.Run(() => CountSpacesInFile(file1));
        var task2 = Task.Run(() => CountSpacesInFile(file2));
        var task3 = Task.Run(() => CountSpacesInFile(file3));

        await Task.WhenAll(task1, task2, task3);
        return task1.Result + task2.Result + task3.Result;
    }

    // Метод для подсчёта пробелов в одном файле
    static int CountSpacesInFile(string filePath)
    {
        try
        {
            string text = File.ReadAllText(filePath);
            return text.Count(c => c == ' ');
        }
        catch
        {
            Console.WriteLine($"Ошибка чтение файла: {filePath}");
            return 0;
        }
    }

    // Метод для обработки всех файлов в папке
    static int CountSpacesInDirectory(string directoryPath)
    {
        string[] files = Directory.GetFiles(directoryPath);
        Task<int>[] tasks = new Task<int>[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string currentFile = files[i]; // Фиксируем значение для каждой задачи
            tasks[i] = Task.Run(() => CountSpacesInFile(currentFile));
        }

        Task.WaitAll(tasks);
        return tasks.Sum(t => t.Result);
    }
}