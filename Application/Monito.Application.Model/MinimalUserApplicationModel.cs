using System;

namespace Monito.Application.Model
{
    public class MinimalUserApplicationModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string IP { get; set; }
        public Guid UUID { get; set; }
    }
}