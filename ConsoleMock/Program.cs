// Путь к файлу
string path = "test.txt";

try
{
    // Чтение всех строк файла
    string[] lines = File.ReadAllLines(path);

    // Построчный вывод содержимого файла
    foreach (string line in lines)
    {
        Console.WriteLine(line);
    }
}
catch (Exception ex)
{
    // В случае ошибки выводим информацию об ошибке
    Console.WriteLine($"Не удалось прочитать файл: {ex.Message}");
}