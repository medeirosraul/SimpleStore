using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleStore.Entities
{
    /// <summary>
    /// Base of all entities
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Entity identificator
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        /// <summary>
        /// When entity created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last entity modification
        /// </summary>
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// If entity is deleted
        /// </summary>
        public bool Deleted { get; set; }
    }
}
