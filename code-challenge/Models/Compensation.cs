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
        public String CompensationID { get; set; }
        public String EmployeeID { get; set; }
        public double Salary { get; set; }
        public DateTime EffectiveDate { get; set; }

        public static bool operator == (  Compensation first,  Compensation second ) 
        {
            bool result = false;
            if (!Object.ReferenceEquals(first, null) && !Object.ReferenceEquals(second, null))
            {
                result =    (first.CompensationID == second.CompensationID);
                result &=   (first.EmployeeID == second.EmployeeID);
                result &=   (first.Salary == second.Salary);
                result &=   (first.EffectiveDate == second.EffectiveDate);
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
