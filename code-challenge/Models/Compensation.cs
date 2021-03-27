using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        [Key]
        public String compensationID { get; set; }
        public String employeeID { get; set; }
        public double salary { get; set; }
        public DateTime effectiveDate { get; set; }

        public static bool operator == (  Compensation first,  Compensation second ) 
        {
            bool result = false;
            if (!Object.ReferenceEquals(first, null) && !Object.ReferenceEquals(second, null))
            {
                result =    (first.compensationID == second.compensationID);
                result &=   (first.employeeID == second.employeeID);
                result &=   (first.salary == second.salary);
                result &=   (first.effectiveDate == second.effectiveDate);
            }
            else if (Object.ReferenceEquals(first, null) && Object.ReferenceEquals(second, null)) 
            {
                result = true;
            }
            return result;
        }

        public static bool operator != ( Compensation first,  Compensation second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

    }
}
