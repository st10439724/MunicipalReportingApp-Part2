using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MunicipalReportingApp.Models;

namespace MunicipalReportingApp.Services
{
    

    public sealed class IssueRepository
    {
        private static readonly Lazy<IssueRepository> _instance = new(() => new IssueRepository());
        public static IssueRepository Instance => _instance.Value;

        private readonly List<IssueReport> _issues = new();
        private readonly string _storageFile;
        private int _nextId = 1;

        private IssueRepository()
        {
            _storageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "issues.json");
            LoadFromDisk();
        }

        
        public IReadOnlyList<IssueReport> Issues => _issues.AsReadOnly();

       
        public int Count => _issues.Count;

        
        public IssueReport Add(string location, string streetName, string category, string description, List<string> attachments)
        {
            var issue = new IssueReport
            {
                Id = _nextId++,
                Location = location,
                StreetName = streetName,
                Category = category,
                Description = description,
                AttachmentPaths = attachments ?? new List<string>(),
                DateReported = DateTime.Now,
                Status = "Submitted"
            };

            _issues.Add(issue);
            SaveToDisk();
            return issue;
        }

        private void SaveToDisk()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_issues, options);
                File.WriteAllText(_storageFile, json);
            }
            catch
            {
                
                // A failure here (e.g. read-only folder) should never crash the app. (test)
            }
        }

        private void LoadFromDisk()
        {
            try
            {
                if (!File.Exists(_storageFile))
                {
                    return;
                }

                var json = File.ReadAllText(_storageFile);
                var loaded = JsonSerializer.Deserialize<List<IssueReport>>(json);

                if (loaded is { Count: > 0 })
                {
                    _issues.AddRange(loaded);
                    _nextId = _issues.Max(i => i.Id) + 1;
                }
            }
            catch
            {
                
            }
        }
    }
}
