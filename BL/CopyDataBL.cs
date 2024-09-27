using EmployeeAccess.DA;
using EmployeeAccess.Model;

namespace EmployeeAccess.BL
{
    public class CopyDataBL
    {
        public static string DataCopy(CopyData CopyData)
        {
            return CopyDataDA.DataCopy(CopyData);
        
        }

        public static int GetCountofTargetDataInserted(CopyData CopyData)
        {
            return CopyDataDA.GetCountofTargetDataInserted(CopyData);
        }
    }
}
