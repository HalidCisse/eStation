using System;
using System.Collections.Generic;
using eStationCore.Model.Hr.Entity;
using eStationCore.Model.Hr.Views;

namespace eStationCore.IManagers
{
    public interface IPayrollManager
    {
        bool AddEmployment(Employment employ);
        bool AddSalary(Salary salary);
        bool CancelPaycheck(Guid payrollGuid);
        bool CancelSalary(Salary salary);
        Employment GetEmployment(Guid employGuid);
        List<EmploymentCard> GetEmployments(Guid satffGuid, DateTime? startDate = default(DateTime?), DateTime? endDate = default(DateTime?));
        List<PayrollCard> GetPayrolls(Guid staffGuid, DateTime? fromDate, DateTime? toDate);
        List<SalaryCard> GetSalaries(Guid employGuid);
        Salary GetSalary(Guid salaryGuid);
        int GetStaffTotalDue(Guid staffGuid);
        double GetStaffTotalPaid(Guid staffGuid, DateTime? startDate, DateTime? endDate);
        bool Paycheck(Guid payrollGuid, double? finalPaycheck = default(double?), string numeroReference = null, TimeSpan? totalHoursWorked = default(TimeSpan?));
        bool UpdateEmployment(Employment employ);
    }
}