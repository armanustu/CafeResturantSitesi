using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CafeResturant.Models
{
    //Burası veri tabanında Application user tablosunu temsil eder buraya property ekleyip nuget package konsolunda code first yöntemi uygularsak yani aşağıdaki örnekteki gibi property ekler sonra  add-migration addsehir sonra update-database yazarsak bu bilgiler Veri tabanında bulunan ApplicationUser tablosuna eklenecektir biz bu yöntemle Idenity framework yapısını kullanarak kişinin profilini güncelleyebiliriz  

    //????????? Sorulacak :Applicationuser tablosuna sehir ekledim Identity/Account/Manage/Index sayfasında güncelleme yapamadım kod değişiminede
    //Identity User interfaceye ne metod yada property ekliyebiliyorum 
	public class ApplicationUser :IdentityUser
	{
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Role { get; set; }

        public string Sehir { get; set; }
    }
}
