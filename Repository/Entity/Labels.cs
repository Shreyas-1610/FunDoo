using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;  
using System.Text;
using System.Text.Json.Serialization;

namespace Repository.Entity
{
    public class Labels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        [ForeignKey("LabelNote")]
        public int NotesId { get; set; }
        [ForeignKey("LabelUser")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual Notes LabelNote { get; set; }
        [JsonIgnore]
        public virtual Users LabelUser { get; set; }
    }
}
