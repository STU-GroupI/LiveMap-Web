using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Domain.Models;
public class ApprovalStatus
{
    public static ApprovalStatus APPROVED = new() { Status = "Approved" };
    public static ApprovalStatus PENDING = new() { Status = "Pending" };
    public static ApprovalStatus REJECTED = new() { Status = "Rejected" };

    public required string Status { get; set; }
}
