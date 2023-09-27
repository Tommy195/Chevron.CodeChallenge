using System;

namespace Chevron.CodeChallenge.Models
{
    public class Boxer
    {
        public Boxer(string description, string name, string nationality)
        {
            Description = description;
            Name = name;
            Nationality = nationality;
            Id = Guid.NewGuid();
        }

        public Boxer(Guid id, string description, string name, string nationality)
        {
            Id = id;
            Description = description;
            Name = name;
            Nationality = nationality;
        }

        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public string Name { get; private set; }
        public string Nationality { get; private set; }
    }
}
