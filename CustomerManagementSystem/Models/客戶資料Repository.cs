using System;
using System.Linq;
using System.Collections.Generic;
    
namespace CustomerManagementSystem.Models
{   
    public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => p.Is刪除 == false);
        }

        public override void Delete(客戶資料 entity)
        {
            entity.Is刪除 = true;
        }

        public void Delete(int id)
        {
            var data = this.Find(id);
            this.Delete(data);
        }

        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<客戶資料> SelectByKeyWord(string keyword, int? 客戶分類Id)
        {
            var data = this.All().Where(p => (p.客戶名稱.Contains(keyword) || p.統一編號.Contains(keyword) || p.電話.Contains(keyword) || p.傳真.Contains(keyword) || p.地址.Contains(keyword) || p.Email.Contains(keyword)));
            if (客戶分類Id != null)
            {
                data = data.Where(p => p.客戶分類Id == 客戶分類Id);
            }

            return data.OrderBy(p => p.Id);
        }
    }

    public  interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}