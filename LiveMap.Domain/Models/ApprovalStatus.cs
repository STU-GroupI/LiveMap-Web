using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Domain.Models;
public class ApprovalStatus
{
    public static readonly string APPROVED = "Approved";
    public static readonly string PENDING = "Pending";
    public static readonly string REJECTED = "Rejected";
    public required string Status { get; set; }
}
