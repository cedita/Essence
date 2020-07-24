using System.ComponentModel.DataAnnotations;

namespace Cedita.Essence.EntityFrameworkCore.Audit
{
    /// <summary>
    /// Table that holds all table names in the database.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Gets or sets the Table Name.
        /// </summary>
        [MaxLength(128)]
        public string Name { get; set; }
    }
}
