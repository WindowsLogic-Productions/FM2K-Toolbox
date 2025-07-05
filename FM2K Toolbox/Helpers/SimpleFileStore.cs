using System;
using System.Collections.Generic;
using System.IO;

// Helper to handle save, remove and append lines to .ini file
public class SimpleFileStore
{
    private readonly string filePath;

    public SimpleFileStore(string filePath)
    {
        this.filePath = filePath;
    }

    // Insert line to the file
    public void AppendLine(string line)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(line);
        }
    }

    // Read lines from the file
    public List<string> LoadLines()
    {
        List<string> lines = new List<string>();

        if (!File.Exists(filePath))
            return lines;

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }

    // remove lines from file
    public void RemoveLines(List<string> linesToRemove)
    {
        if (!File.Exists(filePath))
            return;

        List<string> lines = LoadLines();

        foreach (string item in linesToRemove)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i] == item)
                {
                    lines.RemoveAt(i);
                }
            }
        }

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }

    // remove line from file
    public void RemoveLine(string line)
    {
        RemoveLines(new List<string> { line });
    }
}
