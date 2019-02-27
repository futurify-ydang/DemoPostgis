using System;
using System.ComponentModel.DataAnnotations;

namespace DemoPostgres
{
    public enum State
    {
        Active,
        Inactive
    }

    [Serializable]
    public class MetaDataBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public State State { get; set; }
        public bool IsDisabled { get; set; }
    }
}