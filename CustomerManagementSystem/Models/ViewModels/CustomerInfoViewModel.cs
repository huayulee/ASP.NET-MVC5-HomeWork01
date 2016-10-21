using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace CustomerManagementSystem.Models.ViewModels
{
    public class CustomerInfoViewModel
    {
        public IPagedList<客戶聯絡人> 客戶聯絡人s { get; set; }

        public IPagedList<客戶銀行資訊> 客戶銀行資訊s { get; set; }
    }
}