using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wcss
{
    public partial class Employee_Principal : _PrincipalBase.Principaled
    {
        public Employee Employee { get; set; }

        public Employee_Principal() : base() { }

        public Employee_Principal(Employee employee) : base(employee)
        {
            Employee = employee;
        }
        
        public static void SortListByPrincipal(_Enums.Principal prince, ref List<Employee> list)
        {
            list = new List<Employee>(list.OrderBy(x => new Employee_Principal(x).PrincipalWeight(prince))
                .ThenBy(x => new Employee_Principal(x).PrincipalOrder_Get(prince))
                .ThenBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ThenBy(x => x.EmailAddress_Derived));
        }

        public void SyncOrdinals()
        {
            this.Employee.VcJsonOrdinal = JsonConvert.SerializeObject(base.SyncOrds());
        }

        private List<_PrincipalBase.PrincipalOrdinal> _principalOrdinalList = null;
        public override List<_PrincipalBase.PrincipalOrdinal> PrincipalOrdinalList
        {
            get
            {

                if (_principalOrdinalList == null)
                {
                    //determine that this collection is a MailerShow Collection
                    if (this.Employee.VcJsonOrdinal != null && this.Employee.VcJsonOrdinal.Trim().Length > 0)
                    {
                        try
                        {
                            _principalOrdinalList = JsonConvert.DeserializeObject<List<_PrincipalBase.PrincipalOrdinal>>(this.Employee.VcJsonOrdinal);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);

                            //create a default
                            _principalOrdinalList = new List<_PrincipalBase.PrincipalOrdinal>();
                        }
                    }
                    else
                        _principalOrdinalList = new List<_PrincipalBase.PrincipalOrdinal>();
                }

                return _principalOrdinalList;
            }
            set
            {
                this.Employee.VcJsonOrdinal = JsonConvert.SerializeObject(value);
                _principalOrdinalList = null;
            }
        }
    }
    
    public partial class Employee : _PrincipalBase.IPrincipal
    {
        [XmlAttribute("Login_Derived")]
        public string Login_Derived
        {
            get { return (this.Login != null) ? this.Login.Trim() : string.Empty; }
            set { this.Login = value; }
        }
        [XmlAttribute("EmailAddress_Derived")]
        public string EmailAddress_Derived
        {
            get { return (this.EmailAddress != null) ? this.EmailAddress.Trim().ToLower() : string.Empty; }
            set { this.EmailAddress = value.ToLower(); }
        }
        [XmlAttribute("IsListInDirectory")]
        public bool IsListInDirectory
        {
            get { return this.BListInDirectory; }
            set { this.BListInDirectory = value; }
        }
        [XmlAttribute("Password")]
        public string Password
        {
            get { return Utils.Crypt.Decrypt(this.EPassword,this.EmailAddress_Derived); }
            set { this.EPassword = Utils.Crypt.Encrypt(value, this.EmailAddress_Derived); }
        }

        #region DataSource methods

        /// <summary>
        /// 
        /// </summary>
        /// <principal>_Enums.Principal ToString()</principal>
        /// <status>_Enums.CollectionSearchCriteriaStatusType ToString()</status>
        public static EmployeeCollection GetEmployeesInContext(int startRowIndex, int maximumRows,
            string principal, string status, DateTime startDate, DateTime endDate, string searchTerms)
        {
            EmployeeCollection coll = new EmployeeCollection();

            coll.LoadAndCloseReader(SPs.TxGetEmployeesInContext(
                startRowIndex, maximumRows, principal, status, searchTerms).GetReader());

            return coll;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <principal>_Enums.Principal ToString()</principal>
        /// <status>_Enums.CollectionSearchCriteriaStatusType ToString()</status>
        public static int GetEmployeesInContextCount(
            string principal, string status, DateTime startDate, DateTime endDate, string searchTerms)
        {
            int count = 0;

            using (System.Data.IDataReader dr = SPs.TxGetEmployeesInContextCount(
                principal, status, searchTerms).GetReader())
            {
                while (dr.Read())
                    count = (int)dr.GetValue(0);
                dr.Close();
            }

            return count;
        }

        #endregion
    }
}
