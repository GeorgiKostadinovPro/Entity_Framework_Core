using MinionsDBExercises.IO.Contracts;
using System;
using System.IO;

namespace MinionsDBExercises.IO
{
    public class SQLReader : IReader
    {
        private readonly string fileName;

        public SQLReader(string fileName)
        {
            this.fileName = fileName;
        }

        public string Read()
        {
            string currentDirectoryPath = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(currentDirectoryPath, $"../Queries/{this.fileName}.sql");
            
            string query = File.ReadAllText(fullPath);

            return query;
        }
    }
}
