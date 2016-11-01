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

        public IQueryable<客戶資料> SelectByKeyWord(string keyword)
        {
            return this.All().Where(x => x.Is刪除 == false && (x.客戶名稱.Contains(keyword) || x.統一編號.Contains(keyword) || x.電話.Contains(keyword) || x.傳真.Contains(keyword) || x.地址.Contains(keyword) || x.Email.Contains(keyword))).OrderBy(x => x.Id);
        }
    }

    public  interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}