using System;
using System.Linq;
using System.Collections.Generic;

namespace CustomerManagementSystem.Models
{   
	public  class 客戶分類Repository : EFRepository<客戶分類>, I客戶分類Repository
	{
		public override IQueryable<客戶分類> All()
		{
			return base.All().Where(p => p.Is刪除 == false);
		}

        public IQueryable<客戶分類> SelectAllWithKeyword(string keyword)
        {
            return this.All().Where(p => p.分類名稱.Contains(keyword)).OrderBy(p => p.分類名稱);
        }

        public 客戶分類 Find(int id)
        {
            return base.All().FirstOrDefault(p => p.Id == id);
        }
    }

	public  interface I客戶分類Repository : IRepository<客戶分類>
	{

	}
}