using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "python";
        start.Arguments = "C:\\Users\\tibo9\\source\\repos\\PythonApplication1\\PythonApplication1\\Test.py";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;

        using (Process process = Process.Start(start))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
            }
        }
    }
}
