using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Domain.Models;
public class PointOfInterest
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    // Enum yet to be declared
    //public string Status { get; set; }

    // Object yet to be declared
    //public Location Location { get; set; }

    // Object yet to be declared
    // public Category Category { get; set; }
}