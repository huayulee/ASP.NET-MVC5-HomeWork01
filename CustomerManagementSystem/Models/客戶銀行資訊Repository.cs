using System;
using System.Linq;
using System.Collections.Generic;
    
namespace CustomerManagementSystem.Models
{   
    public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        public override IQueryable<客戶銀行資訊> All()
        {
            
            return base.All().Where(p => p.Is刪除 == false);
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.Is刪除 = true;
        }

        public void Delete(int id)
        {
            var data = this.Find(id);
            this.Delete(data);
        }

        public void Delete客戶的所有銀行資訊By客戶Id(int 客戶Id)
        {
            var 客戶所有營行資訊 = this.All().Where(x => x.客戶Id == 客戶Id);
            foreach (客戶銀行資訊 item in 客戶所有營行資訊)
            {
                this.Delete(item.Id);
            }
        }

        public IQueryable<客戶銀行資訊> SelectByKeyWord(string keyword)
        {
            return this.All().Where(x => (x.銀行名稱.Contains(keyword) || (x.銀行代碼.ToString()).Contains(keyword) || x.帳戶名稱.Contains(keyword) || x.帳戶號碼.Contains(keyword) || x.客戶資料.客戶名稱.Contains(keyword))).OrderBy(x => x.Id);
        }

        public 客戶銀行資訊 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
    }

    public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
    {

    }
}