using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Uplift.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Serivce Name")]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Display(Name = "Description")]
        public string LongDesc { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; } // campo para ligar a foreignkey de category

        [ForeignKey("CategoryId")] // tem que ser igual a propriedade CategoryID
        public Category Category { get; set; } // Obviamente, não será criada uma tabela no banco para esta propriedade pois ela é FK

        [Required]
        public int FrequencyId { get; set; }

        [ForeignKey("FrequencyId")]
        public Frequency Frequency { get; set; }
    }
}
