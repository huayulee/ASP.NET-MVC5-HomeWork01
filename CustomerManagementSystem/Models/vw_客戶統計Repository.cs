using System;
using System.Linq;
using System.Collections.Generic;
    
namespace CustomerManagementSystem.Models
{   
    public  class vw_客戶統計Repository : EFRepository<vw_客戶統計>, Ivw_客戶統計Repository
    {
        public IQueryable<vw_客戶統計> SelectByKeyWord(string keyword)
        {
            return base.All().Where(x => x.客戶名稱.Contains(keyword)).OrderBy(x => x.客戶名稱);
        }
    }

    public  interface Ivw_客戶統計Repository : IRepository<vw_客戶統計>
    {

    }
}