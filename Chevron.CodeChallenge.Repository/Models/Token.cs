using System;

namespace Chevron.CodeChallenge.Models
{
    public class Token
    {
        public Guid? Id { get; set; }
        public bool NeedsRefresh { get; set; }
    }
}
