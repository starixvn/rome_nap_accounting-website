using System;

namespace Fanex.Tarus.Customer.Entities
{
    [Serializable]
    public class BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
