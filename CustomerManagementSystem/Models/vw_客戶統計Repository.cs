using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CustomerManagementSystem.Models
{   
	public  class vw_客戶統計Repository : EFRepository<vw_客戶統計>, Ivw_客戶統計Repository
	{

	}

	public  interface Ivw_客戶統計Repository : IRepository<vw_客戶統計>
	{

	}
}